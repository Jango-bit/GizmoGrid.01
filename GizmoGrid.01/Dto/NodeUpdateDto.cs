namespace GizmoGrid._01.Dto
{
    public class NodeUpdateDto
    {
  
        public string? Label { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public float? ImageSize { get; set; }
        public float? PositionX { get; set; }
        public float? PositionY { get; set; }

        public Guid FlowDiagramId { get; set; }
    }
}
