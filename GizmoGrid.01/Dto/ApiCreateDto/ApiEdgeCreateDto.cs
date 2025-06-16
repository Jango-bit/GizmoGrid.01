using System.ComponentModel.DataAnnotations;

namespace GizmoGrid._01.Dto.ApiCreateDto
{
    public class ApiEdgeCreateDto
    {
        [Required(ErrorMessage = "SourceId is required.")]

        public Guid SourceId { get; set; }

        [Required(ErrorMessage = "Target is required.")]
        public Guid TargetId { get; set; }
    }
}
