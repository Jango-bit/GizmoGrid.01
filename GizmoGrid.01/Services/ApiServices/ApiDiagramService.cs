using GizmoGrid._01.Dto.ApiCreateDto;
using GizmoGrid._01.Entity.Api_sEntity;
using GizmoGrid._01.Repository.ApiRepo;

namespace GizmoGrid._01.Services.ApiServices
{
    public class ApiDiagramService : IApidiagramInterface
    {
        private readonly IApiRepoInterface _repoInterface;
        public ApiDiagramService(IApiRepoInterface repoInterface)
        {
            _repoInterface = repoInterface;
        }
        public async Task<Guid> CreateApiDiagramAsync(Guid userId, ApiDiagramCreateDto dto)
        {
            return await _repoInterface.CreateApiDiagramAsync(userId, dto);
        }
        public async Task<Guid> AddApiNodeAsync(Guid userId, Guid apiDiagramId, ApiNodeCreateDto dto)
        {
            return await _repoInterface.AddApiNodeAsync(userId, apiDiagramId, dto);
        }

        public async Task<Guid> AddApiEdgeAsync(Guid userId, Guid apiDiagramId, ApiEdgeCreateDto dto)
        {
            return await _repoInterface.AddApiEdgeAsync(userId, apiDiagramId, dto);
        }
        public async Task<ApiTableNodes> UpdateApiNodeAsync(Guid userId, Guid apiDiagramId, ApiNodeUpdateDto dto)
        {
            return await _repoInterface.UpdateApiNodeAsync(userId, apiDiagramId, dto);
        }
        public async Task<bool> DeleteApiNodeAsync(Guid userId, Guid apiDiagramId, Guid apiTableNodeId)
        {
            return await _repoInterface.DeleteApiNodeAsync(userId, apiDiagramId, apiTableNodeId);
        }
        public async Task<List<ApiNodeReturnDto>> GetApiNodesAsync(Guid apiDiagramId)
        {
            var nodes = await _repoInterface.GetApiNodesByDiagramIdAsync(apiDiagramId);

            return nodes.Select(n => new ApiNodeReturnDto
            {
                ApiTableNodesId = n.ApiTableNodesId,
                Name = n.Name,
                Description = n.Description,
                PositionX = n.PositionX,
                PositionY = n.PositionY
            }).ToList();
        }
    }
}