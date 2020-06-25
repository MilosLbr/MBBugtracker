using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataModels;
using DTOs;
using MbBugtracker.Data;
using Microsoft.AspNetCore.Authentication;
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
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketCommentsController(ApplicationDbContext applicationDbContext, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = applicationDbContext;
            _mapper = mapper;
            _userManager = userManager;
        }


        [HttpGet("{ticketId}")]
        public async Task<IActionResult> GetCommentsForTicket(int ticketId)
        {
            var ticketFromDb = await _context.Tickets.FindAsync(ticketId);
            if(ticketFromDb == null)
            {
                return BadRequest("Invalid ticket Id!");
            }

            var comments = ticketFromDb.TicketComments;
            var commentsListDto = _mapper.Map<IEnumerable<TicketCommentListDto>>(comments);

            return Ok(commentsListDto);

        }

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

            _context.TicketComments.Add(newComment);

            if(await _context.SaveChangesAsync() >= 1)
            {
                var allCommentsForTicket = await _context.TicketComments.Where(tc => tc.TicketId == ticketId).OrderByDescending(tc=> tc.DateAdded).ToListAsync();
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