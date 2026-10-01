using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Service.Migrations;

/// <inheritdoc />
public partial class AddServiceOrderNumber : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.AddColumn<int>(
            name: "OrderNumber",
            schema: "service",
            table: "Services",
            type: "integer",
            nullable: false,
            defaultValue: 0);

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropColumn(
            name: "OrderNumber",
            schema: "service",
            table: "Services");
}
