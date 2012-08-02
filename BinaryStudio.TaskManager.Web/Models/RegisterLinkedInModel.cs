// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegisterLinkedInModel.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the RegisterLinkedInModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace ThirdPartySignup.Models
{
    using System.Web.Mvc;

    public class RegisterLinkedInModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please enter a valid email")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string LinkedInId { get; set; }

        /// <summary>
        /// Gets or sets the image data.
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// Gets or sets the image mime tipe.
        /// </summary>
        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }
    }
}