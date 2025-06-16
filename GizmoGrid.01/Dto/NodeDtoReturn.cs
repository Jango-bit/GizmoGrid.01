namespace GizmoGrid._01.Dto
{
    public class NodeDtoReturn
    {
        //public Guid Id { get; set; }
        //public string Label { get; set; }
        //public string Description { get; set; }
        //public string ImageUrl { get; set; }     // Processed image URL (not IFormFile)
        //public float PositionX { get; set; }
        //public float PositionY { get; set; }


        public Guid Id { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public float? ImageSize { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
    }
}
