using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MintPlayer.AspNetCore.IdentityServer.Provider.Data.Migrations
{
	public partial class AddDefaultUserAndRole : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.InsertData(
				table: "AspNetRoles",
				columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
				values: new object[] { new Guid("0097e37b-1c43-47d6-9665-419aa28cd8be"), "1bb3c759-721f-44ea-9ff3-623a8cec7eab", "Administrator", "Administrator" });

			migrationBuilder.InsertData(
				table: "AspNetUsers",
				columns: new[] { "Id", "AccessFailedCount", "Bypass2faForExternalLogin", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
				values: new object[] { new Guid("d2c53e36-9a0a-42e4-b075-a4cc6481dd15"), 0, false, "f125d4ba-ce9c-473b-a110-3c81e053c17b", "admin@example.com", true, false, null, "ADMIN@EXAMPLE.COM", "admin", "AQAAAAEAACcQAAAAEAxq56sy77JI+Q3N9GPxkCHd0pzLlyfnXY7vh9vueMFPDi/qyg7CUM7YBPLmFTSNUw==", null, false, "", false, "admin" });

			migrationBuilder.InsertData(
				table: "AspNetUserRoles",
				columns: new[] { "RoleId", "UserId" },
				values: new object[] { new Guid("0097e37b-1c43-47d6-9665-419aa28cd8be"), new Guid("d2c53e36-9a0a-42e4-b075-a4cc6481dd15") });
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "AspNetUserRoles",
				keyColumns: new[] { "RoleId", "UserId" },
				keyValues: new object[] { new Guid("0097e37b-1c43-47d6-9665-419aa28cd8be"), new Guid("d2c53e36-9a0a-42e4-b075-a4cc6481dd15") });

			migrationBuilder.DeleteData(
				table: "AspNetRoles",
				keyColumn: "Id",
				keyValue: new Guid("0097e37b-1c43-47d6-9665-419aa28cd8be"));

			migrationBuilder.DeleteData(
				table: "AspNetUsers",
				keyColumn: "Id",
				keyValue: new Guid("d2c53e36-9a0a-42e4-b075-a4cc6481dd15"));
		}
	}
}
