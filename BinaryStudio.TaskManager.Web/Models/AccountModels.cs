using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.TaskManager.Web.Models
{
    public class LogOnModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
