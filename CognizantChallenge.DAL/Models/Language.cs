using System.ComponentModel.DataAnnotations;

namespace CognizantChallenge.DAL.Models
{
    public class Language
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        public string Template { get; set; }

        [Required]
        public string RequestedName { get; set; }
    }
}