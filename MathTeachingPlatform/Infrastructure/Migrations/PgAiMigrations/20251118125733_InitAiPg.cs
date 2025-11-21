using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.PgAiMigrations
{
    /// <inheritdoc />
    public partial class InitAiPg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ai_configs",
                columns: table => new
                {
                    config_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacher_id = table.Column<int>(type: "integer", nullable: false),
                    config_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    model_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    temperature = table.Column<decimal>(type: "numeric(3,2)", nullable: true),
                    max_tokens = table.Column<int>(type: "integer", nullable: true),
                    config_details = table.Column<string>(type: "text", nullable: true),
                    settings_json = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_configs", x => x.config_id);
                });

            migrationBuilder.CreateTable(
                name: "ai_history_chats",
                columns: table => new
                {
                    chat_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacher_id = table.Column<int>(type: "integer", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    chat_summary = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_history_chats", x => x.chat_id);
                });

            migrationBuilder.CreateTable(
                name: "ai_call_logs",
                columns: table => new
                {
                    log_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    config_id = table.Column<int>(type: "integer", nullable: false),
                    student_id = table.Column<int>(type: "integer", nullable: true),
                    matrix_id = table.Column<int>(type: "integer", nullable: true),
                    service_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    request_text = table.Column<string>(type: "text", nullable: true),
                    request_data = table.Column<string>(type: "text", nullable: true),
                    response_text = table.Column<string>(type: "text", nullable: true),
                    response_data = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_call_logs", x => x.log_id);
                    table.ForeignKey(
                        name: "FK_ai_call_logs_ai_configs_config_id",
                        column: x => x.config_id,
                        principalTable: "ai_configs",
                        principalColumn: "config_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ai_call_logs_config_id",
                table: "ai_call_logs",
                column: "config_id");

            migrationBuilder.CreateIndex(
                name: "IX_ai_call_logs_created_at",
                table: "ai_call_logs",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_ai_call_logs_matrix_id",
                table: "ai_call_logs",
                column: "matrix_id");

            migrationBuilder.CreateIndex(
                name: "IX_ai_call_logs_student_id",
                table: "ai_call_logs",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_ai_configs_is_active",
                table: "ai_configs",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_ai_configs_teacher_id",
                table: "ai_configs",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_ai_configs_teacher_id_is_active",
                table: "ai_configs",
                columns: new[] { "teacher_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_ai_history_chats_created_at",
                table: "ai_history_chats",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_ai_history_chats_teacher_id",
                table: "ai_history_chats",
                column: "teacher_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ai_call_logs");

            migrationBuilder.DropTable(
                name: "ai_history_chats");

            migrationBuilder.DropTable(
                name: "ai_configs");
        }
    }
}
