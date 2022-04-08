using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DIMS.Models.Data
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

        public virtual DbSet<BookedRoom> BookedRooms { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<District> Districts { get; set; } = null!;
        public virtual DbSet<Hotel> Hotels { get; set; } = null!;
        public virtual DbSet<Photo> Photos { get; set; } = null!;
        public virtual DbSet<Province> Provinces { get; set; } = null!;
        public virtual DbSet<Qr> Qrs { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
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
            modelBuilder.Entity<BookedRoom>(entity =>
            {
                entity.HasKey(e => e.BookedId);

                entity.Property(e => e.BookedId)
                    .ValueGeneratedNever()
                    .HasColumnName("BookedID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.BookedRooms)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_BookedRooms_Categories");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.BookedRooms)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK_BookedRooms_Hotels");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.BookedRooms)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_BookedRooms_Room");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.BookedRooms)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_BookedRooms_Users1");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId)
                    .ValueGeneratedNever()
                    .HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.HotelId).HasColumnName("HotelID");

                entity.Property(e => e.Status)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK_Categories_Hotels");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("id")
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.ProvinceId)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("province_id")
                    .IsFixedLength();

                entity.Property(e => e.Type)
                    .HasMaxLength(45)
                    .HasColumnName("type");

                entity.HasOne(d => d.Province)
                    .WithMany(p => p.Districts)
                    .HasForeignKey(d => d.ProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_district_province");
            });

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.Property(e => e.HotelId)
                    .ValueGeneratedNever()
                    .HasColumnName("HotelID");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.District)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Province)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Ward)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.DistrictNavigation)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.District)
                    .HasConstraintName("FK_Hotel_district");

                entity.HasOne(d => d.ProvinceNavigation)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.Province)
                    .HasConstraintName("FK_Hotel_province");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Hotels_Users");

                entity.HasOne(d => d.WardNavigation)
                    .WithMany(p => p.Hotels)
                    .HasForeignKey(d => d.Ward)
                    .HasConstraintName("FK_Hotel_ward");
            });

            modelBuilder.Entity<Photo>(entity =>
            {
                entity.Property(e => e.PhotoId)
                    .ValueGeneratedNever()
                    .HasColumnName("PhotoID");

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
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("id")
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.Type)
                    .HasMaxLength(45)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<Qr>(entity =>
            {
                entity.Property(e => e.QrId)
                    .ValueGeneratedNever()
                    .HasColumnName("QrID");

                entity.Property(e => e.ActiveDate).HasColumnType("datetime");

                entity.Property(e => e.BookedId).HasColumnName("BookedID");

                entity.Property(e => e.ExpireDate).HasColumnType("datetime");

                entity.Property(e => e.QrString)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Booked)
                    .WithMany(p => p.Qrs)
                    .HasForeignKey(d => d.BookedId)
                    .HasConstraintName("FK_Qrs_BookedRooms");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Qrs)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Qrs_Users");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.RoomId)
                    .ValueGeneratedNever()
                    .HasColumnName("RoomID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Room_Categories");
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
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("id")
                    .IsFixedLength();

                entity.Property(e => e.DistrictId)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("district_id")
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.Type)
                    .HasMaxLength(45)
                    .HasColumnName("type");

                entity.HasOne(d => d.District)
                    .WithMany(p => p.Wards)
                    .HasForeignKey(d => d.DistrictId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ward_district");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
