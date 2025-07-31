namespace Newfactjo.ViewModels
{
    public class PermissionMatrixViewModel
    {
        public string RoleName { get; set; }

        // اسم كل صلاحية، وقيمتها (true = مفعلة، false = غير مفعلة)
        public Dictionary<string, bool> Permissions { get; set; }
    }
}
