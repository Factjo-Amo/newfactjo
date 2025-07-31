using System.ComponentModel.DataAnnotations;

namespace Newfactjo.Models
{
    public class Admin
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "الدور مطلوب")]
        public string Role { get; set; } // في AdminUser

    }
}
