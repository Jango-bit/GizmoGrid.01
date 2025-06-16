using GizmoGrid._01.Dto.SchemaDto;
using GizmoGrid._01.Entity.SchemaEntity;

namespace GizmoGrid._01.Services.SchemaServices
{
    public interface ISchemaInterface
    {
        Task<Guid> CreateSchemaDiagramAsync(Guid userId, SchemaDiagramCreateDto dto);
        Task AddTableColumnsBulkAsync(Guid userId, Guid tableNodeId, List<TableColumnCreateDto> columnDtos);
            Task<List<Guid>> CreateTableEdgesAsync(Guid userId, Guid schemaDiagramId, List<TableEdgeCreateDto> edges);
        Task UpdateTableNodeAsync(Guid userId, Guid tableNodeId, TableNodeUpdateDto dto);
        Task DeleteTableNodeAsync(Guid userId, Guid tableNodeId);
        Task<List<TableNodeDtoReturn>> CreateTableNodesAsync(Guid schemaDiagramId, List<TableNodeCreateDto> tableNodes, Guid userId);
        Task<List<TableNodeDtoReturn>> GetTableNodesWithColumnsAsync(Guid schemaDiagramId);
    }
}
