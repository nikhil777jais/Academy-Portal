using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademyPortal.Migrations
{
    public partial class tblbatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Batches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    Technology = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Batch_Start_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Batch_End_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Batch_Capacity = table.Column<int>(type: "int", nullable: true),
                    Classroom_Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    createdBy = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Batches_AspNetUsers_createdBy",
                        column: x => x.createdBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Batches_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Batches_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BatchUser",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BatchId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchUser", x => new { x.BatchId, x.UserId });
                    table.ForeignKey(
                        name: "FK_BatchUser_AllStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "AllStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BatchUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BatchUser_Batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "Batches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Batches_createdBy",
                table: "Batches",
                column: "createdBy");

            migrationBuilder.CreateIndex(
                name: "IX_Batches_ModuleId",
                table: "Batches",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Batches_SkillId",
                table: "Batches",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchUser_StatusId",
                table: "BatchUser",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchUser_UserId",
                table: "BatchUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchUser");

            migrationBuilder.DropTable(
                name: "Batches");
        }
    }
}
