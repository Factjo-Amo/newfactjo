namespace Newfactjo.Models
{
    public class RolePermission
    {
        public int Id { get; set; }

        // مثال: "SuperAdmin", "Editor", "Viewer"
        public string RoleName { get; set; } = string.Empty;

        // مثال: "ManageNews", "ManageArticles", "ManageCategories", "ManageUsers"

        public string PermissionName { get; set; } = string.Empty;


        public bool IsGranted { get; set; }
    }
}
