using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFA.Models
{
    public class UserView
    {
        private const string REGEX_Identification = @"^((?!^Name$)[-a-zA-Z0-9àâäçèêëéìîïòôöùûüÿñÀÂÄÇÈÊËÉÌÎÏÒÔÖÙÛÜ_. '])+$";
        //private const string REGEX_Date = @"^([0-2][0-9]|[3][0-1])-([01][012]|0[1-9])-([0-9][0-9][0-9][0-9])$";     

        [Required]
        public string UserName { get; set; }
        
        [Required]
        [RegularExpression(REGEX_Identification, ErrorMessage = "Contains forbidden characters.")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(REGEX_Identification, ErrorMessage = "Contains forbidden characters.")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Password must contains at least {2} characters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmation")]
        [Compare("Password", ErrorMessage = "Password confirmation doesn't match with password.")]
        public string ConfirmPassword { get; set; }

        //Validation avec System.Date.
        [Required]
        //[RegularExpression(REGEX_Date, ErrorMessage = "Invalid date format (dd-mm-yyyy)")]
        [DataType(DataType.DateTime)]
        public DateTime BirthDate { get; set; }

        //Validation avec System.Net.Mail.MailAddress(string email) qui sera une instance nulle si email est invalide
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public int Sex { get; set; }

        //[RegularExpression(REGEX_Date, ErrorMessage = "Invalid date format (dd-mm-yyyy)")]
        //[DataType(DataType.DateTime)]
        //public DateTime BirthDate { get; set; }

        public UserView()
        {
            UserName = "";
            Password = "";
            FirstName = "";
            LastName = "";

            
        }
    }

    public class LoginView
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }


    public static class SexType
    {
        public static int Null = 0;
        public static int Male = 1;
        public static int Female = 2;
        public static int Other = 3;
    }


}