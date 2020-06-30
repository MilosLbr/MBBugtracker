using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataModels.ViewModels;
using DTOs;
using MbBugtracker.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MbBugtracker.Controllers.Api
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TicketsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        
        public async Task<IActionResult> GetTickets()
        {
            var tickets = await _context.Tickets.OrderBy(t=> t.TicketStatusId).ThenByDescending(t => t.TicketPriorityId).ToListAsync();

            var ticketList = _mapper.Map<List<TicketListDto>>(tickets);

            return Ok(ticketList);
        }

        [HttpPut("updateStatus")]
        public async Task<IActionResult> UpdateTicketStatus(TicketStatusUpdateDto ticketStatusUpdateDto)
        {
            var ticketFromDb = _context.Tickets.Find(ticketStatusUpdateDto.TicketId);

            if(ticketFromDb == null)
            {
                return BadRequest("Ticket with Id: " + ticketStatusUpdateDto.TicketId + " was not found!");
            }

            ticketFromDb.TicketStatusId = ticketStatusUpdateDto.TicketStatusId;

            if(await _context.SaveChangesAsync() >= 1)
            {
                var ticketStatusToReturn = await _context.TicketStatuses.FindAsync(ticketStatusUpdateDto.TicketStatusId);

                var ticketStatusToReturnDto = _mapper.Map<TicketStatusDto>(ticketStatusToReturn);
                return Ok(ticketStatusToReturnDto);
            }
            else
            {
                return BadRequest("An error has occured while updating ticket status!");
            }


        }
    }
}