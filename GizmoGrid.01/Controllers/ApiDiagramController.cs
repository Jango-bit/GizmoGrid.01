using System.Security.Claims;
using GizmoGrid._01.Data;
using GizmoGrid._01.Dto;
using GizmoGrid._01.Dto.ApiCreateDto;
using GizmoGrid._01.Entity;
using GizmoGrid._01.Services.ApiServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GizmoGrid._01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiDiagramController : ControllerBase
    {
        private readonly IApidiagramInterface _aidiagramInterface;
        private readonly CodePlannerDbContext _codePlannerDbContext;    
        public ApiDiagramController(IApidiagramInterface aidiagramInterface , CodePlannerDbContext codePlannerDbContext)
        {
            _aidiagramInterface = aidiagramInterface;
            _codePlannerDbContext = codePlannerDbContext;
        }
        [HttpPost("CREATE APIdIAGRAM")]

        public async Task<IActionResult> CreateApiDiagram([FromBody] ApiDiagramCreateDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var ApiDiagramId = await _aidiagramInterface.CreateApiDiagramAsync(userId, dto);
                return Ok(new { Id = ApiDiagramId });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{apiDiagramId}/nodes")]
        public async Task<IActionResult> CreateApiNode(Guid apiDiagramId, [FromBody] ApiNodeCreateDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                var newNodeId = await _aidiagramInterface.AddApiNodeAsync(userId, apiDiagramId, dto);
                return Ok(new { Id = newNodeId });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating node: {ex.Message}");
            }
        }
        [HttpPost("AddEdges")]
        public async Task<IActionResult> AddApiEdgeAsync(Guid apiDiagramId, [FromBody] ApiEdgeCreateDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

                var result = await _aidiagramInterface.AddApiEdgeAsync(userId, apiDiagramId, dto);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating edge: {ex.Message}");
            }
        }
        [HttpPut("{apiDiagramId}/nodes")]
        public async Task<IActionResult> UpdateApiNode(Guid apiDiagramId, [FromBody] ApiNodeUpdateDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                var updatedNode = await _aidiagramInterface.UpdateApiNodeAsync(userId, apiDiagramId, dto);
                return Ok(updatedNode);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating node: {ex.Message}");
            }
        }
        [HttpDelete("{apiDiagramId}/nodes/{apiTableNodeId}")]
        public async Task<IActionResult> DeleteApiNode(Guid apiDiagramId, Guid apiTableNodeId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                var result = await _aidiagramInterface.DeleteApiNodeAsync(userId, apiDiagramId, apiTableNodeId);

                if (result)
                    return Ok(new { Message = "Node deleted successfully." });

                return BadRequest("Node could not be deleted.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting node: {ex.Message}");
            }
        }

        [HttpGet("{apiDiagramId}/apinodes")]
        public async Task<IActionResult> GetApiNodes(Guid apiDiagramId)
        {
            var nodes = await _aidiagramInterface.GetApiNodesAsync(apiDiagramId);
            return Ok(nodes);
        }

        [HttpGet("GetEdges")]
        public async Task<IActionResult> GetApiEdges(Guid apiDiagramId)
        {
            try
            {
                var result = await _aidiagramInterface.GetApiEdgesByDiagramIdAsync(apiDiagramId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving edges: {ex.Message}");
            }
        }
        [HttpPut("{apiDiagramId}/nodes/{nodeId}/position")]
        public async Task<IActionResult> UpdateNodePosition(Guid apiDiagramId, Guid nodeId, [FromBody] UpdateNodePositionDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var node = await _codePlannerDbContext.ApiTableNodes
                    .Include(n => n.ApiDiagram)
                    .FirstOrDefaultAsync(n =>
                        n.ApiTableNodesId == nodeId &&
                        n.ApiDiagramId == apiDiagramId &&
                        n.ApiDiagram.UserId == userId);

                if (node == null)
                    return NotFound("Node not found or access denied.");

                node.PositionX = dto.PositionX;
                node.PositionY = dto.PositionY;

                await _codePlannerDbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating position: {ex.Message}");
            }
        }
    }
    
}