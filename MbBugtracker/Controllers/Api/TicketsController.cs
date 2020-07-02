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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TicketsController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        
        public async Task<IActionResult> GetTickets()
        {
            var tickets = await _unitOfWork.Tickets.GetAll();
            // order by status then by priority
            tickets = tickets.OrderBy(t => t.TicketStatusId).ThenByDescending(t => t.TicketPriorityId).ToList();

            var ticketListDto = _mapper.Map<List<TicketListDto>>(tickets);

            return Ok(ticketListDto);
        }

        [HttpPut("updateStatus")]
        public async Task<IActionResult> UpdateTicketStatus(TicketStatusUpdateDto ticketStatusUpdateDto)
        {
            var ticketFromDb = await _unitOfWork.Tickets.GetById(ticketStatusUpdateDto.TicketId);

            if(ticketFromDb == null)
            {
                return BadRequest("Ticket with Id: " + ticketStatusUpdateDto.TicketId + " was not found!");
            }

            ticketFromDb.TicketStatusId = ticketStatusUpdateDto.TicketStatusId;

            if(await _unitOfWork.Complete() >= 1)
            {
                var ticketStatusToReturn = await _unitOfWork.TicketStatuses.GetById(ticketStatusUpdateDto.TicketStatusId);

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