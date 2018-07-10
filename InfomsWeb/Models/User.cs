using InfomsWeb.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InfomsWeb.Models
{
    public class User
    {
        public int ID { get; set; }

        [Required]
        [Display(Name ="Staff ID")]
        public string StaffID { get; set; }

        [Required]
        [Display(Name ="Full Name")]
        public string FullName { get; set; }

        [Display(Name = "E-Mail")]
        [EmailAddress(ErrorMessage ="Not a valid E-Mail address")]
        public string Email { get; set; }

        [Display(Name = "Phone No.")]
        [Phone(ErrorMessage = "Not a valid phone number")]
        public string ContactNo { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        public string City { get; set; }

        [Display(Name = "Postal Code")]
        [RegularExpression(@"^(\d{5})?$", ErrorMessage = "Please Enter Valid Postal Code.")]
        public string Postcode { get; set; }

        public string State { get; set; }

        [Required(ErrorMessage = "Login Name is required.", AllowEmptyStrings = false)]
        public string LoginName { get; set; }

        [Required(ErrorMessage = "Password is required.", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }

        public static List<User> GetAllUsers()
        {
            UserDataContext db = new UserDataContext();
            List<User> usr = db.GetAllUser();
            return usr;
        }

        public int Create()
        {
            UserDataContext db = new UserDataContext();
            return db.CreateUser(this);
        }
    }
}