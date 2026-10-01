using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Service.Migrations;

/// <inheritdoc />
public partial class MapOrderProperties : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "Created",
            schema: "service",
            table: "Orders",
            type: "timestamp with time zone",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<string>(
            name: "CustomerName",
            schema: "service",
            table: "Orders",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "Email",
            schema: "service",
            table: "Orders",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "Message",
            schema: "service",
            table: "Orders",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "PhoneNumber",
            schema: "service",
            table: "Orders",
            type: "text",
            nullable: false,
            defaultValue: "");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Created",
            schema: "service",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "CustomerName",
            schema: "service",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "Email",
            schema: "service",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "Message",
            schema: "service",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "PhoneNumber",
            schema: "service",
            table: "Orders");
    }
}
