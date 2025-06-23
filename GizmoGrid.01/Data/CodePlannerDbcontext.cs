





using GizmoGrid._01.Entity;
using GizmoGrid._01.Entity.Api_sEntity;
using GizmoGrid._01.Entity.Projectaccces;
using GizmoGrid._01.Entity.SchemaEntity;
using Microsoft.EntityFrameworkCore;

namespace GizmoGrid._01.Data
{
    public class CodePlannerDbContext : DbContext
    {
        public CodePlannerDbContext(DbContextOptions<CodePlannerDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<FlowDiagram> FlowDiagrams { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Edge> Edges { get; set; }

        public DbSet<SchemaDiagram> SchemaDiagrams { get; set; }
        public DbSet<TableNode> TableNodes { get; set; }
        public DbSet<TableColumn> TableColumns { get; set; }
        public DbSet<TableEdges> TableEdges { get; set; }

        public DbSet<ApiDiagram> ApiDiagrams { get; set; }
        public DbSet<ApiTableNodes> ApiTableNodes { get; set; }
        public DbSet<ApiEdges> ApiEdges { get; set; }
        public DbSet<ProjectAccess> ProjectAccesses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Unique Email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Project → User
            modelBuilder.Entity<Project>()
                .HasOne(p => p.User)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // FlowDiagram → Project
            modelBuilder.Entity<FlowDiagram>()
                .HasOne(fd => fd.Project)
                .WithMany(p => p.FlowDiagrams)
                .HasForeignKey(fd => fd.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // FlowDiagram → User
            modelBuilder.Entity<FlowDiagram>()
                .HasOne(fd => fd.User)
                .WithMany()
                .HasForeignKey(fd => fd.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Node → FlowDiagram
            modelBuilder.Entity<Node>()
                .HasOne(n => n.FlowDiagram)
                .WithMany(fd => fd.Nodes)
                .HasForeignKey(n => n.FlowDiagramId);

            // Edge Source/Target
            modelBuilder.Entity<Edge>()
                .HasOne(e => e.SourceNode)
                .WithMany(n => n.OutgoingEdges)
                .HasForeignKey(e => e.SourceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Edge>()
                .HasOne(e => e.TargetNode)
                .WithMany(n => n.IncomingEdges)
                .HasForeignKey(e => e.TargetId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Edge>()
                .HasOne(e => e.FlowDiagram)
                .WithMany(fd => fd.Edges)
                .HasForeignKey(e => e.FlowDiagramId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Edge>(entity =>
            {
                entity.Property(e => e.SourceHandle).IsRequired();
                entity.Property(e => e.TargetHandle).IsRequired();
            });

            // SchemaDiagram → TableNodes
            modelBuilder.Entity<SchemaDiagram>()
                .HasMany(sd => sd.TableNodes)
                .WithOne(t => t.SchemaDiagram)
                .HasForeignKey(t => t.SchemaDiagramId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ SchemaDiagram → Project (Fix)
            modelBuilder.Entity<SchemaDiagram>()
                .HasOne(sd => sd.Project)
                .WithMany(p => p.SchemaDiagrams)
                .HasForeignKey(sd => sd.ProjectId)
                .OnDelete(DeleteBehavior.Restrict); // or Restrict if preferred

            modelBuilder.Entity<TableNode>()
                .HasMany(t => t.Columns)
                .WithOne(c => c.TableNode)
                .HasForeignKey(c => c.TableNodeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TableEdges>()
                .HasKey(e => e.EdgeId);

            modelBuilder.Entity<TableEdges>()
                .HasOne(e => e.SourceNode)
                .WithMany(n => n.OutgoingEdges)
                .HasForeignKey(e => e.SourceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TableEdges>()
                .HasOne(e => e.TargetNode)
                .WithMany(n => n.IncomingEdges)
                .HasForeignKey(e => e.TargetId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TableColumn>()
                .HasOne(c => c.ForeignKeyReferenceColumn)
                .WithMany()
                .HasForeignKey(c => c.ForeignKeyReferenceColumnId)
                .OnDelete(DeleteBehavior.Restrict);

            // ApiDiagram → User
            modelBuilder.Entity<ApiDiagram>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApiDiagram>()
                .HasMany(a => a.ApiTableNodes)
                .WithOne(n => n.ApiDiagram)
                .HasForeignKey(n => n.ApiDiagramId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApiDiagram>()
                .HasMany(a => a.ApiEdges)
                .WithOne(e => e.ApiDiagram)
                .HasForeignKey(e => e.ApiDiagramId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApiTableNodes>()
                .HasKey(n => n.ApiTableNodesId);

            modelBuilder.Entity<ApiEdges>()
                .HasKey(e => e.ApiEdgesId);

            modelBuilder.Entity<ApiEdges>()
                .HasOne(e => e.SourceNode)
                .WithMany(n => n.OutgoingEdges)
                .HasForeignKey(e => e.SourceId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApiEdges>()
                .HasOne(e => e.TargetNode)
                .WithMany(n => n.IncomingEdges)
                .HasForeignKey(e => e.TargetId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectMember>()
    .HasKey(pm => new { pm.ProjectId, pm.UserId });

            modelBuilder.Entity<ProjectAccess>()
    .HasOne(pa => pa.User)
    .WithMany(u => u.ProjectAccesses)
    .HasForeignKey(pa => pa.UserId)
    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectAccess>()
                .HasOne(pa => pa.Project)
                .WithMany(p => p.ProjectAccesses)
                .HasForeignKey(pa => pa.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add for ProjectMember
            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.User)
                .WithMany(u => u.ProjectMembers)
                .HasForeignKey(pm => pm.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectMember>()
                .HasOne(pm => pm.Project)
                .WithMany(p => p.ProjectMembers)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
