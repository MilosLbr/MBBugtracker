﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using DataModels;
using DTOs;
using MbBugtracker.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MbBugtracker.Controllers.Api
{
    [Route("api/users")]
    [ApiController]
    public class ApplicationUserApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public ApplicationUserApiController(UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUsers([FromQuery] string userName ="")
        {
            var rgx = new Regex(userName, RegexOptions.IgnoreCase);

            var appUsers = await _userManager.Users.ToListAsync();

            var usersDto = _mapper.Map<IEnumerable<ApplicationUserBasicInfoDto>>(appUsers);
            var matchedUsers = usersDto.Where(u => rgx.IsMatch(u.UserName) || rgx.IsMatch(u.Email));

            return Ok(matchedUsers);
        }
    }
}