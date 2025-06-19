using GizmoGrid._01.Data;
using GizmoGrid._01.Dto;
using GizmoGrid._01.Entity;
using GizmoGrid._01.Repository.FlowRepo;
using GizmoGrid._01.Services.ImageUploader;
using Microsoft.EntityFrameworkCore;

public class FlowDiagramRepository : IFlowDiagramRepository
{
    private readonly CodePlannerDbContext _context;
    private readonly IImageUploader _imageUploader;

    public FlowDiagramRepository(
        CodePlannerDbContext context,
        IImageUploader imageUploader)
    {
        _context = context;
        _imageUploader = imageUploader;
    }

    public async Task<Guid> CreateFlowDiagramAsync(Guid userId, FlowDiagramCreateDto dto)
    {
        try
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == dto.ProjectId && p.UserId == userId);

            if (project == null)
                throw new KeyNotFoundException("Project not found.");

            var flowDiagram = new FlowDiagram
            {

                Id = Guid.NewGuid(),
                Label = dto.Name,
                ProjectId = dto.ProjectId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.FlowDiagrams.Add(flowDiagram);
            await _context.SaveChangesAsync();

            return flowDiagram.Id;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error creating flow diagram.", ex);
        }
    }



    public async Task<FlowDiagramDtoReturn> GetFlowDiagramAsync(Guid userId, Guid flowDiagramId)
    {
        var dto = await _context.FlowDiagrams
              .Include(fd => fd.Nodes) 
            .Where(fd => fd.Id == flowDiagramId && fd.UserId == userId)
            .Select(fd => new FlowDiagramDtoReturn
            {
                Id = fd.Id,
                Label = fd.Label,
                ProjectId = fd.ProjectId,
                Nodes = fd.Nodes

                    .Select(n => new NodeDtoReturn
                    {

                        Id = n.Id,
                        Label = n.Label,
                        Description = n.Description,
                        ImageUrl = n.Image,
                        PositionX = n.PositionX,
                        PositionY = n.PositionY
                        ,ImageSize = n.ImageSize
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();

        if (dto == null)
            throw new KeyNotFoundException("Flow diagram not found.");

        return dto;
    }
 
    public async Task<Guid> AddNodeAsync(Guid userId, Guid flowDiagramId, NodeCreateDto dto, string imageUrl)
    {
        var flowDiagram = await _context.FlowDiagrams
            .FirstOrDefaultAsync(fd => fd.Id == flowDiagramId && fd.UserId == userId);

        if (flowDiagram == null)
            throw new KeyNotFoundException("Flow diagram not found or access denied.");

        var node = new Node
        {
            Id = Guid.NewGuid(),
            FlowDiagramId = flowDiagramId,
            Label = dto.Label ?? string.Empty,
            Description = dto.Description ?? string.Empty,
            Image = imageUrl ?? string.Empty,
            ImageSize = dto.ImageSize ?? 1.0f,
            PositionX = (float)(dto.PositionX ?? 0),
            PositionY = (float)(dto.PositionY ?? 0)
        };

        _context.Nodes.Add(node);
        await _context.SaveChangesAsync();
        return node.Id;
    }

    public async Task UpdateNodeAsync(Guid userId, Guid nodeId, NodeUpdateDto dto, string imageUrl)
    {
        var node = await _context.Nodes
            .Include(n => n.FlowDiagram)
            .FirstOrDefaultAsync(n => n.Id == nodeId &&
                                      n.FlowDiagramId == dto.FlowDiagramId &&
                                      n.FlowDiagram.UserId == userId);

        if (node == null)
            throw new KeyNotFoundException("Node not found or access denied.");

        node.Label = dto.Label ?? node.Label;
        node.Description = dto.Description ?? node.Description;
        node.Image = !string.IsNullOrEmpty(imageUrl) ? imageUrl : node.Image;
        node.ImageSize = dto.ImageSize ?? node.ImageSize;
        node.PositionX = dto.PositionX ?? node.PositionX;
        node.PositionY = dto.PositionY ?? node.PositionY;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteNodeAsync(Guid userId, Guid flowDiagramId, Guid nodeId)
    {
        try
        {
            var node = await _context.Nodes
                .Include(n => n.FlowDiagram)
                .FirstOrDefaultAsync(n =>
                    n.Id == nodeId &&
                    n.FlowDiagramId == flowDiagramId &&
                    n.FlowDiagram.UserId == userId
                );

            if (node == null)
                throw new KeyNotFoundException("Node not found or access denied.");

            // 💥 Delete related edges (both where this node is source or target)
            var relatedEdges = await _context.Edges
                .Where(e => e.SourceId == nodeId || e.TargetId == nodeId)
                .ToListAsync();

            _context.Edges.RemoveRange(relatedEdges); // remove edges first ✅
            _context.Nodes.Remove(node);              // then remove node ✅

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error deleting node.", ex);
        }
    }

    public async Task<Edge> AddEdgeAsync(Guid userId, Guid flowDiagramId, EdgeCreateDto dto)
    {
        try
        {
            var flowDiagram = await _context.FlowDiagrams
                .FirstOrDefaultAsync(fd => fd.Id == flowDiagramId && fd.UserId == userId);

            if (flowDiagram == null)
                throw new KeyNotFoundException("Flow diagram not found or access denied.");

            var edge = new Edge
            {
                Id = Guid.NewGuid(),
                FlowDiagramId = flowDiagramId,
                SourceId = dto.SourceId,
                TargetId = dto.TargetId,
                SourceHandle = dto.SourceHandle ?? "default",
                TargetHandle = dto.TargetHandle ?? "default"
            };

            _context.Edges.Add(edge);
            await _context.SaveChangesAsync();

            return edge;
                
                
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error adding edge.", ex);
        }
    }
    public async Task<List<Edge>> GetEdgesByDiagramIdAsync(Guid userId, Guid diagramId)
    {
        Console.WriteLine($"Total Edges Found: {_context.Edges.Count()}");
        Console.WriteLine($"Edges for Diagram (all): {_context.Edges.Count(e => e.FlowDiagramId == diagramId)}");
        Console.WriteLine($"Edges for Diagram + User: {_context.Edges.Count(e => e.FlowDiagramId == diagramId && e.FlowDiagram.UserId == userId)}");

        return await _context.Edges
      .Include(e => e.FlowDiagram)
      .Include(e => e.SourceNode)
      .Include(e => e.TargetNode)
      .Where(e => e.FlowDiagramId == diagramId && e.FlowDiagram.UserId == userId)
      .ToListAsync();
    }


    public async Task<NodeDtoReturn> GetNodeByIdAsync(Guid userId, Guid nodeId)
    {
        var node = await _context.Nodes
            .Include(n => n.FlowDiagram)
            .Where(n => n.Id == nodeId && n.FlowDiagram.UserId == userId)
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

        if (node == null)
            throw new KeyNotFoundException("Node not found or access denied.");

        return node;
    }
}


