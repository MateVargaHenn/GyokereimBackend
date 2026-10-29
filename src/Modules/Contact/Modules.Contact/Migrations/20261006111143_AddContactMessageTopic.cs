using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Contact.Migrations;

/// <inheritdoc />
public partial class AddContactMessageTopic : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<string>(
            name: "Topic",
            schema: "contact",
            table: "ContactMessages",
            type: "text",
            defaultValue: "general",
            nullable: true);

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(
            name: "Topic",
            schema: "contact",
            table: "ContactMessages");
}
