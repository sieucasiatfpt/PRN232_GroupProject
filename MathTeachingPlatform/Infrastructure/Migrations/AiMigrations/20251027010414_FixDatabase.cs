using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.AiMigrations
{
    /// <inheritdoc />
    public partial class FixDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ai_call_log_ai_config_ConfigId",
                table: "ai_call_log");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ai_history_chat",
                table: "ai_history_chat");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ai_config",
                table: "ai_config");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ai_call_log",
                table: "ai_call_log");

            migrationBuilder.RenameTable(
                name: "ai_history_chat",
                newName: "ai_history_chats");

            migrationBuilder.RenameTable(
                name: "ai_config",
                newName: "ai_configs");

            migrationBuilder.RenameTable(
                name: "ai_call_log",
                newName: "ai_call_logs");

            migrationBuilder.RenameColumn(
                name: "Message",
                table: "ai_history_chats",
                newName: "message");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "ai_history_chats",
                newName: "teacher_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ai_history_chats",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "ai_history_chats",
                newName: "chat_id");

            migrationBuilder.RenameColumn(
                name: "Temperature",
                table: "ai_configs",
                newName: "temperature");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "ai_configs",
                newName: "teacher_id");

            migrationBuilder.RenameColumn(
                name: "SettingsJson",
                table: "ai_configs",
                newName: "settings_json");

            migrationBuilder.RenameColumn(
                name: "ModelName",
                table: "ai_configs",
                newName: "model_name");

            migrationBuilder.RenameColumn(
                name: "MaxTokens",
                table: "ai_configs",
                newName: "max_tokens");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ai_configs",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ConfigId",
                table: "ai_configs",
                newName: "config_id");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "ai_call_logs",
                newName: "student_id");

            migrationBuilder.RenameColumn(
                name: "ResponseText",
                table: "ai_call_logs",
                newName: "response_text");

            migrationBuilder.RenameColumn(
                name: "RequestText",
                table: "ai_call_logs",
                newName: "request_text");

            migrationBuilder.RenameColumn(
                name: "MatrixId",
                table: "ai_call_logs",
                newName: "matrix_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "ai_call_logs",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ConfigId",
                table: "ai_call_logs",
                newName: "config_id");

            migrationBuilder.RenameColumn(
                name: "LogId",
                table: "ai_call_logs",
                newName: "log_id");

            migrationBuilder.RenameIndex(
                name: "IX_ai_call_log_ConfigId",
                table: "ai_call_logs",
                newName: "IX_ai_call_logs_config_id");

            migrationBuilder.AlterColumn<string>(
                name: "message",
                table: "ai_history_chats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "chat_summary",
                table: "ai_history_chats",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "config_details",
                table: "ai_configs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "config_name",
                table: "ai_configs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "ai_configs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "request_data",
                table: "ai_call_logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "response_data",
                table: "ai_call_logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "service_name",
                table: "ai_call_logs",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ai_history_chats",
                table: "ai_history_chats",
                column: "chat_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ai_configs",
                table: "ai_configs",
                column: "config_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ai_call_logs",
                table: "ai_call_logs",
                column: "log_id");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    role = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    student_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    enrollment_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    major = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.student_id);
                    table.ForeignKey(
                        name: "FK_students_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teachers",
                columns: table => new
                {
                    teacher_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    hire_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teachers", x => x.teacher_id);
                    table.ForeignKey(
                        name: "FK_teachers_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    status = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_attempts", x => x.attempt_id);
                    table.ForeignKey(
                        name: "FK_exam_attempts_students_student_id",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    payment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    teacher_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    payment_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    payment_method = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.payment_id);
                    table.ForeignKey(
                        name: "FK_payments_teachers_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teachers",
                        principalColumn: "teacher_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    subject_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    teacher_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects", x => x.subject_id);
                    table.ForeignKey(
                        name: "FK_subjects_teachers_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teachers",
                        principalColumn: "teacher_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "classes",
                columns: table => new
                {
                    class_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    teacher_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    schedule = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    start_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    end_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classes", x => x.class_id);
                    table.ForeignKey(
                        name: "FK_classes_subjects_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_classes_teachers_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teachers",
                        principalColumn: "teacher_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exam_matrices",
                columns: table => new
                {
                    matrix_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    difficulty_distribution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    total_questions = table.Column<int>(type: "int", nullable: false),
                    generated_on = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_matrices", x => x.matrix_id);
                    table.ForeignKey(
                        name: "FK_exam_matrices_subjects_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "syllabi",
                columns: table => new
                {
                    syllabus_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    teacher_id = table.Column<int>(type: "int", nullable: false),
                    subject_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_syllabi", x => x.syllabus_id);
                    table.ForeignKey(
                        name: "FK_syllabi_subjects_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_syllabi_teachers_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teachers",
                        principalColumn: "teacher_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "activities",
                columns: table => new
                {
                    activity_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    class_id = table.Column<int>(type: "int", nullable: false),
                    exam_attempt_id = table.Column<int>(type: "int", nullable: true),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    activity_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    due_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activities", x => x.activity_id);
                    table.ForeignKey(
                        name: "FK_activities_classes_class_id",
                        column: x => x.class_id,
                        principalTable: "classes",
                        principalColumn: "class_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_activities_exam_attempts_exam_attempt_id",
                        column: x => x.exam_attempt_id,
                        principalTable: "exam_attempts",
                        principalColumn: "attempt_id");
                });

            migrationBuilder.CreateTable(
                name: "class_students",
                columns: table => new
                {
                    class_id = table.Column<int>(type: "int", nullable: false),
                    student_id = table.Column<int>(type: "int", nullable: false),
                    enrollment_status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    enrolled_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_class_students", x => new { x.class_id, x.student_id });
                    table.ForeignKey(
                        name: "FK_class_students_classes_class_id",
                        column: x => x.class_id,
                        principalTable: "classes",
                        principalColumn: "class_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_class_students_students_student_id",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "student_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "exam_questions",
                columns: table => new
                {
                    question_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    syllabus_id = table.Column<int>(type: "int", nullable: true),
                    matrix_id = table.Column<int>(type: "int", nullable: true),
                    question_text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    question_type = table.Column<int>(type: "int", nullable: false),
                    options_json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    answers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    marks = table.Column<int>(type: "int", nullable: true),
                    points = table.Column<decimal>(type: "decimal(5,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_questions", x => x.question_id);
                    table.ForeignKey(
                        name: "FK_exam_questions_exam_matrices_matrix_id",
                        column: x => x.matrix_id,
                        principalTable: "exam_matrices",
                        principalColumn: "matrix_id");
                    table.ForeignKey(
                        name: "FK_exam_questions_syllabi_syllabus_id",
                        column: x => x.syllabus_id,
                        principalTable: "syllabi",
                        principalColumn: "syllabus_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ai_history_chats_created_at",
                table: "ai_history_chats",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "IX_ai_history_chats_teacher_id",
                table: "ai_history_chats",
                column: "teacher_id");

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
                name: "IX_activities_class_id",
                table: "activities",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_activities_exam_attempt_id",
                table: "activities",
                column: "exam_attempt_id");

            migrationBuilder.CreateIndex(
                name: "IX_class_students_student_id",
                table: "class_students",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_subject_id",
                table: "classes",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_teacher_id",
                table: "classes",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempts_student_id",
                table: "exam_attempts",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_matrices_subject_id",
                table: "exam_matrices",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_questions_matrix_id",
                table: "exam_questions",
                column: "matrix_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_questions_syllabus_id",
                table: "exam_questions",
                column: "syllabus_id");

            migrationBuilder.CreateIndex(
                name: "IX_payments_teacher_id",
                table: "payments",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_students_user_id",
                table: "students",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_subjects_teacher_id",
                table: "subjects",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_syllabi_subject_id",
                table: "syllabi",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_syllabi_teacher_id",
                table: "syllabi",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_teachers_user_id",
                table: "teachers",
                column: "user_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ai_call_logs_ai_configs_config_id",
                table: "ai_call_logs",
                column: "config_id",
                principalTable: "ai_configs",
                principalColumn: "config_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ai_call_logs_exam_matrices_matrix_id",
                table: "ai_call_logs",
                column: "matrix_id",
                principalTable: "exam_matrices",
                principalColumn: "matrix_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ai_call_logs_students_student_id",
                table: "ai_call_logs",
                column: "student_id",
                principalTable: "students",
                principalColumn: "student_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ai_configs_teachers_teacher_id",
                table: "ai_configs",
                column: "teacher_id",
                principalTable: "teachers",
                principalColumn: "teacher_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ai_history_chats_teachers_teacher_id",
                table: "ai_history_chats",
                column: "teacher_id",
                principalTable: "teachers",
                principalColumn: "teacher_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ai_call_logs_ai_configs_config_id",
                table: "ai_call_logs");

            migrationBuilder.DropForeignKey(
                name: "FK_ai_call_logs_exam_matrices_matrix_id",
                table: "ai_call_logs");

            migrationBuilder.DropForeignKey(
                name: "FK_ai_call_logs_students_student_id",
                table: "ai_call_logs");

            migrationBuilder.DropForeignKey(
                name: "FK_ai_configs_teachers_teacher_id",
                table: "ai_configs");

            migrationBuilder.DropForeignKey(
                name: "FK_ai_history_chats_teachers_teacher_id",
                table: "ai_history_chats");

            migrationBuilder.DropTable(
                name: "activities");

            migrationBuilder.DropTable(
                name: "class_students");

            migrationBuilder.DropTable(
                name: "exam_questions");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "exam_attempts");

            migrationBuilder.DropTable(
                name: "classes");

            migrationBuilder.DropTable(
                name: "exam_matrices");

            migrationBuilder.DropTable(
                name: "syllabi");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropTable(
                name: "teachers");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ai_history_chats",
                table: "ai_history_chats");

            migrationBuilder.DropIndex(
                name: "IX_ai_history_chats_created_at",
                table: "ai_history_chats");

            migrationBuilder.DropIndex(
                name: "IX_ai_history_chats_teacher_id",
                table: "ai_history_chats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ai_configs",
                table: "ai_configs");

            migrationBuilder.DropIndex(
                name: "IX_ai_configs_is_active",
                table: "ai_configs");

            migrationBuilder.DropIndex(
                name: "IX_ai_configs_teacher_id",
                table: "ai_configs");

            migrationBuilder.DropIndex(
                name: "IX_ai_configs_teacher_id_is_active",
                table: "ai_configs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ai_call_logs",
                table: "ai_call_logs");

            migrationBuilder.DropIndex(
                name: "IX_ai_call_logs_created_at",
                table: "ai_call_logs");

            migrationBuilder.DropIndex(
                name: "IX_ai_call_logs_matrix_id",
                table: "ai_call_logs");

            migrationBuilder.DropIndex(
                name: "IX_ai_call_logs_student_id",
                table: "ai_call_logs");

            migrationBuilder.DropColumn(
                name: "chat_summary",
                table: "ai_history_chats");

            migrationBuilder.DropColumn(
                name: "config_details",
                table: "ai_configs");

            migrationBuilder.DropColumn(
                name: "config_name",
                table: "ai_configs");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "ai_configs");

            migrationBuilder.DropColumn(
                name: "request_data",
                table: "ai_call_logs");

            migrationBuilder.DropColumn(
                name: "response_data",
                table: "ai_call_logs");

            migrationBuilder.DropColumn(
                name: "service_name",
                table: "ai_call_logs");

            migrationBuilder.RenameTable(
                name: "ai_history_chats",
                newName: "ai_history_chat");

            migrationBuilder.RenameTable(
                name: "ai_configs",
                newName: "ai_config");

            migrationBuilder.RenameTable(
                name: "ai_call_logs",
                newName: "ai_call_log");

            migrationBuilder.RenameColumn(
                name: "message",
                table: "ai_history_chat",
                newName: "Message");

            migrationBuilder.RenameColumn(
                name: "teacher_id",
                table: "ai_history_chat",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ai_history_chat",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "chat_id",
                table: "ai_history_chat",
                newName: "ChatId");

            migrationBuilder.RenameColumn(
                name: "temperature",
                table: "ai_config",
                newName: "Temperature");

            migrationBuilder.RenameColumn(
                name: "teacher_id",
                table: "ai_config",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "settings_json",
                table: "ai_config",
                newName: "SettingsJson");

            migrationBuilder.RenameColumn(
                name: "model_name",
                table: "ai_config",
                newName: "ModelName");

            migrationBuilder.RenameColumn(
                name: "max_tokens",
                table: "ai_config",
                newName: "MaxTokens");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ai_config",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "config_id",
                table: "ai_config",
                newName: "ConfigId");

            migrationBuilder.RenameColumn(
                name: "student_id",
                table: "ai_call_log",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "response_text",
                table: "ai_call_log",
                newName: "ResponseText");

            migrationBuilder.RenameColumn(
                name: "request_text",
                table: "ai_call_log",
                newName: "RequestText");

            migrationBuilder.RenameColumn(
                name: "matrix_id",
                table: "ai_call_log",
                newName: "MatrixId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ai_call_log",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "config_id",
                table: "ai_call_log",
                newName: "ConfigId");

            migrationBuilder.RenameColumn(
                name: "log_id",
                table: "ai_call_log",
                newName: "LogId");

            migrationBuilder.RenameIndex(
                name: "IX_ai_call_logs_config_id",
                table: "ai_call_log",
                newName: "IX_ai_call_log_ConfigId");

            migrationBuilder.AlterColumn<string>(
                name: "Message",
                table: "ai_history_chat",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ai_history_chat",
                table: "ai_history_chat",
                column: "ChatId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ai_config",
                table: "ai_config",
                column: "ConfigId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ai_call_log",
                table: "ai_call_log",
                column: "LogId");

            migrationBuilder.AddForeignKey(
                name: "FK_ai_call_log_ai_config_ConfigId",
                table: "ai_call_log",
                column: "ConfigId",
                principalTable: "ai_config",
                principalColumn: "ConfigId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
