using System.Security.Claims;
using GizmoGrid._01.Data;
using GizmoGrid._01.Dto.SchemaDto;
using GizmoGrid._01.Entity;
using GizmoGrid._01.Services.SchemaServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GizmoGrid._01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemaController : ControllerBase
    {
        private readonly ISchemaInterface _schemaInterface;
        private readonly CodePlannerDbContext _codePlannerDbContext;
        public SchemaController(ISchemaInterface schemaInterface,CodePlannerDbContext codePlannerDbContext)
        {
            _schemaInterface = schemaInterface;
            _codePlannerDbContext = codePlannerDbContext;   
        }
        [HttpPost("CreateSchemaDiagram")]
        public async Task<IActionResult> CreateSchema([FromBody] SchemaDiagramCreateDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var schemaDiagramId = await _schemaInterface.CreateSchemaDiagramAsync(userId, dto);
                return Ok(new { SchemaDiagramId = schemaDiagramId });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }
    
        [HttpPost("{schemaDiagramId}/tablenodes")]
        public async Task<IActionResult> CreateTableNodes(Guid schemaDiagramId, [FromBody] List<TableNodeCreateDto> tableNodes)
        {
            if (tableNodes == null || !tableNodes.Any())
            {
                return BadRequest(new { Message = "No table nodes provided." });
            }

            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                var createdNodes = await _schemaInterface.CreateTableNodesAsync(schemaDiagramId, tableNodes, userId);

                return Ok(createdNodes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Failed to create table nodes.",
                    Error = ex.Message
                });
            }
        }

        [HttpPost("nodes/{tableNodeId}/columns/bulk")]
        public async Task<IActionResult> AddColumnsBulk(Guid tableNodeId, [FromBody] BulkTableColumnCreateDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _schemaInterface.AddTableColumnsBulkAsync(userId, tableNodeId, dto.Columns);
                return Ok("Columns added successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{schemaDiagramId}/edges")]
        public async Task<IActionResult> CreateTableEdges(Guid schemaDiagramId, [FromBody] List<TableEdgeCreateDto> edges)
        {


            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                var edgeIds = await _schemaInterface.CreateTableEdgesAsync(userId, schemaDiagramId, edges);
                return Ok(edgeIds);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
        [HttpPut("nodes/{tableNodeId}")]
        public async Task<IActionResult> UpdateTableNode(Guid tableNodeId, [FromBody] TableNodeUpdateDto dto)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdClaim))
                return Unauthorized(new { message = "User is not authenticated." });

            var userId = Guid.Parse(userIdClaim);
            try
            {
                await _schemaInterface.UpdateTableNodeAsync(userId, tableNodeId, dto);
                return Ok("Succes");
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(new { message = knf.Message });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error", details = ex.Message });
            }
        }
        [HttpDelete("nodes/{tableNodeId}")]
        public async Task<IActionResult> DeleteTableNode(Guid tableNodeId)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userIdClaim))
                return Unauthorized(new { message = "User is not authenticated." });

            var userId = Guid.Parse(userIdClaim);

            try
            {
                await _schemaInterface.DeleteTableNodeAsync(userId, tableNodeId);
                return Ok("U");
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(new { message = knf.Message });
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error", details = ex.Message });
            }
        }
        [HttpGet("{schemaDiagramId}/tablenodes")]
        public async Task<IActionResult> GetTableNodes(Guid schemaDiagramId)
        {
            var nodes = await _schemaInterface.GetTableNodesWithColumnsAsync(schemaDiagramId);
            return Ok(nodes);
        }
        [HttpGet("{schemaDiagramId}/edges")]
        public async Task<IActionResult> GetTableEdges(Guid schemaDiagramId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var edges = await _schemaInterface.GetTableEdgesAsync(userId, schemaDiagramId);
                return Ok(edges);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
        [HttpPut("{schemaDiagramId}/nodes/{nodeId}/position")]
        public async Task<IActionResult> UpdateNodePosition(Guid schemaDiagramId, Guid nodeId, [FromBody] Tableposition dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var node = await _codePlannerDbContext.TableNodes
                    .Include(n => n.SchemaDiagram)
                    .FirstOrDefaultAsync(n => n.TableNodeId == nodeId && n.SchemaDiagramId == schemaDiagramId && n.SchemaDiagram.UserId == userId);

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
