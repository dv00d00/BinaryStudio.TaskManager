using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Web.Models
{
    public class UserViewModel
    {
        [UIHint("Genre")]
        [Display(Name = "User")]
        [DisplayFormat(NullDisplayText = "---Please Select---")]        
        public User User { get; set; }

        [UIHint("Employee")]
        [Display(Name = "Employee")]
        [DisplayFormat(NullDisplayText = "---Please Select---")]
        public Employee Employee { get; set; }
    }
}