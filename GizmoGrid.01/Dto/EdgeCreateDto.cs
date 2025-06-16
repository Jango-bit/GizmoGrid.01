using System.ComponentModel.DataAnnotations;

namespace GizmoGrid._01.Dto
{
    public class EdgeCreateDto
    {
        [Required(ErrorMessage = "Id is required.")]

        public Guid SourceId { get; set; }

        [Required(ErrorMessage = "Target is required.")]
        public Guid TargetId { get; set; }
        [Required]
        public string SourceHandle { get; set; } = "default";

        [Required]
        public string TargetHandle { get; set; } = "default";

    }
}
