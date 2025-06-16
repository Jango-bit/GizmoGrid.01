namespace GizmoGrid._01.Dto
{
    public class EdgeReturnDto
    {
        public Guid Id { get; set; }
        public Guid SourceId { get; set; }
        public Guid TargetId { get; set; }

        public string SourceHandle { get; set; } = "source";
        public string TargetHandle { get; set; } = "target";
    }
}
