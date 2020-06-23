using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DataModels;
using DataModels.EnumConstants;
using DTOs;
using MbBugtracker.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MbBugtracker.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketResolutionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketResolutionController(ApplicationDbContext applicationDbContext, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = applicationDbContext;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet("{ticketId}")]
        public async Task<IActionResult> GetResolutionForTicket(int ticketId)
        {
            var ticketFromDb = await _context.Tickets.FindAsync(ticketId);

            if(ticketFromDb == null)
            {
                return BadRequest("Invalid ticket Id!");
            }

            var resolution = _mapper.Map<TicketResolutionDto>(ticketFromDb.TicketResolution);

            return Ok(resolution);
        }

        [HttpPost("{ticketId}")]
        public async Task<ActionResult> PostResolutionForTicket(int ticketId, TicketResolutionCommentDto ticketResolution)
        {
            if(string.IsNullOrEmpty(ticketResolution.ResolutionComment)){
                return BadRequest("Comment is required for resolving the ticket!");
            }

            var ticketFromDb = await _context.Tickets.FindAsync(ticketId);

            if (ticketFromDb == null)
            {
                return BadRequest("Invalid ticket Id!");
            }
            if(ticketFromDb.TicketResolution != null)
            {
                return BadRequest("Ticket is already resolved! Resoluiton Id : " + ticketFromDb.TicketResolution.Id + "!");
            }

            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var resolution = new TicketResolution
            {
                CreatedBy = currentUser.UserName,
                CreatedByUserId = currentUser.Id,
                DateCreated = DateTime.Now,
                ResolutionComment = ticketResolution.ResolutionComment,
                TicketId = ticketId
            };

            ticketFromDb.TicketStatusId = ticketResolution.TicketStatusId;

            _context.TicketResolution.Add(resolution);

            if(await _context.SaveChangesAsync() >=1)
            {
                return Ok("Created new ticket resolution!");
            }
            else
            {
                return BadRequest("An erron happened while saving new ticket resolution!");
            }


        }
    }
}