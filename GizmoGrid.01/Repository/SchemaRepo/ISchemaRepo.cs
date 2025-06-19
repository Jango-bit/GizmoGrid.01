using GizmoGrid._01.Dto;
using GizmoGrid._01.Dto.SchemaDto;
using GizmoGrid._01.Entity;
using GizmoGrid._01.Entity.SchemaEntity;

namespace GizmoGrid._01.Repository.SchemaRepo
{
    public interface ISchemaRepo
    {
        Task<Guid> CreateSchemaDiagram(Guid UserId, SchemaDiagramCreateDto dto);
        Task AddTableColumnsBulkAsync(Guid userId, Guid tableNodeId, List<TableColumnCreateDto> columnDtos);
        Task<List<TableNode>> CreateTableNodesAsync(Guid schemaDiagramId, List<TableNodeCreateDto> tableNodes, Guid userId);

        Task<TableNode> CreateTableNodeAsync(Guid schemaDiagramId, TableNodeCreateDto dto);
        //Task<List<Guid>> AddEdgeAsync(Guid userId, Guid schemaDiagramId, List<TableEdgeCreateDto> edges);
        Task<List<TableEdgeDtoReturn>> AddEdgeAsync(Guid userId, Guid schemaDiagramId, List<TableEdgeCreateDto> edges);
        Task UpdateTableNodeAsync(Guid userId, Guid tableNodeId, TableNodeUpdateDto dto);
        Task DeleteTableNodeAsync(Guid userId, Guid tableNodeId);
        Task<List<TableNode>> GetTableNodesWithColumnsAsync(Guid schemaDiagramId);

        Task<List<TableEdgeDtoReturn>> GetTableEdgesAsync(Guid userId, Guid schemaDiagramId);

    }
}
