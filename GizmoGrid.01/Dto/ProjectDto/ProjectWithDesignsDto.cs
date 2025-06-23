namespace GizmoGrid._01.Dto.ProjectDto
{
    public class ProjectWithDesignsDto
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<FlowDiagramDto> FlowDiagrams { get; set; }
        public List<ApiDiagramDto> ApiDiagrams { get; set; }
        public List<SchemaDiagramDto> SchemaDiagrams { get; set; }
    }
}
