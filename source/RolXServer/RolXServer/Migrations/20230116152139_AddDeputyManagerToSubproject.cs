// -----------------------------------------------------------------------
// <copyright file="20230116152139_AddDeputyManagerToSubproject.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RolXServer.Migrations
{
    /// <summary>
    /// Adds deputy manager to the subproject model.
    /// </summary>
    public partial class AddDeputyManagerToSubproject : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DeputyManagerId",
                table: "Subprojects",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Subprojects_DeputyManagerId",
                table: "Subprojects",
                column: "DeputyManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subprojects_Users_DeputyManagerId",
                table: "Subprojects",
                column: "DeputyManagerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subprojects_Users_DeputyManagerId",
                table: "Subprojects");

            migrationBuilder.DropIndex(
                name: "IX_Subprojects_DeputyManagerId",
                table: "Subprojects");

            migrationBuilder.DropColumn(
                name: "DeputyManagerId",
                table: "Subprojects");
        }
    }
}
