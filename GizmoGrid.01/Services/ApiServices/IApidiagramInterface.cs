using GizmoGrid._01.Dto;
using GizmoGrid._01.Dto.ApiCreateDto;
using GizmoGrid._01.Entity.Api_sEntity;

namespace GizmoGrid._01.Services.ApiServices
{
    public interface IApidiagramInterface
    {
        Task<Guid> CreateApiDiagramAsync(Guid userId, ApiDiagramCreateDto dto);
        Task<Guid> AddApiNodeAsync(Guid userId, Guid apiDiagramId, ApiNodeCreateDto dto);
        Task<ApiEdgeReturnDto> AddApiEdgeAsync(Guid userId, Guid apiDiagramId, ApiEdgeCreateDto dto);
        Task<ApiTableNodes> UpdateApiNodeAsync(Guid userId, Guid apiDiagramId, ApiNodeUpdateDto dto);
        Task<bool> DeleteApiNodeAsync(Guid userId, Guid apiDiagramId, Guid apiTableNodeId);
        Task<List<ApiNodeReturnDto>> GetApiNodesAsync(Guid apiDiagramId);
        Task<List<ApiEdgeReturnDto>> GetApiEdgesByDiagramIdAsync(Guid apiDiagramId);
    }
}
