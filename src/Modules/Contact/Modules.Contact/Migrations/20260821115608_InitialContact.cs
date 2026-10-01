using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Modules.Contact.Migrations;

/// <inheritdoc />
public partial class InitialContact : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "contact");

        migrationBuilder.CreateTable(
            name: "ContactMessages",
            schema: "contact",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                Email = table.Column<string>(type: "character varying(320)", maxLength: 320, nullable: false),
                PhoneNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                Message = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                AttachmentId = table.Column<int>(type: "integer", nullable: true),
                NotificationSent = table.Column<bool>(type: "boolean", nullable: false),
                ReplyToId = table.Column<int>(type: "integer", nullable: true),
                LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                ModifiedBy = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ContactMessages", x => x.Id);
                table.ForeignKey(
                    name: "FK_ContactMessages_ContactMessages_ReplyToId",
                    column: x => x.ReplyToId,
                    principalSchema: "contact",
                    principalTable: "ContactMessages",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_ContactMessages_ReplyToId",
            schema: "contact",
            table: "ContactMessages",
            column: "ReplyToId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(
            name: "ContactMessages",
            schema: "contact");
}
