using FluentValidation;
using IC_ebilet.pl.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IC_ebilet.pl.ViewModel
{
    [FluentValidation.Attributes.Validator(typeof(UserValidation))]
    public class UserViewModel
    { 
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public FavouriteViewModel Fav { get; set; }
        public List<int> BannedEventID { get; set; }

    }
    public class UserValidation : AbstractValidator<UserViewModel>
    {
        public UserValidation()
        {
            RuleFor(x => x.Email).NotNull().Length(2, 50).EmailAddress();
            RuleFor(x => x.Password).NotNull();
        }
    }
}