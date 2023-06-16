// -----------------------------------------------------------------------
// <copyright file="20230616060743_AddProjectArchitect.cs" company="Christian Ewald">
// Copyright (c) Christian Ewald. All rights reserved.
// Licensed under the MIT license.
// See LICENSE.md in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RolXServer.Migrations;

/// <summary>
/// Add an optional architect to sub-projects.
/// </summary>
/// <seealso cref="Microsoft.EntityFrameworkCore.Migrations.Migration" />
public partial class AddProjectArchitect : Migration
{
    /// <inheritdoc/>
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "ArchitectId",
            table: "Subprojects",
            type: "char(36)",
            nullable: true,
            collation: "ascii_general_ci");

        migrationBuilder.CreateIndex(
            name: "IX_Subprojects_ArchitectId",
            table: "Subprojects",
            column: "ArchitectId");

        migrationBuilder.AddForeignKey(
            name: "FK_Subprojects_Users_ArchitectId",
            table: "Subprojects",
            column: "ArchitectId",
            principalTable: "Users",
            principalColumn: "Id");
    }

    /// <inheritdoc/>
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Subprojects_Users_ArchitectId",
            table: "Subprojects");

        migrationBuilder.DropIndex(
            name: "IX_Subprojects_ArchitectId",
            table: "Subprojects");

        migrationBuilder.DropColumn(
            name: "ArchitectId",
            table: "Subprojects");
    }
}
