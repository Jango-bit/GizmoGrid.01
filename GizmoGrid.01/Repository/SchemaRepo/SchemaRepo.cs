using GizmoGrid._01.Data;
using GizmoGrid._01.Dto.SchemaDto;
using GizmoGrid._01.Entity;
using GizmoGrid._01.Entity.SchemaEntity;
using Microsoft.EntityFrameworkCore;

namespace GizmoGrid._01.Repository.SchemaRepo
{
    public class SchemaRepo : ISchemaRepo
    {
        private readonly CodePlannerDbContext _codePlannerDbContext;
        public SchemaRepo(CodePlannerDbContext codePlannerDbContext)
        {
            _codePlannerDbContext = codePlannerDbContext;
        }
        public async Task<Guid> CreateSchemaDiagram(Guid UserId, SchemaDiagramCreateDto dto)
        {
            try
            {
                var project = await _codePlannerDbContext.Projects
                .FirstOrDefaultAsync(p => p.Id == dto.ProjectId && p.UserId == UserId);
                if (project == null)
                {
                    throw new KeyNotFoundException("Project not found.");
                }
                var schemaDiagram = new SchemaDiagram
                {
                    SchemaDiagramId = Guid.NewGuid(),
                    ProjectId = dto.ProjectId,
                    UserId = UserId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _codePlannerDbContext.Add(schemaDiagram);
                await _codePlannerDbContext.SaveChangesAsync();

                return schemaDiagram.SchemaDiagramId;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error creating Schema diagram.", ex);
            }
        }

        public async Task<List<TableNode>> CreateTableNodesAsync(Guid schemaDiagramId, List<TableNodeCreateDto> tableNodes, Guid userId)
        {
            try
            {
                var createdNodes = new List<TableNode>();

                foreach (var nodeDto in tableNodes)
                {
                    var tableNode = new TableNode
                    {
                        TableNodeId = Guid.NewGuid(),
                        SchemaDiagramId = schemaDiagramId,
                        TableName = nodeDto.TableName,
                        DatabaseName = nodeDto.DatabaseName,
                        DatabaseEngine = nodeDto.DatabaseEngine,
                        PositionX = nodeDto.PositionX,
                        PositionY = nodeDto.PositionY,
                        Columns = nodeDto.Columns.Select(c => new TableColumn
                        {
                            TableColumnId = Guid.NewGuid(),
                            ColumnName = c.ColumnName,
                            DataType = c.DataType,
                            IsPrimaryKey = c.IsPrimaryKey,
                            IsNullable = c.IsNullable,
                            IsUnique = c.IsUnique,
                            DefaultValue = c.DefaultValue,
                            ForeignKeyTable = c.ForeignKey?.Table,
                            ForeignKeyColumn = c.ForeignKey?.Column
                        }).ToList()
                    };

                    createdNodes.Add(tableNode);
                    _codePlannerDbContext.TableNodes.Add(tableNode);
                }

                await _codePlannerDbContext.SaveChangesAsync();
                return createdNodes;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error creating table nodes.", ex);
            }
        }

        public async Task AddTableColumnsBulkAsync(Guid userId, Guid tableNodeId, List<TableColumnCreateDto> columnDtos)
        {
            var tableNode = await _codePlannerDbContext.TableNodes
                .Include(t => t.SchemaDiagram)
                .FirstOrDefaultAsync(t => t.TableNodeId == tableNodeId && t.SchemaDiagram.UserId == userId);

            if (tableNode == null)
                throw new KeyNotFoundException("TableNode not found or access denied.");

            var newColumns = columnDtos.Select(dto => new TableColumn
            {
                TableColumnId = Guid.NewGuid(),
                TableNodeId = tableNodeId,
                ColumnName = dto.ColumnName,
                DataType = dto.DataType,
                IsPrimaryKey = dto.IsPrimaryKey,
                IsNullable = dto.IsNullable,
                IsUnique = dto.IsUnique,
                DefaultValue = dto.DefaultValue
            }).ToList();

            // Step 3: Bulk insert
            await _codePlannerDbContext.TableColumns.AddRangeAsync(newColumns);
            await _codePlannerDbContext.SaveChangesAsync();
        }



        public async Task<List<TableEdgeDtoReturn>> AddEdgeAsync(Guid userId, Guid schemaDiagramId, List<TableEdgeCreateDto> edges)
        {
            try
            {
                var schemaDiagram = await _codePlannerDbContext.SchemaDiagrams
                    .FirstOrDefaultAsync(i => i.SchemaDiagramId == schemaDiagramId && i.UserId == userId);

                if (schemaDiagram == null)
                    throw new KeyNotFoundException("Schema diagram not found or access denied.");

                var createdEdges = new List<TableEdgeDtoReturn>();

                foreach (var dto in edges)
                {
                    var edgeId = Guid.NewGuid();

                    var tableEdge = new TableEdges
                    {
                        EdgeId = edgeId,
                        SchemaDiagramId = schemaDiagramId,
                        SourceId = dto.SourceId,
                        TargetId = dto.TargetId,
                    };

                    _codePlannerDbContext.TableEdges.Add(tableEdge);

                    createdEdges.Add(new TableEdgeDtoReturn
                    {
                        EdgeId = edgeId,
                        SourceId = dto.SourceId,
                        TargetId = dto.TargetId
                    });
                }

                await _codePlannerDbContext.SaveChangesAsync();
                return createdEdges;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error adding edges to schema diagram.", ex);
            }
        }
        public async Task UpdateTableNodeAsync(Guid userId, Guid tableNodeId, TableNodeUpdateDto dto)
        {
            var tableNode = await _codePlannerDbContext.TableNodes
                .Include(t => t.Columns)
                .Include(t => t.SchemaDiagram)
                .FirstOrDefaultAsync(t => t.TableNodeId == tableNodeId);

            if (tableNode == null)
                throw new KeyNotFoundException("Table node not found.");

            if (tableNode.SchemaDiagram.UserId != userId)
                throw new UnauthorizedAccessException("You do not have permission to update this node.");

            // Update properties
            tableNode.TableName = dto.TableName;
            tableNode.DatabaseEngine = dto.DatabaseEngine;
            tableNode.DatabaseName = dto.DatabaseName;
            tableNode.PositionX = dto.PositionX;
            tableNode.PositionY = dto.PositionY;

            // Update columns
            foreach (var colDto in dto.Columns)
            {
                var existingColumn = tableNode.Columns.FirstOrDefault(c => c.TableColumnId == colDto.TableColumnId);
                if (existingColumn != null)
                {
                    existingColumn.ColumnName = colDto.ColumnName;
                    existingColumn.DataType = colDto.DataType;
                    existingColumn.IsPrimaryKey = colDto.IsPrimaryKey;
                    existingColumn.IsNullable = colDto.IsNullable;
                    existingColumn.IsUnique = colDto.IsUnique;
                    existingColumn.DefaultValue = colDto.DefaultValue;
                }
            }

            await _codePlannerDbContext.SaveChangesAsync();
        }


        public async Task DeleteTableNodeAsync(Guid userId, Guid tableNodeId)
        {
            var tableNode = await _codePlannerDbContext.TableNodes
                .Include(t => t.SchemaDiagram)
                .Include(t => t.Columns)
                .FirstOrDefaultAsync(t => t.TableNodeId == tableNodeId);

            if (tableNode == null)
                throw new KeyNotFoundException("Table node not found.");

            if (tableNode.SchemaDiagram.UserId != userId)
                throw new UnauthorizedAccessException("You do not have permission to delete this node.");

            var relatedEdges = await _codePlannerDbContext.TableEdges
                .Where(e => e.SourceId == tableNodeId || e.TargetId == tableNodeId)
                .ToListAsync();
            _codePlannerDbContext.TableEdges.RemoveRange(relatedEdges);

            _codePlannerDbContext.TableColumns.RemoveRange(tableNode.Columns);
            _codePlannerDbContext.TableNodes.Remove(tableNode);

            await _codePlannerDbContext.SaveChangesAsync();
        }
        public async Task<TableNode> CreateTableNodeAsync(Guid schemaDiagramId, TableNodeCreateDto dto)
        {
            var diagram = await _codePlannerDbContext.SchemaDiagrams.FindAsync(schemaDiagramId);
            if (diagram == null)
                throw new KeyNotFoundException("SchemaDiagram not found");

            var tableNode = new TableNode
            {
                TableNodeId = Guid.NewGuid(),
                TableName = dto.TableName,
                DatabaseName = dto.DatabaseName,
                DatabaseEngine = dto.DatabaseEngine,
                PositionX = dto.PositionX,
                PositionY = dto.PositionY,
                SchemaDiagramId = schemaDiagramId,
                Columns = dto.Columns.Select(c => new TableColumn
                {
                    TableColumnId = Guid.NewGuid(),
                    ColumnName = c.ColumnName,
                    DataType = c.DataType,
                    IsPrimaryKey = c.IsPrimaryKey,
                    IsNullable = c.IsNullable,
                    IsUnique = c.IsUnique,
                    DefaultValue = c.DefaultValue,
                }).ToList()
            };

            _codePlannerDbContext.TableNodes.Add(tableNode);
            await _codePlannerDbContext.SaveChangesAsync();

            return tableNode;
        }
        public async Task<List<TableNode>> GetTableNodesWithColumnsAsync(Guid schemaDiagramId)
        {
            return await _codePlannerDbContext.TableNodes
                .Where(n => n.SchemaDiagramId == schemaDiagramId)
                .Include(n => n.Columns)
                .ToListAsync();
        }

        public async Task<List<TableEdgeDtoReturn>> GetTableEdgesAsync(Guid userId, Guid schemaDiagramId)
        {
            var diagram = await _codePlannerDbContext.SchemaDiagrams
                .FirstOrDefaultAsync(d => d.SchemaDiagramId == schemaDiagramId && d.UserId == userId);

            if (diagram == null)
                throw new KeyNotFoundException("Schema not found or access denied.");

            return await _codePlannerDbContext.TableEdges
                .Where(e => e.SchemaDiagramId == schemaDiagramId)
                .Select(e => new TableEdgeDtoReturn
                {
                    EdgeId = e.EdgeId,
                    SourceId = e.SourceId,
                    TargetId = e.TargetId
                })
                .ToListAsync();
        }
    }
}