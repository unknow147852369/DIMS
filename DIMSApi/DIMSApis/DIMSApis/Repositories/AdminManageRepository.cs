﻿using AutoMapper;
using DIMSApis.Interfaces;
using DIMSApis.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace DIMSApis.Repositories
{
    public class AdminManageRepository : IAdminManage
    {
        private readonly DIMSContext _context;
        private readonly IMapper _mapper;

        private string role1 = "HOST";

        public AdminManageRepository(DIMSContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AcpectHost(int UserId)
        {
            var user = await _context.Users
                .Where(u => u.UserId == UserId).FirstOrDefaultAsync();
            if (user != null)
            {
                user.Role = role1;
                if (await _context.SaveChangesAsync() > 0)
                    return 1;
                return 3;
            }
            return 0;
        }
    }
}