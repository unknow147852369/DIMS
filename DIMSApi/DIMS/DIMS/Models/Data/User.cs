using System;
using System.Collections.Generic;

namespace DIMS.Models.Data
{
    public partial class User
    {
        public User()
        {
            BookedRooms = new HashSet<BookedRoom>();
            Hotels = new HashSet<Hotel>();
            Qrs = new HashSet<Qr>();
        }

        public int UserId { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AvatarUrl { get; set; }
        public string? CccdbackUrl { get; set; }
        public string? CccdfrontUrl { get; set; }
        public DateTime? Birthday { get; set; }
        public string Role { get; set; } = null!;
        public string? Description { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? UnlockKey { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<BookedRoom> BookedRooms { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
        public virtual ICollection<Qr> Qrs { get; set; }
    }
}
