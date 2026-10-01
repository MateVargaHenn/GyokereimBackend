using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Chat.Migrations;

/// <inheritdoc />
public partial class FixChatMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.CreateIndex(
            name: "IX_Intents_Key",
            schema: "chat",
            table: "Intents",
            column: "Key",
            unique: true);

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropIndex(
            name: "IX_Intents_Key",
            schema: "chat",
            table: "Intents");
}
