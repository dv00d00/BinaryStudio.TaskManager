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
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the image data.
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// Gets or sets the image mime tipe.
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }

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
}