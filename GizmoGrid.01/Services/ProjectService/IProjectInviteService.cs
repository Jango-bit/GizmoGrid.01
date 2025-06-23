namespace GizmoGrid._01.Services.ProjectService
{
    public interface IProjectInviteService
    {
        Task InviteAsync(Guid projectId, Guid ownerId, string inviteeEmail);

    }
}
