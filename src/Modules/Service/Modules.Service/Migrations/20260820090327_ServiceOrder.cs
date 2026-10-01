using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Modules.Service.Migrations;

/// <inheritdoc />
public partial class ServiceOrder : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.EnsureSchema(
            name: "service");

        migrationBuilder.CreateTable(
            name: "OrderStatus",
            schema: "service",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false),
                Name = table.Column<string>(type: "text", nullable: false),
                NormalizedName = table.Column<string>(type: "text", nullable: false, computedColumnSql: "UPPER(\"Name\")", stored: true),
                ConcurrencyStamp = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()")
            },
            constraints: table => table.PrimaryKey("PK_OrderStatus", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Services",
            schema: "service",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "text", nullable: false),
                ShortDescription = table.Column<string>(type: "text", nullable: false),
                Description = table.Column<string>(type: "text", nullable: false),
                Slug = table.Column<string>(type: "text", nullable: false),
                Price = table.Column<decimal>(type: "numeric", nullable: false),
                LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                ModifiedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "System")
            },
            constraints: table => table.PrimaryKey("PK_Services", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Orders",
            schema: "service",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                CustomerName = table.Column<string>(type: "text", nullable: false),
                Email = table.Column<string>(type: "text", nullable: false),
                PhoneNumber = table.Column<string>(type: "text", nullable: false),
                Message = table.Column<string>(type: "text", nullable: true),
                OrderStatusCode = table.Column<int>(type: "integer", nullable: false),
                ServiceId = table.Column<int>(type: "integer", nullable: false),
                TrackingCode = table.Column<string>(type: "text", nullable: false),
                Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                OrderStatusId = table.Column<int>(type: "integer", nullable: true),
                LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()"),
                ModifiedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false, defaultValue: "System")
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Orders", x => x.Id);
                table.ForeignKey(
                    name: "FK_Orders_OrderStatus_OrderStatusCode",
                    column: x => x.OrderStatusCode,
                    principalSchema: "service",
                    principalTable: "OrderStatus",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_Orders_OrderStatus_OrderStatusId",
                    column: x => x.OrderStatusId,
                    principalSchema: "service",
                    principalTable: "OrderStatus",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Orders_Services_ServiceId",
                    column: x => x.ServiceId,
                    principalSchema: "service",
                    principalTable: "Services",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Orders_OrderStatusCode",
            schema: "service",
            table: "Orders",
            column: "OrderStatusCode");

        migrationBuilder.CreateIndex(
            name: "IX_Orders_OrderStatusId",
            schema: "service",
            table: "Orders",
            column: "OrderStatusId");

        migrationBuilder.CreateIndex(
            name: "IX_Orders_ServiceId",
            schema: "service",
            table: "Orders",
            column: "ServiceId");

        migrationBuilder.CreateIndex(
            name: "IX_Orders_TrackingCode",
            schema: "service",
            table: "Orders",
            column: "TrackingCode",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_OrderStatus_Name",
            schema: "service",
            table: "OrderStatus",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Services_Slug",
            schema: "service",
            table: "Services",
            column: "Slug",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Orders",
            schema: "service");

        migrationBuilder.DropTable(
            name: "OrderStatus",
            schema: "service");

        migrationBuilder.DropTable(
            name: "Services",
            schema: "service");
    }
}
