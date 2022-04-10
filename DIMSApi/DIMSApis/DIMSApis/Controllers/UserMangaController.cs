#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DIMSApis.Models.Data;
using Microsoft.AspNetCore.Authorization;

namespace DIMSApis.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserMangaController : ControllerBase
    {
        private readonly DIMSContext _context;

        public UserMangaController(DIMSContext context)
        {
            _context = context;
        }



    }
}
