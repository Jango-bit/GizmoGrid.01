using GizmoGrid._01.Data;
using GizmoGrid._01.Dto.SchemaDto;
using GizmoGrid._01.Entity.SchemaEntity;
using GizmoGrid._01.Repository.SchemaRepo;

namespace GizmoGrid._01.Services.SchemaServices
{
    public class SchemaService : ISchemaInterface
    {
        private readonly ISchemaRepo _schemaRepo;

        public SchemaService(ISchemaRepo schemaRepo)
        {
            _schemaRepo = schemaRepo;
        }

        public async Task<Guid> CreateSchemaDiagramAsync(Guid userId, SchemaDiagramCreateDto dto)
        {
            return await _schemaRepo.CreateSchemaDiagram(userId, dto);
        }

        public async Task AddTableColumnsBulkAsync(Guid userId, Guid tableNodeId, List<TableColumnCreateDto> columnDtos)
        {
            await _schemaRepo.AddTableColumnsBulkAsync(userId, tableNodeId, columnDtos);
        }
        public async Task<List<TableNodeDtoReturn>> CreateTableNodesAsync(Guid schemaDiagramId, List<TableNodeCreateDto> tableNodes, Guid userId)
        {
            var createdNodes = await _schemaRepo.CreateTableNodesAsync(schemaDiagramId, tableNodes, userId);

            return createdNodes.Select(node => new TableNodeDtoReturn
            {
                TableNodeId = node.TableNodeId,
                TableName = node.TableName,
                DatabaseName = node.DatabaseName,
                DatabaseEngine = node.DatabaseEngine,
                PositionX = node.PositionX,
                PositionY = node.PositionY,
                Columns = node.Columns.Select(col => new TableColumnDtoReturn
                {
                    ColumnId = col.TableColumnId,
                    ColumnName = col.ColumnName,
                    DataType = col.DataType,
                    IsPrimaryKey = col.IsPrimaryKey,
                    IsNullable = col.IsNullable,
                    IsUnique = col.IsUnique,
                    DefaultValue = col.DefaultValue,
                    ForeignKey = (col.ForeignKeyTable != null && col.ForeignKeyColumn != null)
                        ? new ForeignKeyReferenceDto
                        {
                            Table = col.ForeignKeyTable,
                            Column = col.ForeignKeyColumn
                        }
                        : null
                }).ToList()
            }).ToList();
        }


        public async Task<List<TableEdgeDtoReturn>> CreateTableEdgesAsync(Guid userId, Guid schemaDiagramId, List<TableEdgeCreateDto> edges)
        {
            try
            {
                return await _schemaRepo.AddEdgeAsync(userId, schemaDiagramId, edges);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error creating table edges in service layer.", ex);
            }
        
        }
        public async Task UpdateTableNodeAsync(Guid userId, Guid tableNodeId, TableNodeUpdateDto dto)
        {
            try
            {
                await _schemaRepo.UpdateTableNodeAsync(userId, tableNodeId, dto);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error updating table node.", ex);
            }
        }

        public async Task DeleteTableNodeAsync(Guid userId, Guid tableNodeId)
        {
            try
            {
                await _schemaRepo.DeleteTableNodeAsync(userId, tableNodeId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error deleting table node.", ex);
            }
        }
        public async Task<List<TableNodeDtoReturn>> GetTableNodesWithColumnsAsync(Guid schemaDiagramId)
        {
            var nodes = await _schemaRepo.GetTableNodesWithColumnsAsync(schemaDiagramId);

            return nodes.Select(n => new TableNodeDtoReturn
            {
                TableNodeId = n.TableNodeId,
                TableName = n.TableName,
                DatabaseName = n.DatabaseName,
                DatabaseEngine = n.DatabaseEngine,
                PositionX = n.PositionX,
                PositionY = n.PositionY,
                Columns = n.Columns.Select(c => new TableColumnDtoReturn
                {
                    ColumnId = c.TableColumnId,
                    ColumnName = c.ColumnName,
                    DataType = c.DataType,
                    IsPrimaryKey = c.IsPrimaryKey,
                    IsNullable = c.IsNullable,
                    IsUnique = c.IsUnique,
                    DefaultValue = c.DefaultValue,
                }).ToList()
            }).ToList();

        }
        public async Task<List<TableEdgeDtoReturn>> GetTableEdgesAsync(Guid userId, Guid schemaDiagramId)
        {
            return await _schemaRepo.GetTableEdgesAsync(userId, schemaDiagramId);
        }
    }
}