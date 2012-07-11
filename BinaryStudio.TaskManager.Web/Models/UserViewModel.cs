using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BinaryStudio.TaskManager.Logic.Domain;

namespace BinaryStudio.TaskManager.Web.Models
{
    public class UserViewModel
    {
        [UIHint("User")]
        [Display(Name = "User")]
        public List<User> Users { get; set; }
    }
}