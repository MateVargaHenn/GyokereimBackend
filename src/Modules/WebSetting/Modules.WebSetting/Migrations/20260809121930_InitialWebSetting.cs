using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Modules.WebSetting.Migrations;

/// <inheritdoc />
public partial class InitialWebSetting : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "websetting");

        migrationBuilder.CreateTable(
            name: "WebSettings",
            schema: "websetting",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Key = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                Value = table.Column<string>(type: "character varying(65000)", maxLength: 65000, nullable: false),
                LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                ModifiedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "System")
            },
            constraints: table => table.PrimaryKey("PK_WebSettings", x => x.Id));
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(
            name: "WebSettings",
            schema: "websetting");
}
