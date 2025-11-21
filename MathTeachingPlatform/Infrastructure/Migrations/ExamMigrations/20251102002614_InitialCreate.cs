using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.ExamMigrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "exam_attempts",
                columns: table => new
                {
                    attempt_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    student_id = table.Column<int>(type: "int", nullable: false),
                    start_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    end_time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    score = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    attempt_number = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_attempts", x => x.attempt_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempts_start_time",
                table: "exam_attempts",
                column: "start_time");

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempts_status",
                table: "exam_attempts",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempts_student_id",
                table: "exam_attempts",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempts_student_id_attempt_number",
                table: "exam_attempts",
                columns: new[] { "student_id", "attempt_number" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "exam_attempts");
        }
    }
}
