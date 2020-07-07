using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataModels;
using DTOs;
using MbBugtracker.Data;
using MbBugtracker.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MbBugtracker.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketCommentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketCommentsController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }


        [HttpGet("{ticketId}")]
        public async Task<IActionResult> GetCommentsForTicket(int ticketId)
        {
            var ticketFromDb = await _unitOfWork.Tickets.GetById(ticketId);
            if(ticketFromDb == null)
            {
                return BadRequest("Invalid ticket Id!");
            }

            var comments = ticketFromDb.TicketComments;
            var commentsListDto = _mapper.Map<IEnumerable<TicketCommentListDto>>(comments);

            return Ok(commentsListDto);

        }

        [Authorize(Roles = "Admin,ProjectManager,Developer")]
        [HttpPost("{ticketId}")]        
        public async Task<IActionResult> PostCommentForTicket(int ticketId, TicketCommentCreateDto commentDto)
        {
            if (string.IsNullOrWhiteSpace(commentDto.Content))
            {
                return BadRequest("Please enter comment content!");
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var newComment = new TicketComment
            {
                CreatedBy = currentUser.UserName,
                CreatedByUserId = currentUser.Id,
                DateAdded = DateTime.Now,
                TicketId = ticketId,
                Content = commentDto.Content
            };

            _unitOfWork.TicketComments.Add(newComment);

            if(await _unitOfWork.Complete() >= 1)
            {
                var allCommentsForTicket = await _unitOfWork.TicketComments.Filter(tc => tc.TicketId == ticketId).OrderByDescending(tc=> tc.DateAdded).ToListAsync();
                var commentListDto = _mapper.Map<IEnumerable<TicketCommentListDto>>(allCommentsForTicket);

                return Ok(commentListDto);
            }
            else
            {
                return BadRequest("An error happened while posting new comment!");
            }
        }
    }
}