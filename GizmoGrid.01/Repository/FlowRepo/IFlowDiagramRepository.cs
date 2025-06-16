using GizmoGrid._01.Dto;
using GizmoGrid._01.Entity;

namespace GizmoGrid._01.Repository.FlowRepo
{
    public interface IFlowDiagramRepository
    {
        Task<Guid> CreateFlowDiagramAsync(Guid userId, FlowDiagramCreateDto dto);
        Task<FlowDiagramDtoReturn> GetFlowDiagramAsync(Guid userId, Guid flowDiagramId);
        Task<Guid> AddNodeAsync(Guid userId, Guid flowDiagramId, NodeCreateDto dto, string imageUrl);
        Task<NodeDtoReturn> GetNodeByIdAsync(Guid userId, Guid nodeId);
        Task UpdateNodeAsync(Guid userId, Guid nodeId, NodeUpdateDto dto, string imageUrl);
        Task DeleteNodeAsync(Guid userId, Guid nodeId);
        Task<Edge> AddEdgeAsync(Guid userId, Guid flowDiagramId, EdgeCreateDto dto);

        Task<List<Edge>> GetEdgesByDiagramIdAsync(Guid userId, Guid diagramId);

    }
}
