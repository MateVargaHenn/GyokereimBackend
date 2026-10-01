#nullable disable

namespace Modules.Chat.Migrations;

/// <inheritdoc />
public partial class InitialChat : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "chat");

        migrationBuilder.CreateTable(
            name: "Intents",
            schema: "chat",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Key = table.Column<string>(type: "text", nullable: false),
                Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                ModifiedBy = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Intents", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Answers",
            schema: "chat",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                IntentId = table.Column<int>(type: "integer", nullable: false),
                LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                ModifiedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "System")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Answers", x => x.Id);
                table.ForeignKey(
                    name: "FK_Answers_Intents_IntentId",
                    column: x => x.IntentId,
                    principalSchema: "chat",
                    principalTable: "Intents",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Utterances",
            schema: "chat",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                IntentId = table.Column<int>(type: "integer", nullable: false),
                LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                ModifiedBy = table.Column<string>(type: "text", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Utterances", x => x.Id);
                table.ForeignKey(
                    name: "FK_Utterances_Intents_IntentId",
                    column: x => x.IntentId,
                    principalSchema: "chat",
                    principalTable: "Intents",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Answers_IntentId",
            schema: "chat",
            table: "Answers",
            column: "IntentId");

        migrationBuilder.CreateIndex(
            name: "IX_Utterances_IntentId",
            schema: "chat",
            table: "Utterances",
            column: "IntentId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Answers",
            schema: "chat");

        migrationBuilder.DropTable(
            name: "Utterances",
            schema: "chat");

        migrationBuilder.DropTable(
            name: "Intents",
            schema: "chat");
    }
}
