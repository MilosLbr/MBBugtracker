using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DTOs;
using Microsoft.AspNetCore.Http;
using DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MbBugtracker.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace MbBugtracker.Controllers.Api
{
    [Route("api/tickets")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public TicketsController(IMapper mapper, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        
        public async Task<IActionResult> GetTickets()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            var tickets = await _unitOfWork.Tickets.GetTicketsForCurrentUsersProjects(currentUser);

            // order by status then by priority
            tickets = tickets
                .OrderBy(t => {
                    if(t.TicketStatusId == 2)
                    {
                        return 5;
                    }
                    else if(t.TicketStatusId == 5)
                    {
                        return 4;
                    }
                    else
                    {
                        return t.TicketStatusId;
                    }
                })
                .ThenByDescending(t => t.TicketPriorityId)
                .ToList();

            var ticketListDto = _mapper.Map<List<TicketListDto>>(tickets);

            return Ok(ticketListDto);
        }

        [Authorize(Roles = "Admin,ProjectManager,Developer")]
        [HttpPut("updateStatus")]
        public async Task<IActionResult> UpdateTicketStatus(TicketStatusUpdateDto ticketStatusUpdateDto)
        {
            var ticketFromDb = await _unitOfWork.Tickets.GetById(ticketStatusUpdateDto.TicketId);

            if(ticketFromDb == null)
            {
                return BadRequest("Ticket with Id: " + ticketStatusUpdateDto.TicketId + " was not found!");
            }

            ticketFromDb.TicketStatusId = ticketStatusUpdateDto.TicketStatusId;
            var ticketActivityLog = await LogStatusChange(ticketStatusUpdateDto.TicketId);

            if(await _unitOfWork.Complete() >= 1)
            {
                var ticketStatusToReturn = await _unitOfWork.TicketStatuses.GetById(ticketStatusUpdateDto.TicketStatusId);

                var activityAndStatusDto = PrepareActivityAndStatusDto(ticketStatusToReturn, ticketActivityLog);

                return Ok(activityAndStatusDto);
            }
            else
            {
                return BadRequest("An error has occured while updating ticket status!");
            }
        }

        private async Task<TicketActivityLog> LogStatusChange(int ticketId)
        {
            var appUser = await _userManager.GetUserAsync(User);
            var appUserId = appUser.Id;

            var activityLog = new TicketActivityLog()
            {
                ActivityDate = DateTime.Now,
                ActivityDescription = "Edited TicketStatusId",
                ApplicationUserId = appUserId,
                TicketId = ticketId,
                ApplicationUser = appUser
            };

            _unitOfWork.TicketActivityLogs.Add(activityLog);

            return activityLog;
        }

        private ActivityAndStatusDto PrepareActivityAndStatusDto(TicketStatus ticketStatusToReturn, TicketActivityLog ticketActivityLog)
        {
            var ticketStatusToReturnDto = _mapper.Map<TicketStatusDto>(ticketStatusToReturn);

            var ticketActivityLogToReturnDto = _mapper.Map<TicketActivityLogDto>(ticketActivityLog);

            var activityAndStatusDto = new ActivityAndStatusDto
            {
                TicketStatusDto = ticketStatusToReturnDto,
                TicketActivityLogDto = ticketActivityLogToReturnDto
            };

            return activityAndStatusDto;
        }
    }
}