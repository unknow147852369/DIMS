﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DIMSApis.Models.Data;
using Microsoft.AspNetCore.Authorization;
using DIMSApis.Interfaces;
using DIMSApis.Models.Input;
using System.Security.Claims;

namespace DIMSApis.Controllers
{
    [Authorize(Roles = "HOST")]
    [Route("api/[controller]")]
    [ApiController]
    public class HostManageController : ControllerBase
    {
        private readonly IHostManage _host;

        public HostManageController(IHostManage host)
        {
            _host = host;
        }


        [HttpPost("Crete_Hotel")]
        public async Task<IActionResult> CreateHotel(HotelInput htInput)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int check = await _host.CreateHotel(htInput,userId);
            if (check == 1)
            {
                return Ok();
            }
            else if (check == 3)
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("Crete_Room")]
        public async Task<IActionResult> CreateRoom(NewRoomInput roomInput)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var check = await _host.CreateRoom(roomInput, userId);
            if (check.Equals(1))
            {
                return Ok();
            }
            else if (check.Equals(3))
            {
                return NoContent();
            }
            else
            {
                return BadRequest("Some rooms are existed "+check );
            }
        }

        [HttpGet("Host_All_Hotel")]
        public async Task<IActionResult> GetAllHotel()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var Hotel = await _host.GetListAllHotel(userId);
            return Ok(Hotel);
        }

        [HttpGet("Host_All_Hotel_Room")]
        public async Task<IActionResult> GetAllHotelRoom(int hotelId)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var HotelRoom = await _host.GetListAllHotelRoom(hotelId, userId);
            return Ok(HotelRoom);
        }

    }
}