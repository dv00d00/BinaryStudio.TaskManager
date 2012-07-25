// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserModels.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the RegisterUserModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BinaryStudio.TaskManager.Web.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    /// <summary>
    /// The register user model.
    /// </summary>
    public class RegisterUserModel
    {
        public int userId { get; set; }

        [Required(ErrorMessage = "Username is required!")]
        [Display(Name = "User name")]
        [MaxLength(50, ErrorMessage = "Username maximum length is 50 symbols!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [Display(Name = "Email")]        
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password is required!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "{0} maximum length is 100 symbols and minimum length is {2} symbol.", MinimumLength = 1)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required!")]
        [Display(Name = "Confirm password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The passwords do not match!")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// The change password model.
    /// </summary>
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} должени иметь минимальную длину {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повторный ввод пароля")]
        [Compare("Новый пароль", ErrorMessage = "Значения не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}