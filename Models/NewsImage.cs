using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Newfactjo.Models
{
    public class NewsImage
    {
        public int Id { get; set; }

        [Required]
        public string ImagePath { get; set; } = string.Empty; // مسار الصورة المحفوظة

        // المفتاح الأجنبي الذي يربط الصورة بالخبر
        public int NewsId { get; set; }

        [ForeignKey("NewsId")]
        public News News { get; set; } = null!;
    }
}
