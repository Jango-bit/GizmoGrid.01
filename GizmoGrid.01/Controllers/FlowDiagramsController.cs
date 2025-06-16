using GizmoGrid._01.Dto;
using GizmoGrid._01.Services.FlowDiagramService;
using GizmoGrid._01.Services.ImageUploader;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GizmoGrid._01.Data;
using GizmoGrid._01.Entity;

namespace GizmoGrid._01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlowDiagramsController : ControllerBase
    {
        private readonly IFlowDiagramService _flowDiagramService;
        private readonly IImageUploader _imageUploader;
        private readonly CodePlannerDbContext _codePlannerDbContext;

        public FlowDiagramsController(IFlowDiagramService flowDiagramService, IImageUploader imageUploader ,CodePlannerDbContext codePlannerDbContext)
        {
            _flowDiagramService = flowDiagramService;
            _imageUploader = imageUploader;
            _codePlannerDbContext = codePlannerDbContext;   
        }

        [HttpPost]
        public async Task<IActionResult> CreateFlowDiagram([FromBody] FlowDiagramCreateDto dto)
        {

            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var flowDiagramId = await _flowDiagramService.CreateFlowDiagramAsync(userId, dto);
                return Ok(new { Id = flowDiagramId });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlowDiagram(Guid id)
        {
            try
            {
                var userIdString = HttpContext.Items["UserId"].ToString();
                if (!Guid.TryParse(userIdString, out var userId))
                    return Unauthorized("Invalid user ID format.");
                Console.WriteLine(userId);
                var flowDiagram = await _flowDiagramService.GetFlowDiagramAsync(userId, id);
                Console.WriteLine($"NODE COUNT: {flowDiagram.Nodes.Count}");

                return Ok(flowDiagram);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        //[HttpPost("{flowDiagramId}/nodes")]
        //public async Task<IActionResult> AddNode(Guid flowDiagramId, [FromForm] NodeCreateDto dto)
        //{

        //    try
        //    {
        //        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        //        var nodeId = await _flowDiagramService.AddNodeAsync(userId, flowDiagramId, dto);
        //        return Ok(new { Id = nodeId });
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        return NotFound();
        //    }
        //}

        //[HttpPut("Updatenodes/{nodeId}")]
        //public async Task<IActionResult> UpdateNode(Guid nodeId, [FromForm] NodeUpdateDto dto)
        //{
        //    try
        //    {
        //        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        //        await _flowDiagramService.UpdateNodeAsync(userId, nodeId, dto);
        //        return Ok("Updated!");
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        return NotFound();
        //    }
        //}
        [HttpPost("{flowDiagramId}/nodes")]
        public async Task<IActionResult> AddNode(Guid flowDiagramId, [FromForm] NodeCreateDto dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest("Form data binding failed. Make sure all form keys match the DTO property names exactly.");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var nodeId = await _flowDiagramService.AddNodeAsync(userId, flowDiagramId, dto);
                var node = await _codePlannerDbContext.Nodes
                    .Where(n => n.Id == nodeId)
                    .Select(n => new NodeDtoReturn
                    {
                        Id = n.Id,
                        Label = n.Label,
                        Description = n.Description,
                        ImageUrl = n.Image,
                        ImageSize = n.ImageSize,
                        PositionX = n.PositionX,
                        PositionY = n.PositionY
                    })
                    .FirstOrDefaultAsync();
                return Ok(node);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error adding node: {ex.Message}");
            }
        }

        [HttpPut("Updatenodes/{nodeId}")]
        public async Task<IActionResult> UpdateNode(Guid nodeId, [FromForm] NodeUpdateDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _flowDiagramService.UpdateNodeAsync(userId, nodeId, dto);
                return Ok("Updated!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating node: {ex.Message}");
            }
        }
        [HttpDelete("Deletenodes/{nodeId}")]
        public async Task<IActionResult> DeleteNode(Guid nodeId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _flowDiagramService.DeleteNodeAsync(userId, nodeId);
                return Ok("Deleted Succesfully");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpPost("{flowDiagramId}/edges")]
        public async Task<IActionResult> AddEdge(Guid flowDiagramId, [FromBody] EdgeCreateDto dto)
        {


            try
            {
                var userIdString = HttpContext.Items["UserId"].ToString();
                if (!Guid.TryParse(userIdString, out var userId))
                    return Unauthorized("Invalid user ID format.");
                var edgeId = await _flowDiagramService.AddEdgeAsync(userId, flowDiagramId, dto);
                return Ok(new
                {
                   edgeId.Id,
                   edgeId.SourceId,
                   edgeId.TargetId,
                   edgeId.SourceHandle,
                   edgeId.TargetHandle,

                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        [HttpGet("{id}/edges")]
        public async Task<IActionResult> GetEdges(Guid id)
        {
            try
            {
                var userIdString = HttpContext.Items["UserId"]?.ToString();
                if (!Guid.TryParse(userIdString, out var userId))
                    return Unauthorized("Invalid user ID.");

                var edges = await _flowDiagramService.GetEdgesDtoByDiagramIdAsync(userId, id);
                return Ok(edges);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching edges: {ex.Message}");
            }
        }
        [HttpGet("Nodes/{nodeId}")]
        public async Task<IActionResult> GetNode(Guid nodeId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var node = await _flowDiagramService.GetNodeByIdAsync(userId, nodeId);
                return Ok(node);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Node not found or access denied.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching node: {ex.Message}");
            }
        }
        [HttpPut("{flowDiagramId}/nodes/{nodeId}/position")]
        public async Task<IActionResult> UpdateNodePosition(Guid flowDiagramId, Guid nodeId, [FromBody] nodepositionUpdateDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var node = await _codePlannerDbContext.Nodes
                    .Include(n => n.FlowDiagram)
                    .FirstOrDefaultAsync(n => n.Id == nodeId && n.FlowDiagramId == flowDiagramId && n.FlowDiagram.UserId == userId);

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
        [HttpPut("{nodeId}/imagesize")]
        public async Task<IActionResult> UpdateImageSize(Guid nodeId, [FromBody] UpdateImageSizeDto dto)
        {
            if (dto.ImageSize <= 0)
                return BadRequest("ImageSize must be greater than 0");

            var node = await _codePlannerDbContext.Nodes.FindAsync(nodeId);
            if (node == null)
                return NotFound("Node not found");

            node.ImageSize = dto.ImageSize;

            await _codePlannerDbContext.SaveChangesAsync();

            return Ok(new
            {
                node.Id,
                node.ImageSize
            });
        }

    }

}

