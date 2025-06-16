using GizmoGrid._01.Entity;
using GizmoGrid._01.Entity.Api_sEntity;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure unique emails
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Project belongs to one user, user can have many projects
            modelBuilder.Entity<Project>()
                .HasOne(p => p.User)
                .WithMany(u => u.Projects)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade); // this is okay

            // FlowDiagram belongs to a Project
            modelBuilder.Entity<FlowDiagram>()
                .HasOne(fd => fd.Project)
                .WithMany(p => p.FlowDiagrams)
                .HasForeignKey(fd => fd.ProjectId)
                .OnDelete(DeleteBehavior.Cascade); // keep this as cascade

            // FlowDiagram belongs to a User — FIX IS HERE!
            modelBuilder.Entity<FlowDiagram>()
                .HasOne(fd => fd.User)
                .WithMany()
                .HasForeignKey(fd => fd.UserId)
                .OnDelete(DeleteBehavior.Restrict); // restrict this — NO CASCADE

            // Node belongs to a FlowDiagram
            modelBuilder.Entity<Node>()
                .HasOne(n => n.FlowDiagram)
                .WithMany(fd => fd.Nodes)
                .HasForeignKey(n => n.FlowDiagramId);

            modelBuilder.Entity<Edge>()
                .HasOne(e => e.SourceNode)
                .WithMany(n => n.OutgoingEdges)
                .HasForeignKey(e => e.SourceId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Edge>(entity =>
            {
                entity.Property(e => e.SourceHandle).IsRequired();
                entity.Property(e => e.TargetHandle).IsRequired();
            });

            modelBuilder.Entity<Edge>()
                .HasOne(e => e.TargetNode)
                .WithMany(n => n.IncomingEdges)
                .HasForeignKey(e => e.TargetId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ NEW: Edge → FlowDiagram
            modelBuilder.Entity<Edge>()
                .HasOne(e => e.FlowDiagram)
                .WithMany(fd => fd.Edges)
                .HasForeignKey(e => e.FlowDiagramId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SchemaDiagram>()
    //.HasMany<TableNode>()
    .HasMany(t=>t.TableNodes)
    .WithOne(t => t.SchemaDiagram)
    .HasForeignKey(t => t.SchemaDiagramId)
    .OnDelete(DeleteBehavior.Cascade);

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
    .WithMany() // you can use .WithMany("ReferencedBy") if you want reverse nav
    .HasForeignKey(c => c.ForeignKeyReferenceColumnId)
    .OnDelete(DeleteBehavior.Restrict); // prevent cascade loops




            // ApiDiagram → User
            modelBuilder.Entity<ApiDiagram>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict); // prevent cascade loop if deleting user

            // ApiDiagram → ApiTableNodes (1:N)
            modelBuilder.Entity<ApiDiagram>()
                .HasMany(a => a.ApiTableNodes)
                .WithOne()
                .HasForeignKey("ApiDiagramId") // shadow property
                .OnDelete(DeleteBehavior.Cascade);

            // ApiDiagram → ApiEdges (1:N)
            modelBuilder.Entity<ApiDiagram>()
                .HasMany(a => a.ApiEdges)
                .WithOne(e => e.ApiDiagram)
                .HasForeignKey(e => e.ApiDiagramId)
                .OnDelete(DeleteBehavior.Cascade);

            // ApiTableNodes primary key
            modelBuilder.Entity<ApiTableNodes>()
                .HasKey(n => n.ApiTableNodesId);

            // ApiEdges primary key
            modelBuilder.Entity<ApiEdges>()
                .HasKey(e => e.ApiEdgesId);

            // ApiEdges → SourceNode
            modelBuilder.Entity<ApiEdges>()
        .HasOne(e => e.SourceNode)
        .WithMany(n => n.OutgoingEdges)
        .HasForeignKey(e => e.SourceId)
        .OnDelete(DeleteBehavior.Restrict);


            // ApiEdges → TargetNode
            modelBuilder.Entity<ApiEdges>()
                .HasOne(e => e.TargetNode)
                .WithMany(n => n.IncomingEdges)
                .HasForeignKey(e => e.TargetId)
                .OnDelete(DeleteBehavior.Restrict);



        }

    }
}