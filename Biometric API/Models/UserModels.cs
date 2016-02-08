using System.ComponentModel.DataAnnotations;

namespace Biometric_API.Models
{
    public class UserModels
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [Required]
        public string JMBG { get; set; }
        [Required]
        public int AccessLvl { get; set; }
    }
}