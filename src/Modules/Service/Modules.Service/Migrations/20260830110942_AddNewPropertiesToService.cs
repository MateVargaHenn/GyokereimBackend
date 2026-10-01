using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Service.Migrations;

/// <inheritdoc />
public partial class AddNewPropertiesToService : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Orders_OrderStatus_OrderStatusId",
            schema: "service",
            table: "Orders");

        migrationBuilder.DropIndex(
            name: "IX_Orders_OrderStatusId",
            schema: "service",
            table: "Orders");

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
            name: "OrderStatusId",
            schema: "service",
            table: "Orders");

        migrationBuilder.DropColumn(
            name: "PhoneNumber",
            schema: "service",
            table: "Orders");

        migrationBuilder.AlterColumn<decimal>(
            name: "Price",
            schema: "service",
            table: "Services",
            type: "numeric(18,2)",
            precision: 18,
            scale: 2,
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "numeric");

        migrationBuilder.AddColumn<int>(
            name: "ServiceStatusCode",
            schema: "service",
            table: "Services",
            type: "integer",
            nullable: false,
            defaultValue: 0);

        migrationBuilder.CreateTable(
            name: "ServiceStatus",
            schema: "service",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                NormalizedName = table.Column<string>(type: "text", nullable: false, computedColumnSql: "UPPER(\"Name\")", stored: true),
                ConcurrencyStamp = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()")
            },
            constraints: table => table.PrimaryKey("PK_ServiceStatus", x => x.Id));

        migrationBuilder.CreateIndex(
            name: "IX_ServiceStatus_Name",
            schema: "service",
            table: "ServiceStatus",
            column: "Name",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ServiceStatus",
            schema: "service");

        migrationBuilder.DropColumn(
            name: "ServiceStatusCode",
            schema: "service",
            table: "Services");

        migrationBuilder.AlterColumn<decimal>(
            name: "Price",
            schema: "service",
            table: "Services",
            type: "numeric",
            nullable: false,
            oldClrType: typeof(decimal),
            oldType: "numeric(18,2)",
            oldPrecision: 18,
            oldScale: 2);

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

        migrationBuilder.AddColumn<int>(
            name: "OrderStatusId",
            schema: "service",
            table: "Orders",
            type: "integer",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "PhoneNumber",
            schema: "service",
            table: "Orders",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateIndex(
            name: "IX_Orders_OrderStatusId",
            schema: "service",
            table: "Orders",
            column: "OrderStatusId");

        migrationBuilder.AddForeignKey(
            name: "FK_Orders_OrderStatus_OrderStatusId",
            schema: "service",
            table: "Orders",
            column: "OrderStatusId",
            principalSchema: "service",
            principalTable: "OrderStatus",
            principalColumn: "Id");
    }
}
