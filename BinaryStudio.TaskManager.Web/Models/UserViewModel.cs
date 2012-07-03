using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Web.Models
{
    public class UserViewModel
    {
        [UIHint("User")]
        [Display(Name = "User")]
        [DisplayFormat(NullDisplayText = "---Please Select---")]        
        public List<User> Users { get; set; }

        [UIHint("Employee")]
        [Display(Name = "Employee")]
        [DisplayFormat(NullDisplayText = "---Please Select---")]
        public List<Employee> Employees { get; set; }
    }
}