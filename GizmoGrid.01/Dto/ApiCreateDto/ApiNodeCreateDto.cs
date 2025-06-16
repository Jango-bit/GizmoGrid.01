using System.ComponentModel.DataAnnotations;

namespace GizmoGrid._01.Dto.ApiCreateDto
{
    public class ApiNodeCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float PositionX { get; set; }
        public float PositionY { get; set; }
    }
}
