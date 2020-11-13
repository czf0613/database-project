using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

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
        public string UserName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 8)]
        public string Password { get; set; }

        public string IconURL { get; set; } = "https://pic-bed.xyz/res/icons/default.png";

        public Gender Gender { get; set; } = Gender.SECRET;

        [Required]
        public string Name { get; set; }

        public string Profile { get; set; }

        [Required]
        [StringLength(11)]
        [RegularExpression(@"^1[0-9]{10}$")]
        public string Phone { get; set; }

        [RegularExpression(@"^[A-Za-z0-9\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$")]
        public string Email { get; set; }

        [StringLength(18)]
        [RegularExpression(@"^[1-9]\d{5}(18|19|([23]\d))\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\d{3}[0-9Xx]$")]
        public string IDNumber { get; set; }

        public DateTimeOffset Birthday { get; set; }

        public DateTimeOffset RegistrationTime { get; set; } = DateTimeOffset.Now;

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
    }

    public class Student: User
    {
        [Required]
        [RegularExpression(@"^20[0-9]{10}$")]
        public string SerialNumber { get; set; }

        [StringLength(40)]
        public string College { get; set; }

        [StringLength(40)]
        public string Major { get; set; }

        public string Dormitory { get; set; }
    }

    public class Admin: User
    {
        public string Department { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        public int Level { get; set; }
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

        private List<string> content = new List<string>();
        public string Photos 
        {
            get
            {
                return JsonSerializer.Serialize(content);
            }
            set
            {
                content = JsonSerializer.Deserialize<List<string>>(value);
            }
        }

        public DateTimeOffset ReleaseTime { get; set; } = DateTimeOffset.Now;

        [Required]
        public decimal Price { get; set; } = 0.0M;

        [Required]
        public Student Seller { get; set; }

        public bool Sold { get; set; } = false;
    }

    public class SalesRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CommodityId { get; set; }

        [Required]
        public Student Buyer { get; set; }

        public DateTimeOffset TransactionTime { get; set; } = DateTimeOffset.Now;

        [Required]
        public decimal Auction { get; set; } = 0.0M;
    }
}
