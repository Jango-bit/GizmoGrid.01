using GizmoGrid._01.Entity.Projectaccces;

namespace GizmoGrid._01.Entity
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public List<Project> Projects { get; set; } = new List<Project>();
        public ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
        public ICollection<ProjectAccess> ProjectAccesses { get; set; } = new List<ProjectAccess>();

    }

}

