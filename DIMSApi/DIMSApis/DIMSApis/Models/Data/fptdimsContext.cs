using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DIMSApis.Models.Data
{
    public partial class fptdimsContext : DbContext
    {
        public fptdimsContext()
        {
        }

        public fptdimsContext(DbContextOptions<fptdimsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<BookingDetail> BookingDetails { get; set; } = null!;
        public virtual DbSet<BookingDetailMenu> BookingDetailMenus { get; set; } = null!;
        public virtual DbSet<BookingDetailPrice> BookingDetailPrices { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<District> Districts { get; set; } = null!;
        public virtual DbSet<DoorLog> DoorLogs { get; set; } = null!;
        public virtual DbSet<Facility> Facilities { get; set; } = null!;
        public virtual DbSet<Feedback> Feedbacks { get; set; } = null!;
        public virtual DbSet<Hotel> Hotels { get; set; } = null!;
        public virtual DbSet<HotelRequest> HotelRequests { get; set; } = null!;
        public virtual DbSet<HotelType> HotelTypes { get; set; } = null!;
        public virtual DbSet<InboundUser> InboundUsers { get; set; } = null!;
        public virtual DbSet<Menu> Menus { get; set; } = null!;
        public virtual DbSet<Otp> Otps { get; set; } = null!;
        public virtual DbSet<Photo> Photos { get; set; } = null!;
        public virtual DbSet<Province> Provinces { get; set; } = null!;
        public virtual DbSet<Qr> Qrs { get; set; } = null!;
        public virtual DbSet<QrCheckUp> QrCheckUps { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<SpecialPrice> SpecialPrices { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Voucher> Vouchers { get; set; } = null!;
        public virtual DbSet<Ward> Wards { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=concac");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.PaymentMethod).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasMaxLength(15);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.VoucherId).HasColumnName("VoucherID");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK_BBookings_Hotels");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_BBookings_Users");

                entity.HasOne(d => d.Voucher)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.VoucherId)
                    .HasConstraintName("FK_BBookings_Voucher");
            });

            modelBuilder.Entity<BookingDetail>(entity =>
            {
                entity.Property(e => e.BookingDetailId).HasColumnName("BookingDetailID");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.BookingDetails)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK_BookingDetails_BBookings");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.BookingDetails)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_BookedRoom_Rooms");
            });

            modelBuilder.Entity<BookingDetailMenu>(entity =>
            {
                entity.ToTable("BookingDetailMenu");

                entity.Property(e => e.BookingDetailMenuId).HasColumnName("BookingDetailMenuID");

                entity.Property(e => e.BookingDetailId).HasColumnName("BookingDetailID");

                entity.Property(e => e.MenuId).HasColumnName("MenuID");

                entity.HasOne(d => d.BookingDetail)
                    .WithMany(p => p.BookingDetailMenus)
                    .HasForeignKey(d => d.BookingDetailId)
                    .HasConstraintName("FK_BookingDetailMenu_BookingDetails");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.BookingDetailMenus)
                    .HasForeignKey(d => d.MenuId)
                    .HasConstraintName("FK_BookingDetailMenu_Menus");
            });

            modelBuilder.Entity<BookingDetailPrice>(entity =>
            {
                entity.ToTable("BookingDetailPrice");

                entity.Property(e => e.BookingDetailPriceId).HasColumnName("BookingDetailPriceID");

                entity.Property(e => e.BookingDetailId).HasColumnName("BookingDetailID");

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.HasOne(d => d.BookingDetail)
                    .WithMany(p => p.BookingDetailPrices)
                    .HasForeignKey(d => d.BookingDetailId)
                    .HasConstraintName("FK_BookingDetailPrice_BookingDetails");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(100);

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK_Categories_Hotels");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("id");

                entity.Property(e => e.DistrictNoMark)
                    .IsUnicode(false)
                    .HasColumnName("districtNoMark");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.ProvinceId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("province_id");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.ProvinceId)
                    .HasConstraintName("FK_level2s_level1s");
            });

            modelBuilder.Entity<DoorLog>(entity =>
            {
                entity.ToTable("DoorLog");

                entity.Property(e => e.DoorLogId).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.DoorCondition)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.DoorQrContent)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RoomlId).HasColumnName("RoomlID");

                entity.HasOne(d => d.Rooml)
                    .WithMany(p => p.DoorLogs)
                    .HasForeignKey(d => d.RoomlId)
                    .HasConstraintName("FK_DoorLog_Rooms");
            });

            modelBuilder.Entity<Facility>(entity =>
            {
                entity.Property(e => e.FacilityId)
                    .ValueGeneratedNever()
                    .HasColumnName("FacilityID");

                entity.Property(e => e.FacilityImage).IsUnicode(false);

                entity.Property(e => e.FacilityName).HasMaxLength(100);

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedback");

                entity.HasIndex(e => e.BookingId, "IX_Feedback")
                    .IsUnique();

                entity.Property(e => e.FeedbackId)
                    .ValueGeneratedNever()
                    .HasColumnName("FeedbackID");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Booking)
                    .WithOne(p => p.Feedback)
                    .HasForeignKey<Feedback>(d => d.BookingId)
                    .HasConstraintName("FK_Feedback_BBookings");
            });

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.District)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HotelNameNoMark).IsUnicode(false);

                entity.Property(e => e.HotelTypeId).HasColumnName("HotelTypeID");

                entity.Property(e => e.Province)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Ward)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.DistrictNavigation)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.District)
                    .HasConstraintName("FK_Hotels_Districts1");

                entity.HasOne(d => d.HotelType)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.HotelTypeId)
                    .HasConstraintName("FK_Hotels_HotelTypes");

                entity.HasOne(d => d.ProvinceNavigation)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.Province)
                    .HasConstraintName("FK_Hotels_Provinces1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Hotels_Users");

                entity.HasOne(d => d.WardNavigation)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.Ward)
                    .HasConstraintName("FK_Hotels_Wards1");
            });

            modelBuilder.Entity<HotelRequest>(entity =>
            {
                entity.ToTable("HotelRequest");

                entity.HasIndex(e => e.HotelId, "IX_HotelRequest")
                    .IsUnique();

                entity.Property(e => e.HotelRequestId).HasColumnName("HotelRequestID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.District)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Evidence)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.HotelTypeId).HasColumnName("HotelTypeID");

                entity.Property(e => e.PendingStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Province)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Ward)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.DistrictNavigation)
                    .WithMany(p => p.HotelRequests)
                    .HasForeignKey(d => d.District)
                    .HasConstraintName("FK_HotelRequest_Districts");

                entity.HasOne(d => d.Hotel)
                    .WithOne(p => p.HotelRequest)
                    .HasForeignKey<HotelRequest>(d => d.HotelId)
                    .HasConstraintName("FK_HotelRequest_Hotels");

                entity.HasOne(d => d.HotelType)
                    .WithMany(p => p.HotelRequests)
                    .HasForeignKey(d => d.HotelTypeId)
                    .HasConstraintName("FK_HotelRequest_HotelTypes");

                entity.HasOne(d => d.ProvinceNavigation)
                    .WithMany(p => p.HotelRequests)
                    .HasForeignKey(d => d.Province)
                    .HasConstraintName("FK_HotelRequest_Provinces");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HotelRequests)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_HotelRequest_Users");

                entity.HasOne(d => d.WardNavigation)
                    .WithMany(p => p.HotelRequests)
                    .HasForeignKey(d => d.Ward)
                    .HasConstraintName("FK_HotelRequest_Wards");
            });

            modelBuilder.Entity<HotelType>(entity =>
            {
                entity.Property(e => e.HotelTypeId).HasColumnName("HotelTypeID");

                entity.Property(e => e.HotelTypeName).HasMaxLength(50);

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<InboundUser>(entity =>
            {
                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.UserBirthday).HasColumnType("datetime");

                entity.Property(e => e.UserIdCard)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.UserSex)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Booking)
                    .WithMany(p => p.InboundUsers)
                    .HasForeignKey(d => d.BookingId)
                    .HasConstraintName("FK_InboundUsers_Bookings");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.Property(e => e.MenuId).HasColumnName("MenuID");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.MenuName).HasMaxLength(100);

                entity.Property(e => e.MenuType).HasMaxLength(100);

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.Menus)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK_Menus_Hotels");
            });

            modelBuilder.Entity<Otp>(entity =>
            {
                entity.Property(e => e.CodeOtp)
                    .HasMaxLength(20)
                    .HasColumnName("codeOtp")
                    .IsFixedLength();

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createDate");

                entity.Property(e => e.Purpose).HasMaxLength(50);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Otps)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Otps_Users");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.Property(e => e.PhotoId).HasColumnName("PhotoID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Photos_Categories");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK_Photo_Hotel");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.ProvinceNoMark).HasColumnName("provinceNoMark");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<Qr>(entity =>
            {
                entity.HasIndex(e => e.BookingDetailId, "IX_Qrs")
                    .IsUnique();

                entity.Property(e => e.QrId).HasColumnName("QrID");

                entity.Property(e => e.BookingDetailId).HasColumnName("BookingDetailID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.QrContent).IsUnicode(false);

                entity.Property(e => e.QrRandomString)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.QrUrl).IsUnicode(false);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.BookingDetail)
                    .WithOne(p => p.Qr)
                    .HasForeignKey<Qr>(d => d.BookingDetailId)
                    .HasConstraintName("FK_Qrs_BookingDetails");
            });

            modelBuilder.Entity<QrCheckUp>(entity =>
            {
                entity.HasIndex(e => e.BookingId, "IX_QrCheckUps")
                    .IsUnique();

                entity.Property(e => e.QrCheckUpId).HasColumnName("QrCheckUpID");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.CheckIn).HasColumnType("datetime");

                entity.Property(e => e.CheckOut).HasColumnType("datetime");

                entity.Property(e => e.QrCheckUpRandomString)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.QrContent).IsUnicode(false);

                entity.Property(e => e.QrUrl).IsUnicode(false);

                entity.HasOne(d => d.Booking)
                    .WithOne(p => p.QrCheckUp)
                    .HasForeignKey<QrCheckUp>(d => d.BookingId)
                    .HasConstraintName("FK_QrCheckUps_Bookings");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.RoomDescription).HasMaxLength(100);

                entity.Property(e => e.RoomName).IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Rooms_Categories");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK_Rooms_Hotels1");
            });

            modelBuilder.Entity<SpecialPrice>(entity =>
            {
                entity.ToTable("SpecialPrice");

                entity.Property(e => e.SpecialPriceId).HasColumnName("SpecialPriceID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.SpecialDate).HasColumnType("datetime");

                entity.Property(e => e.SpecialPrice1).HasColumnName("SpecialPrice");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.SpecialPrices)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SpecialPrice_Categories");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Birthday).HasColumnType("date");

                entity.Property(e => e.CccdbackUrl).HasColumnName("CCCDBackUrl");

                entity.Property(e => e.CccdfrontUrl).HasColumnName("CCCDFrontUrl");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Gender).HasMaxLength(10);

                entity.Property(e => e.PhoneNumber).HasMaxLength(15);

                entity.Property(e => e.Role).HasMaxLength(10);

                entity.Property(e => e.UserName).HasMaxLength(100);
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.ToTable("Voucher");

                entity.Property(e => e.VoucherId).HasColumnName("VoucherID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.VoucherCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VoucherName).HasMaxLength(100);

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.Vouchers)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK_Voucher_Hotels");
            });

            modelBuilder.Entity<Ward>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("id");

                entity.Property(e => e.DistrictId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("district_id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.WardNoMark)
                    .IsUnicode(false)
                    .HasColumnName("wardNoMark");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Wards)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_level3s_level2s");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
