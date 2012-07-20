﻿// --------------------------------------------------------------------------------------------------------------------
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

        [Required(ErrorMessage = "Имя пользователя обязательно к заполнению!")]
        [Display(Name = "Имя пользователя")]
        [MaxLength(50, ErrorMessage = "Имя пользователя не может содержать более 50 символов!")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email обязателен к заполнению!")]
        [Display(Name = "Email:")]        
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage = "Пароль обязателен к заполнению!")]
        [Display(Name = "Пароль:")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Пароль должен иметь минимальную длину 6 символов.", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтверждение пароля обязательно к заполнению!")]
        [Display(Name = "Подтверждение пароля:")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Введённые пароли не совпадают!")]
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