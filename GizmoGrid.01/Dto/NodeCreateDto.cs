using System.ComponentModel.DataAnnotations;

namespace GizmoGrid._01.Dto
{
    public class NodeCreateDto
    {


        [Required(ErrorMessage = "Label is required.")]
        public string? Label { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public float? ImageSize { get; set; }
        public double? PositionX { get; set; }
        public double? PositionY { get; set; }

    }
}
