using GizmoGrid._01.Dto;
using GizmoGrid._01.Dto.ApiCreateDto;
using GizmoGrid._01.Entity.Api_sEntity;

namespace GizmoGrid._01.Repository.ApiRepo
{
    public interface IApiRepoInterface
    {
        Task<Guid> CreateApiDiagramAsync(Guid userId, ApiDiagramCreateDto dto);
        Task<Guid> AddApiNodeAsync(Guid userId, Guid apiDiagramId, ApiNodeCreateDto dto);
        Task<ApiEdges> AddApiEdgeAsync(Guid userId, Guid apiDiagramId, ApiEdgeCreateDto dto); 
        Task<ApiTableNodes> UpdateApiNodeAsync(Guid userId, Guid apiDiagramId, ApiNodeUpdateDto dto);
        Task<bool> DeleteApiNodeAsync(Guid userId, Guid apiDiagramId, Guid apiTableNodeId);
        Task<List<ApiTableNodes>> GetApiNodesByDiagramIdAsync(Guid apiDiagramId);

        Task<List<ApiEdges>> GetApiEdgesByDiagramIdAsync(Guid apiDiagramId);

    }
}
