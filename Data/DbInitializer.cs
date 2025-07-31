using System.Linq;
using Newfactjo.Models;

namespace Newfactjo.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // تأكد من أن قاعدة البيانات تم إنشاؤها
            context.Database.EnsureCreated();

            // ✅ أضف مستخدم superadmin إن لم يكن موجودًا
            if (!context.Admins.Any(a => a.Username == "superadmin"))
            {
                context.Admins.Add(new Admin
                {
                    Username = "superadmin",
                    Password = "1234",
                    Role = "SuperAdmin"
                });
                context.SaveChanges();
            }

            // ✅ أضف التصنيفات إذا لم تكن موجودة
            if (!context.Categories.Any())
            {
                var categories = new Category[]
                {
                    new Category { Name = "المجتمع" },
                    new Category { Name = "سياسة" },
                    new Category { Name = "منوعات" },
                    new Category { Name = "اقتصاد" },
                    new Category { Name = "حوارات" },
                    new Category { Name = "العالم" },
                    new Category { Name = "الحقيقة TV" },
                    new Category { Name = "المزيد" }
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }
        }
    }
}
