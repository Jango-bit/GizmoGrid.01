using GizmoGrid._01.Data;
using GizmoGrid._01.Dto;
using GizmoGrid._01.Dto.ApiCreateDto;
using GizmoGrid._01.Entity;
using GizmoGrid._01.Entity.Api_sEntity;
using Microsoft.EntityFrameworkCore;

namespace GizmoGrid._01.Repository.ApiRepo
{
    public class ApiRepoService : IApiRepoInterface
    {
        private readonly CodePlannerDbContext _codePlannerDbContext;
        public ApiRepoService(CodePlannerDbContext codePlannerDbContext)
        {
            _codePlannerDbContext = codePlannerDbContext;

        }

        public async Task<Guid> CreateApiDiagramAsync(Guid userId, ApiDiagramCreateDto dto)
        {
            try
            {
                var project = await _codePlannerDbContext.Projects
               .FirstOrDefaultAsync(p => p.Id == dto.ProjectId && p.UserId == userId);
                if (project == null)
                {
                    throw new KeyNotFoundException("Project not found.");

                }
                var ApiDiagram = new ApiDiagram
                {
                    ApiDiagramId = Guid.NewGuid(),
                    ProjectId = dto.ProjectId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _codePlannerDbContext.ApiDiagrams.Add(ApiDiagram);
                await _codePlannerDbContext.SaveChangesAsync();
                return ApiDiagram.ApiDiagramId;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error creating flow diagram.", ex);
            }

        }
        public async Task<Guid> AddApiNodeAsync(Guid userId, Guid apiDiagramId, ApiNodeCreateDto dto)
        {
            var apiDiagram = await _codePlannerDbContext.ApiDiagrams
                .FirstOrDefaultAsync(d => d.ApiDiagramId == apiDiagramId && d.UserId == userId);

            if (apiDiagram == null)
                throw new KeyNotFoundException("API diagram not found or access denied.");

            var newNode = new ApiTableNodes
            {
                ApiTableNodesId = Guid.NewGuid(),
                ApiDiagramId = apiDiagramId,
                Name = dto.Name,
                Description = dto.Description,
                PositionX = dto.PositionX,
                PositionY = dto.PositionY
            };

            _codePlannerDbContext.ApiTableNodes.Add(newNode);
            await _codePlannerDbContext.SaveChangesAsync();

            return newNode.ApiTableNodesId;
        }
        public async Task<ApiEdges> AddApiEdgeAsync(Guid userId, Guid apiDiagramId, ApiEdgeCreateDto dto)
        {
            var diagram = await _codePlannerDbContext.ApiDiagrams
                .FirstOrDefaultAsync(fd => fd.ApiDiagramId == apiDiagramId && fd.UserId == userId);

            if (diagram == null)
                throw new KeyNotFoundException("Diagram not found or access denied.");

            var edge = new ApiEdges
            {
                ApiEdgesId = Guid.NewGuid(),
                ApiDiagramId = apiDiagramId,
                SourceId = dto.SourceId,
                TargetId = dto.TargetId
            };

            _codePlannerDbContext.ApiEdges.Add(edge);
            await _codePlannerDbContext.SaveChangesAsync();

            return edge;
        }


        //public async Task<ApiTableNodes> UpdateApiNodeAsync(Guid userId, Guid apiDiagramId, ApiNodeUpdateDto dto)
        //{
        //    var apiDiagram = await _codePlannerDbContext.ApiDiagrams
        //        .Include(d => d.ApiTableNodes)
        //        .FirstOrDefaultAsync(d => d.ApiDiagramId == apiDiagramId && d.UserId == userId);

        //    if (apiDiagram == null)
        //        throw new KeyNotFoundException("API diagram not found or access denied.");

        //    var node = apiDiagram.ApiTableNodes.FirstOrDefault(n => n.ApiTableNodesId == dto.ApiTableNodesId);

        //    if (node == null)
        //        throw new KeyNotFoundException("Node not found.");

        //    node.Name = dto.Name;
        //    node.Description = dto.Description;
        //    node.PositionX = dto.PositionX;
        //    node.PositionY = dto.PositionY;

        //    await _codePlannerDbContext.SaveChangesAsync();

        //    return node;
        //}
        public async Task<ApiNodeReturnDto> UpdateApiNodeAsync(Guid userId, Guid apiDiagramId, ApiNodeUpdateDto dto)
        {
            var apiDiagram = await _codePlannerDbContext.ApiDiagrams
                .Include(d => d.ApiTableNodes)
                .FirstOrDefaultAsync(d => d.ApiDiagramId == apiDiagramId && d.UserId == userId);

            if (apiDiagram == null)
                throw new KeyNotFoundException("API diagram not found or access denied.");

            var node = apiDiagram.ApiTableNodes.FirstOrDefault(n => n.ApiTableNodesId == dto.ApiTableNodesId);
            if (node == null)
                throw new KeyNotFoundException("Node not found.");

            node.Name = dto.Name;
            node.Description = dto.Description;
            node.PositionX = dto.PositionX;
            node.PositionY = dto.PositionY;

            await _codePlannerDbContext.SaveChangesAsync();

            return new ApiNodeReturnDto
            {
                ApiTableNodesId = node.ApiTableNodesId,
                Name = node.Name,
                Description = node.Description,
                PositionX = node.PositionX,
                PositionY = node.PositionY
            };
        }
        public async Task<bool> DeleteApiNodeAsync(Guid userId, Guid apiDiagramId, Guid apiTableNodeId)
        {
            var diagram = await _codePlannerDbContext.ApiDiagrams
                .Include(d => d.ApiEdges)
                .Include(d => d.ApiTableNodes)
                .FirstOrDefaultAsync(d => d.ApiDiagramId == apiDiagramId && d.UserId == userId);

            if (diagram == null)
                throw new KeyNotFoundException("API diagram not found or access denied.");

            var relatedEdges = diagram.ApiEdges
                .Where(e => e.SourceId == apiTableNodeId || e.TargetId == apiTableNodeId)
                .ToList();

            _codePlannerDbContext.ApiEdges.RemoveRange(relatedEdges);

            var node = diagram.ApiTableNodes.FirstOrDefault(n => n.ApiTableNodesId == apiTableNodeId);
            if (node == null)
                throw new KeyNotFoundException("API node not found.");

            _codePlannerDbContext.ApiTableNodes.Remove(node);

            await _codePlannerDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<ApiTableNodes>> GetApiNodesByDiagramIdAsync(Guid apiDiagramId)
        {
            return await _codePlannerDbContext.ApiTableNodes
                .Where(n => n.ApiDiagramId == apiDiagramId)
                .ToListAsync();
        }
        public async Task<List<ApiEdges>> GetApiEdgesByDiagramIdAsync(Guid apiDiagramId)
        {
            return await _codePlannerDbContext.ApiEdges
                .Where(e => e.ApiDiagramId == apiDiagramId)
                .ToListAsync();
        }
    }
}






