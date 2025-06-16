

using GizmoGrid._01.Dto;
using GizmoGrid._01.Entity;
using GizmoGrid._01.Repository.FlowRepo;
using GizmoGrid._01.Services.ImageUploader;

namespace GizmoGrid._01.Services.FlowDiagramService
{
    public class FlowDiagramService : IFlowDiagramService
    {
        private readonly IFlowDiagramRepository _flowDiagramRepository;
        private readonly IImageUploader _imageUploader;

        public FlowDiagramService(
            IFlowDiagramRepository flowDiagramRepository,
            IImageUploader imageUploader)
        {
            _flowDiagramRepository = flowDiagramRepository;
            _imageUploader = imageUploader;
        }

        public async Task<Guid> CreateFlowDiagramAsync(Guid userId, FlowDiagramCreateDto dto)
        {
            return await _flowDiagramRepository.CreateFlowDiagramAsync(userId, dto);
        }

        public async Task<FlowDiagramDtoReturn> GetFlowDiagramAsync(Guid userId, Guid flowDiagramId)
        {
            return await _flowDiagramRepository.GetFlowDiagramAsync(userId, flowDiagramId);
        }

        //public async Task<Guid> AddNodeAsync(Guid userId, Guid flowDiagramId, NodeCreateDto dto)
        //{
        //    var imageUrl = await _imageUploader.UploadImage(dto.Image);
        //    return await _flowDiagramRepository.AddNodeAsync(userId, flowDiagramId, dto, imageUrl);
        //}

        //public async Task UpdateNodeAsync(Guid userId, Guid nodeId, NodeUpdateDto dto)
        //{
        //    var imageUrl = await _imageUploader.UploadImage(dto.Image);
        //    await _flowDiagramRepository.UpdateNodeAsync(userId, nodeId, dto, imageUrl);
        //}
        public async Task<Guid> AddNodeAsync(Guid userId, Guid flowDiagramId, NodeCreateDto dto)
        {
            var imageResult = await _imageUploader.UploadImage(dto.Image);
            dto.ImageSize = dto.ImageSize ?? imageResult.ImageSize;
            return await _flowDiagramRepository.AddNodeAsync(userId, flowDiagramId, dto, imageResult.Url);
        }

        public async Task UpdateNodeAsync(Guid userId, Guid nodeId, NodeUpdateDto dto)
        {
            string imageUrl = null;
            if (dto.Image != null)
            {
                var imageResult = await _imageUploader.UploadImage(dto.Image);
                imageUrl = imageResult.Url;
                dto.ImageSize = dto.ImageSize ?? imageResult.ImageSize;
            }
            await _flowDiagramRepository.UpdateNodeAsync(userId, nodeId, dto, imageUrl);
        }
        public async Task DeleteNodeAsync(Guid userId, Guid nodeId)
        {
            await _flowDiagramRepository.DeleteNodeAsync(userId, nodeId);
        }

        public async Task<Edge> AddEdgeAsync(Guid userId, Guid flowDiagramId, EdgeCreateDto dto)
        {
            return await _flowDiagramRepository.AddEdgeAsync(userId, flowDiagramId, dto);
        }

        public async Task<List<EdgeReturnDto>> GetEdgesDtoByDiagramIdAsync(Guid userId, Guid diagramId)
        {
            var edges = await _flowDiagramRepository.GetEdgesByDiagramIdAsync(userId, diagramId);

            return edges.Select(e => new EdgeReturnDto
            {
                Id = e.Id,
                SourceId = e.SourceId,
                TargetId = e.TargetId,
                SourceHandle = "source", // or e.SourceNode.Label or any string logic
                TargetHandle = "target"
            }).ToList();
        }
        public async Task<NodeDtoReturn> GetNodeByIdAsync(Guid userId, Guid nodeId)
        {
            return await _flowDiagramRepository.GetNodeByIdAsync(userId, nodeId);
        }
    }
}

