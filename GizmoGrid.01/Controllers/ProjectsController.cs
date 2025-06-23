using GizmoGrid._01.Dto;
using GizmoGrid._01.Services.ProjectService;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GizmoGrid._01.Dto.ProjectDto;
using GizmoGrid._01.Services.EmailService;

namespace GizmoGrid._01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IEmailService _emailService;
        private readonly IProjectInviteService _inviteService;


        public ProjectsController(IProjectService projectService,IEmailService emailService, IProjectInviteService inviteService)
        {
            _projectService = projectService;
            _emailService = emailService;
            _inviteService= inviteService;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var claimValue = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(claimValue) || !Guid.TryParse(claimValue, out var userId))
                    return Unauthorized("Invalid or missing user ID in claims.");
                var projectId = await _projectService.CreateProjectAsync(userId, dto);
                return Ok(new { Id = projectId });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var projects = await _projectService.GetProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(Guid id)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var project = await _projectService.GetProjectAsync(userId, id);
                return Ok(project);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(Guid id, [FromBody] ProjectUpdateDto dto)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _projectService.UpdateProjectAsync(userId, id, dto);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _projectService.DeleteProjectAsync(userId, id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
        [HttpGet("designs")]
        public async Task<IActionResult> GetMyProjectsWithDesigns()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var result = await _projectService.GetProjectsWithDesignsByUserIdAsync(userId);
            return Ok(result);
        }
        [HttpPost("{id:guid}/invite")]
        public async Task<IActionResult> InviteToProject(Guid id, [FromBody] InviteDto dto)
        {
            if (dto == null)
                return BadRequest("Request body is missing.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("Invitee email is required.");

            var ownerIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (ownerIdStr == null || !Guid.TryParse(ownerIdStr, out var ownerId))
                return Unauthorized("Invalid user ID.");

            // Now safe to call service
            await _inviteService.InviteAsync(id, ownerId, dto.Email);
            return Ok(new { message = $"Invitation sent to {dto.Email}" });
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinProject([FromBody] JoinDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _projectService.AddUserToProject(userId, dto.ProjectId);
            return Ok("Joined project successfully!");
        }

    }
}
