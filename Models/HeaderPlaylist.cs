using System.ComponentModel.DataAnnotations;

namespace Newfactjo.Models
{
    public class HeaderPlaylist
    {
        public int Id { get; set; }

        [Display(Name = "رابط الوسائط")]
        public string? MediaUrl { get; set; }
        // الصورة أو الفيديو

        public string? LinkUrl { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public string? MediaType { get; set; } // "image" أو "video"
    }
}
