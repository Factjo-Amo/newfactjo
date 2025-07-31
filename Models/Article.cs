using System;
using System.ComponentModel.DataAnnotations;

namespace Newfactjo.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }

        public bool IsPublished { get; set; }

        [StringLength(100)]
        public string Author { get; set; } = string.Empty;

        public int? CategoryId { get; set; }

        public string? ImageUrl { get; set; }

        // ✅ الحقول الجديدة لعرض بطاقة الكاتب
        [StringLength(200)]
        public string? AuthorImageUrl { get; set; }

        [StringLength(500)]
        public string? AuthorBio { get; set; }
    }
}
