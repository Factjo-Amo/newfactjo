using System.ComponentModel.DataAnnotations;

namespace Newfactjo.Models
{
    public class AdminUser
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "نوع الصلاحية مطلوب")]
        public string Role { get; set; } = string.Empty;  // ✅ ← تم تغيير النوع إلى string وإضافة القيمة الافتراضية

        // ✅ لم نعد بحاجة إلى enum UserRole هنا، لأنه سيتم أخذ الأدوار من جدول Roles
    }
}
