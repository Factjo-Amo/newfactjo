using System.ComponentModel.DataAnnotations;

namespace Newfactjo.Models
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
