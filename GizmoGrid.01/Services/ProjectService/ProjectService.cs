using GizmoGrid._01.Data;
using GizmoGrid._01.Dto;
using GizmoGrid._01.Dto.ProjectDto;
using GizmoGrid._01.Entity;
using GizmoGrid._01.Entity.Projectaccces;
using Microsoft.EntityFrameworkCore;

namespace GizmoGrid._01.Services.ProjectService
{
    public class ProjectService : IProjectService
    {
        private readonly CodePlannerDbContext _context;
        public ProjectService(CodePlannerDbContext context )
        {
            _context = context;
        }

        public async Task<Guid> CreateProjectAsync(Guid userId, ProjectCreateDto dto)
        {
            var project = new Project
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                UserId = userId
            };
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project.Id;
        }

        public async Task<List<Project>> GetProjectsAsync()
        {
            return await _context.Projects

                .ToListAsync();
        }

        public async Task<Project> GetProjectAsync(Guid userId, Guid projectId)
        {
            var project = await _context.Projects
                .Include(p => p.FlowDiagrams)
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);
            if (project == null)
                throw new KeyNotFoundException("Project not found.");
            return project;
        }

        public async Task UpdateProjectAsync(Guid userId, Guid projectId, ProjectUpdateDto dto)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);
            if (project == null)
                throw new KeyNotFoundException("Project not found.");

            project.Name = dto.Name ?? project.Name;
            project.Description = dto.Description ?? project.Description;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(Guid userId, Guid projectId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);
            if (project == null)
                throw new KeyNotFoundException("Project not found.");

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
        }


        public async Task<List<ProjectWithDesignsDto>> GetProjectsWithDesignsByUserIdAsync(Guid userId)
        {
            return await _context.Projects
                .Where(p => p.UserId == userId)
                .Include(p => p.FlowDiagrams)
                .Include(p => p.ApiDiagrams)
                .Include(p => p.SchemaDiagrams)
                .Select(p => new ProjectWithDesignsDto
                {
                    ProjectId = p.Id,
                    ProjectName = p.Name,
                    Description = p.Description,

                    FlowDiagrams = p.FlowDiagrams.Select(f => new FlowDiagramDto
                    {
                        Id = f.Id,
                        Name = f.Label,

                    }).ToList(),

                    ApiDiagrams = p.ApiDiagrams.Select(a => new ApiDiagramDto
                    {
                        Id = a.ApiDiagramId,
                        Name = a.Name
                    }).ToList(),

                    SchemaDiagrams = p.SchemaDiagrams.Select(s => new SchemaDiagramDto
                    {
                        Id = s.SchemaDiagramId,
                        Name = s.Name
                    }).ToList()
                })
                .ToListAsync();
        }
        public async Task AddUserToProject(Guid userId, Guid projectId)
        {
            var project = await _context.Projects
                .Include(p => p.ProjectMembers)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                throw new KeyNotFoundException("Project not found.");

            // Prevent duplicates
            if (project.ProjectMembers.Any(pm => pm.UserId == userId))
                return;

            project.ProjectMembers.Add(new ProjectMember
            {
                ProjectId = projectId,
                UserId = userId
            });

            await _context.SaveChangesAsync();
        }
    }
}
