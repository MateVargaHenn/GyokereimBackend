using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Modules.Blog.Migrations;

/// <inheritdoc />
public partial class RemovePostLikes : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(
            name: "PostLikes",
            schema: "blog");

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "PostLikes",
            schema: "blog",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                PostId = table.Column<int>(type: "integer", nullable: false),
                AnonymId = table.Column<Guid>(type: "uuid", nullable: false),
                LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                ModifiedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "System")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PostLikes", x => x.Id);
                table.ForeignKey(
                    name: "FK_PostLikes_Posts_PostId",
                    column: x => x.PostId,
                    principalSchema: "blog",
                    principalTable: "Posts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_PostLikes_PostId_AnonymId",
            schema: "blog",
            table: "PostLikes",
            columns: new[] { "PostId", "AnonymId" },
            unique: true);
    }
}
