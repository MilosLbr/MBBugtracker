using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DataModels.ViewModels;
using DTOs;
using MbBugtracker.Data;
using Microsoft.AspNetCore.Http;
using DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MbBugtracker.Services.Interfaces;

namespace MbBugtracker.Controllers.Api
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TicketsController(ApplicationDbContext context, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _context = context;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
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