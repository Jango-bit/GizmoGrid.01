namespace GizmoGrid._01.Entity.Projectaccces
{
    public class ProjectAccess
    {


        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }

        public Project Project { get; set; }
        public User User { get; set; }
    }
}
