using System.ComponentModel.DataAnnotations;

namespace GizmoGrid._01.Dto
{
    public class ProjectCreateDto
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
