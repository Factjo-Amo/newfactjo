using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Newfactjo.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        // ✅ علاقة العكسية مع الأخبار
        public virtual List<News>? News { get; set; }

        public bool ShowInTopBar { get; set; } = false;


    }
}
