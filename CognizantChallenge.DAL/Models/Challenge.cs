using System;
using System.ComponentModel.DataAnnotations;

namespace CognizantChallenge.DAL.Models
{
    public class Challenge
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        
        [Required]
        public int Input { get; set; }
        
        [Required]
        public int Output { get; set; }
    }
}