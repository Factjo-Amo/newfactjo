using Newfactjo.Data;

namespace Newfactjo.Services
{
    public class PermissionService
    {
        private readonly AppDbContext _context;

        public PermissionService(AppDbContext context)
        {
            _context = context;
        }

        public bool HasPermission(string roleName, string permissionName)
        {
            return _context.RolePermissions
                .Any(p => p.RoleName == roleName && p.PermissionName == permissionName && p.IsGranted);
        }
    }
}
