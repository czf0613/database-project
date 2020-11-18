﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SecondHand.model
{
    public class Databases: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<SalesRecord> SalesRecords { get; set; }

        public Databases(DbContextOptions<Databases> Options): base(Options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Console.WriteLine("Database Creating Or Updating...");
            modelBuilder.Entity<Commodity>()
                .Property(e => e.Photos)
                .HasConversion(
                v => (v == null || v.Count == 0) ? "[]" : JsonSerializer.Serialize(v, null),
                v => (v == null || v.Length == 0) ? new List<string>() : JsonSerializer.Deserialize<List<string>>(v, null)
                );

            modelBuilder.Entity<SalesRecord>()
                .HasOne(e => e.Seller)
                .WithMany(s => s.Sold);

            modelBuilder.Entity<SalesRecord>()
                .HasOne(e => e.Buyer)
                .WithMany(s => s.Bought);

            modelBuilder.Entity<User>()
                .Property(e => e.Gender)
                .HasConversion(
                v => v.ToString(),
                v => (Gender)Enum.Parse(typeof(Gender), v)
                );

            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.UserName, u.Phone, u.SerialNumber })
                .IsUnique();

            modelBuilder.Entity<Commodity>()
                .Property(e => e.Price)
                .HasPrecision(10, 4);

            modelBuilder.Entity<SalesRecord>()
                .Property(e => e.Auction)
                .HasPrecision(10, 4);

            Console.WriteLine("Database Migration Success");
        }
    }

    public enum Gender
    {
        MALE, FEMALE, UNKNOWN, SECRET
    }

    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string UserName { get; set; } = "default";

        [Required]
        [StringLength(50, MinimumLength = 8)]
        [JsonIgnore]
        public string Password { get; set; } = "12345678";

        [Required]
        public string IconURL { get; set; } = "https://pic-bed.xyz/res/icons/default.png";

        [Required]
        public Gender Gender { get; set; } = Gender.SECRET;

        [Required]
        public string Name { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        [Required]
        public string Profile { get; set; } = "这个人很懒，什么简介都没有写";

        [Required]
        [StringLength(11)]
        [RegularExpression(@"^1[0-9]{10}$")]
        public string Phone { get; set; }

        [RegularExpression(@"^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$")]
        public string Email { get; set; } = "";

        [StringLength(18)]
        [RegularExpression(@"^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$")]
        [JsonIgnore]
        public string IDNumber { get; set; } = "";

        [Required]
        public DateTimeOffset Birthday { get; set; }

        [Required]
        [JsonIgnore]
        public DateTimeOffset RegistrationTime { get; set; } = DateTimeOffset.Now;

        [NotMapped]
        public int Age
        {
            get
            {
                var now = DateTimeOffset.Now;
                var gap = now.Year - Birthday.Year;
                if (now.Month > Birthday.Month || (now.Month == Birthday.Month && now.Day >= Birthday.Day))
                    gap -= 0;
                else
                    gap -= 1;
                return gap;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is User))
                return false;
            var user = (User)obj;
            return user.UserName == this.UserName;
        }

        public override int GetHashCode()
        {
            return (this.Id + this.UserName).GetHashCode();
        }
    }

    public class Student: User
    {
        [StringLength(40)]
        public string College { get; set; }

        [StringLength(40)]
        public string Major { get; set; }

        [Required]
        public string Dormitory { get; set; } = "";

        [Required]
        public List<Commodity> AllMyCommodities { get; set; } = new List<Commodity>();

        [Required]
        public List<SalesRecord> Sold { get; set; } = new List<SalesRecord>();

        [Required]
        public List<SalesRecord> Bought { get; set; } = new List<SalesRecord>();

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return (this.College + this.Major + this.Dormitory).GetHashCode();
        }
    }

    public class Admin: User
    {
        public string Department { get; set; } = "NetWork Center";

        public int Level { get; set; } = 1;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class Commodity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public List<string> Photos { get; set; } = new List<string>();

        [Required]
        public DateTimeOffset ReleaseTime { get; set; } = DateTimeOffset.Now;

        [Required]
        public decimal Price { get; set; } = 0.0M;

        [Required]
        public Student Seller { get; set; }

        [ConcurrencyCheck]
        public bool Sold { get; set; } = false;
    }

    public class SalesRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public Commodity Commodity { get; set; }

        [Required]
        public Student Seller { get; set; }

        [Required]
        public Student Buyer { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        [Required]
        public DateTimeOffset TransactionTime { get; set; } = DateTimeOffset.Now;

        [Required]
        public decimal Auction { get; set; } = 0.0M;

        [ConcurrencyCheck]
        public bool Check { get; set; } = false;
    }
}
