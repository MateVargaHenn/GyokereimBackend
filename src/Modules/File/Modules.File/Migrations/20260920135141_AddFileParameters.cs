using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.File.Migrations;

/// <inheritdoc />
public partial class AddFileParameters : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "ModifiedBy",
            schema: "file",
            table: "Files",
            type: "character varying(100)",
            maxLength: 100,
            nullable: false,
            defaultValue: "System",
            oldClrType: typeof(string),
            oldType: "text");

        migrationBuilder.AlterColumn<DateTime>(
            name: "LastModified",
            schema: "file",
            table: "Files",
            type: "timestamp with time zone",
            nullable: false,
            defaultValueSql: "now()",
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone");

        migrationBuilder.DropColumn(
            name: "Content",
            schema: "file",
            table: "Files");

        migrationBuilder.AddColumn<string>(
            name: "AltText",
            schema: "file",
            table: "Files",
            type: "text",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsPublic",
            schema: "file",
            table: "Files",
            type: "boolean",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<long>(
            name: "Size",
            schema: "file",
            table: "Files",
            type: "bigint",
            nullable: false,
            defaultValue: 0L);

        migrationBuilder.AddColumn<string>(
            name: "StoragePath",
            schema: "file",
            table: "Files",
            type: "text",
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "Title",
            schema: "file",
            table: "Files",
            type: "text",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Files_Key",
            schema: "file",
            table: "Files",
            column: "Key",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_Files_Key",
            schema: "file",
            table: "Files");

        migrationBuilder.DropColumn(
            name: "AltText",
            schema: "file",
            table: "Files");

        migrationBuilder.DropColumn(
            name: "IsPublic",
            schema: "file",
            table: "Files");

        migrationBuilder.DropColumn(
            name: "Size",
            schema: "file",
            table: "Files");

        migrationBuilder.DropColumn(
            name: "StoragePath",
            schema: "file",
            table: "Files");

        migrationBuilder.DropColumn(
            name: "Title",
            schema: "file",
            table: "Files");

        migrationBuilder.AddColumn<byte[]>(
            name: "Content",
            schema: "file",
            table: "Files",
            type: "bytea",
            nullable: false,
            defaultValue: Array.Empty<byte>());

        migrationBuilder.AlterColumn<string>(
            name: "ModifiedBy",
            schema: "file",
            table: "Files",
            type: "text",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "character varying(100)",
            oldMaxLength: 100,
            oldDefaultValue: "System");

        migrationBuilder.AlterColumn<DateTime>(
            name: "LastModified",
            schema: "file",
            table: "Files",
            type: "timestamp with time zone",
            nullable: false,
            oldClrType: typeof(DateTime),
            oldType: "timestamp with time zone",
            oldDefaultValueSql: "now()");
    }
}
