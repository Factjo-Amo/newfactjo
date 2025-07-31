using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

namespace Newfactjo.Filters
{
    public class AdminAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var adminUsername = context.HttpContext.Session.GetString("AdminUsername");

            if (string.IsNullOrEmpty(adminUsername))
            {
                // إذا لم يكن الأدمن مسجل دخول، يعيد توجيهه لصفحة تسجيل الدخول
                context.Result = new RedirectToActionResult("Login", "Admin", null);
            }
        }
    }
}
