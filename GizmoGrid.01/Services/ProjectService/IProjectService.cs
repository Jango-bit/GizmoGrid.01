using GizmoGrid._01.Dto;
using GizmoGrid._01.Dto.ProjectDto;
using GizmoGrid._01.Entity;

namespace GizmoGrid._01.Services.ProjectService
{
    public interface IProjectService
    {
        Task<Guid> CreateProjectAsync(Guid userId, ProjectCreateDto dto);
        Task<List<Project>> GetProjectsAsync();
        Task<Project> GetProjectAsync(Guid userId, Guid projectId);
        Task UpdateProjectAsync(Guid userId, Guid projectId, ProjectUpdateDto dto);
        Task DeleteProjectAsync(Guid userId, Guid projectId);
        Task<List<ProjectWithDesignsDto>> GetProjectsWithDesignsByUserIdAsync(Guid userId);


        Task AddUserToProject(Guid userId, Guid projectId);
    }
}
