using GizmoGrid._01.Dto;
using GizmoGrid._01.Entity;

namespace GizmoGrid._01.Services.FlowDiagramService
{
    public interface IFlowDiagramService
    {
        Task<Guid> CreateFlowDiagramAsync(Guid userId, FlowDiagramCreateDto dto);
        Task<FlowDiagramDtoReturn> GetFlowDiagramAsync(Guid userId, Guid flowDiagramId);
        Task<Guid> AddNodeAsync(Guid userId, Guid flowDiagramId, NodeCreateDto dto);
        Task UpdateNodeAsync(Guid userId, Guid nodeId, NodeUpdateDto dto);
        Task DeleteNodeAsync(Guid userId, Guid nodeId);
        Task<Edge> AddEdgeAsync(Guid userId, Guid flowDiagramId, EdgeCreateDto dto);
        Task<List<EdgeReturnDto>> GetEdgesDtoByDiagramIdAsync(Guid userId, Guid diagramId);
        Task<NodeDtoReturn> GetNodeByIdAsync(Guid userId, Guid nodeId);
    }
}
