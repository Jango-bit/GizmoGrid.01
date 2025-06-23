using GizmoGrid._01.Services.EmailService;

namespace GizmoGrid._01.Services.ProjectService
{
    public class ProjectInviteService : IProjectInviteService
    {
        private readonly IProjectService _projectService;
        private readonly IEmailService _emailService;

        public ProjectInviteService(IProjectService projectService, IEmailService emailService)
        {
            _projectService = projectService;
            _emailService = emailService;
        }

        public async Task InviteAsync(Guid projectId, Guid ownerId, string inviteeEmail)
        {
            var project = await _projectService.GetProjectAsync(ownerId, projectId);
            var subject = $"Invite to collaborate on '{project.Name}'";
            var body = $"You've been invited to collaborate on '{project.Name}'.\n\nProject ID: {project.Id}";
            await _emailService.SendAsync(inviteeEmail, subject, body);
        }
    }
}