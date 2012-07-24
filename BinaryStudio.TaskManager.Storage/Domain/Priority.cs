﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BinaryStudio.TaskManager.Logic.Domain
{
    public class Priority
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Descriptio { get; set; }

        [Required]
        public int Value { get; set; }
    }
}