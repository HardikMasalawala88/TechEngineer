using System.ComponentModel.DataAnnotations;

namespace TechEngineer.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}