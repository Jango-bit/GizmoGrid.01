using System.ComponentModel.DataAnnotations;

namespace GizmoGrid._01.Dto
{
    public class FlowDiagramCreateDto
    {
        [Required(ErrorMessage = "Name is required.")]

        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "UserId is required.")]
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
    }
}
