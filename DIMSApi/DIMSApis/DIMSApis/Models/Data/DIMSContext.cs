using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DIMSApis.Models.Data
{
    public partial class DIMSContext : DbContext
    {
        public DIMSContext()
        {
        }

        public DIMSContext(DbContextOptions<DIMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; } = null!;
        public virtual DbSet<BookingDetail> BookingDetails { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<District> Districts { get; set; } = null!;
        public virtual DbSet<Hotel> Hotels { get; set; } = null!;
        public virtual DbSet<Photo> Photos { get; set; } = null!;
        public virtual DbSet<Province> Provinces { get; set; } = null!;
        public virtual DbSet<Qr> Qrs { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Ward> Wards { get; set; } = null!;
        public virtual DbSet<staff> staff { get; set; } = null!;

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

                entity.Property(e => e.Condition).HasMaxLength(50);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.PhoneNumber).HasMaxLength(15);

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK_BookedRooms_Hotels");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_BookedRooms_Users1");
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
                    .HasConstraintName("FK_BookingDetails_Bookings");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.BookingDetails)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_BookedRoom_Rooms");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(100);
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("id");

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

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.Property(e => e.HotelId)
                    .ValueGeneratedNever()
                    .HasColumnName("HotelID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.District)
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
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.District)
                    .HasConstraintName("FK_Hotels_Districts");

                entity.HasOne(d => d.District1)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.District)
                    .HasConstraintName("FK_Hotels_Provinces");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Hotels_Users");

                entity.HasOne(d => d.WardNavigation)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.Ward)
                    .HasConstraintName("FK_Hotels_Wards");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.Property(e => e.PhotoId).HasColumnName("PhotoID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK_Photo_Hotel");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Photos)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_Photos_Rooms");
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Type).HasColumnName("type");
            });

            modelBuilder.Entity<Qr>(entity =>
            {
                entity.HasIndex(e => e.BookingDetailId, "IX_Qrs")
                    .IsUnique();

                entity.Property(e => e.QrId).HasColumnName("QrID");

                entity.Property(e => e.BookingDetailId).HasColumnName("BookingDetailID");

                entity.Property(e => e.CheckIn).HasColumnType("datetime");

                entity.Property(e => e.CheckOut).HasColumnType("datetime");

                entity.HasOne(d => d.BookingDetail)
                    .WithOne(p => p.Qr)
                    .HasForeignKey<Qr>(d => d.BookingDetailId)
                    .HasConstraintName("FK_Qrs_BookingDetails");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.RoomName).IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Rooms_Categories");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK_Rooms_Hotels");
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

                entity.Property(e => e.UnlockKey).HasMaxLength(10);

                entity.Property(e => e.UserName).HasMaxLength(100);
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

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Wards)
                    .HasForeignKey(d => d.DistrictId)
                    .HasConstraintName("FK_level3s_level2s");
            });

            modelBuilder.Entity<staff>(entity =>
            {
                entity.ToTable("Staff");

                entity.Property(e => e.Id)
                    .HasMaxLength(10)
                    .HasColumnName("ID")
                    .IsFixedLength();

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.Job).HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.staff)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK_Staff_Hotels");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.staff)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Staff_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
