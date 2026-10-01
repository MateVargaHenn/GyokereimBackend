using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Modules.Blog.Migrations;

/// <inheritdoc />
public partial class InitialBlog : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "blog");

        migrationBuilder.CreateTable(
            name: "Posts",
            schema: "blog",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                AuthorId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                ParentId = table.Column<int>(type: "integer", nullable: true),
                Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                MetaTitle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Slug = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                Summary = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                PostStatus = table.Column<int>(type: "integer", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                PublishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                Content = table.Column<string>(type: "text", nullable: false),
                LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                ModifiedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "System")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Posts", x => x.Id);
                table.ForeignKey(
                    name: "FK_Posts_Posts_ParentId",
                    column: x => x.ParentId,
                    principalSchema: "blog",
                    principalTable: "Posts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateTable(
            name: "Tags",
            schema: "blog",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "text", nullable: false),
                Slug = table.Column<string>(type: "text", nullable: false),
                LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                ModifiedBy = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Tags", x => x.Id));

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

        migrationBuilder.CreateTable(
            name: "PostMeta",
            schema: "blog",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                PostId = table.Column<int>(type: "integer", nullable: false),
                Key = table.Column<string>(type: "text", nullable: false),
                Value = table.Column<string>(type: "text", nullable: false),
                LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                ModifiedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "System")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PostMeta", x => x.Id);
                table.ForeignKey(
                    name: "FK_PostMeta_Posts_PostId",
                    column: x => x.PostId,
                    principalSchema: "blog",
                    principalTable: "Posts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "PostTags",
            schema: "blog",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                PostId = table.Column<int>(type: "integer", nullable: false),
                TagId = table.Column<int>(type: "integer", nullable: false),
                LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                ModifiedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "System")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PostTags", x => x.Id);
                table.ForeignKey(
                    name: "FK_PostTags_Posts_PostId",
                    column: x => x.PostId,
                    principalSchema: "blog",
                    principalTable: "Posts",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_PostTags_Tags_TagId",
                    column: x => x.TagId,
                    principalSchema: "blog",
                    principalTable: "Tags",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_PostLikes_PostId_AnonymId",
            schema: "blog",
            table: "PostLikes",
            columns: new[] { "PostId", "AnonymId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_PostMeta_PostId_Key",
            schema: "blog",
            table: "PostMeta",
            columns: new[] { "PostId", "Key" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Posts_AuthorId",
            schema: "blog",
            table: "Posts",
            column: "AuthorId");

        migrationBuilder.CreateIndex(
            name: "IX_Posts_ParentId",
            schema: "blog",
            table: "Posts",
            column: "ParentId");

        migrationBuilder.CreateIndex(
            name: "IX_Posts_PostStatus",
            schema: "blog",
            table: "Posts",
            column: "PostStatus");

        migrationBuilder.CreateIndex(
            name: "IX_Posts_PublishedAt",
            schema: "blog",
            table: "Posts",
            column: "PublishedAt");

        migrationBuilder.CreateIndex(
            name: "IX_Posts_Slug",
            schema: "blog",
            table: "Posts",
            column: "Slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_PostTags_PostId_TagId",
            schema: "blog",
            table: "PostTags",
            columns: new[] { "PostId", "TagId" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_PostTags_TagId",
            schema: "blog",
            table: "PostTags",
            column: "TagId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PostLikes",
            schema: "blog");

        migrationBuilder.DropTable(
            name: "PostMeta",
            schema: "blog");

        migrationBuilder.DropTable(
            name: "PostTags",
            schema: "blog");

        migrationBuilder.DropTable(
            name: "Posts",
            schema: "blog");

        migrationBuilder.DropTable(
            name: "Tags",
            schema: "blog");
    }
}
