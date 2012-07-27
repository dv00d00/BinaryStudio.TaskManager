using System;
using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class Priority
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Value { get; set; }
        public int Value2 { get; set; }
        public int Value3 { get; set; }

        
    }
}
