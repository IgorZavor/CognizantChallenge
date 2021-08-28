using System;
using System.ComponentModel.DataAnnotations;

namespace CognizantChallenge.DAL.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        public int Scores { get; set; }
        public string Tasks { get; set; }
    }
}