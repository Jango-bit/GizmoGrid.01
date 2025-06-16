using System.Security.Claims;
using GizmoGrid._01.Dto;
using GizmoGrid._01.Dto.ApiCreateDto;
using GizmoGrid._01.Entity;
using GizmoGrid._01.Services.ApiServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GizmoGrid._01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiDiagramController : ControllerBase
    {
        private readonly IApidiagramInterface _aidiagramInterface;
        public ApiDiagramController(IApidiagramInterface aidiagramInterface)
        {
            _aidiagramInterface = aidiagramInterface;
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

                var edgeId = await _aidiagramInterface.AddApiEdgeAsync(userId, apiDiagramId, dto);

                return Ok(new { Id = edgeId });
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
    }
}