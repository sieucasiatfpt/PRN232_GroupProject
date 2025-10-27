using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.AuthMigrations
{
    /// <inheritdoc />
    public partial class FixDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "students");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Role",
                table: "users",
                newName: "role");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "users",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "users",
                newName: "password_hash");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "users",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "users",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "IX_users_Username",
                table: "users",
                newName: "IX_users_username");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "teachers",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "teachers",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Bio",
                table: "teachers",
                newName: "bio");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "teachers",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "teachers",
                newName: "teacher_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "students",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "students",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "students",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "EnrollmentDate",
                table: "students",
                newName: "enrollment_date");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "students",
                newName: "student_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "payments",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "payments",
                newName: "amount");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "payments",
                newName: "teacher_id");

            migrationBuilder.RenameColumn(
                name: "PaymentDate",
                table: "payments",
                newName: "payment_date");

            migrationBuilder.RenameColumn(
                name: "Method",
                table: "payments",
                newName: "payment_method");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "payments",
                newName: "payment_id");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "teachers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "department",
                table: "teachers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "hire_date",
                table: "teachers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "students",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "major",
                table: "students",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ai_configs",
                columns: table => new
                {
                    config_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    teacher_id = table.Column<int>(type: "int", nullable: false),
                    config_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    model_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    temperature = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    max_tokens = table.Column<int>(type: "int", nullable: true),
                    config_details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    settings_json = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_configs", x => x.config_id);
                    table.ForeignKey(
                        name: "FK_ai_configs_teachers_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teachers",
                        principalColumn: "teacher_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ai_history_chats",
                columns: table => new
                {
                    chat_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    teacher_id = table.Column<int>(type: "int", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    chat_summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_history_chats", x => x.chat_id);
                    table.ForeignKey(
                        name: "FK_ai_history_chats_teachers_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "teachers",
                        principalColumn: "teacher_id",
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
                name: "ai_call_logs",
                columns: table => new
                {
                    log_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    config_id = table.Column<int>(type: "int", nullable: false),
                    student_id = table.Column<int>(type: "int", nullable: true),
                    matrix_id = table.Column<int>(type: "int", nullable: true),
                    service_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    request_text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    request_data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    response_text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    response_data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    table.ForeignKey(
                        name: "FK_ai_call_logs_exam_matrices_matrix_id",
                        column: x => x.matrix_id,
                        principalTable: "exam_matrices",
                        principalColumn: "matrix_id");
                    table.ForeignKey(
                        name: "FK_ai_call_logs_students_student_id",
                        column: x => x.student_id,
                        principalTable: "students",
                        principalColumn: "student_id");
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
                name: "IX_users_email",
                table: "users",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_teachers_status",
                table: "teachers",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_teachers_user_id",
                table: "teachers",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_students_status",
                table: "students",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_students_user_id",
                table: "students",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payments_payment_date",
                table: "payments",
                column: "payment_date");

            migrationBuilder.CreateIndex(
                name: "IX_payments_status",
                table: "payments",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_payments_teacher_id",
                table: "payments",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_activities_class_id",
                table: "activities",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_activities_exam_attempt_id",
                table: "activities",
                column: "exam_attempt_id");

            migrationBuilder.CreateIndex(
                name: "IX_ai_call_logs_config_id",
                table: "ai_call_logs",
                column: "config_id");

            migrationBuilder.CreateIndex(
                name: "IX_ai_call_logs_matrix_id",
                table: "ai_call_logs",
                column: "matrix_id");

            migrationBuilder.CreateIndex(
                name: "IX_ai_call_logs_student_id",
                table: "ai_call_logs",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_ai_configs_teacher_id",
                table: "ai_configs",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_ai_history_chats_teacher_id",
                table: "ai_history_chats",
                column: "teacher_id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_payments_teachers_teacher_id",
                table: "payments",
                column: "teacher_id",
                principalTable: "teachers",
                principalColumn: "teacher_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_students_users_user_id",
                table: "students",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_teachers_users_user_id",
                table: "teachers",
                column: "user_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payments_teachers_teacher_id",
                table: "payments");

            migrationBuilder.DropForeignKey(
                name: "FK_students_users_user_id",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_teachers_users_user_id",
                table: "teachers");

            migrationBuilder.DropTable(
                name: "activities");

            migrationBuilder.DropTable(
                name: "ai_call_logs");

            migrationBuilder.DropTable(
                name: "ai_history_chats");

            migrationBuilder.DropTable(
                name: "class_students");

            migrationBuilder.DropTable(
                name: "exam_questions");

            migrationBuilder.DropTable(
                name: "exam_attempts");

            migrationBuilder.DropTable(
                name: "ai_configs");

            migrationBuilder.DropTable(
                name: "classes");

            migrationBuilder.DropTable(
                name: "exam_matrices");

            migrationBuilder.DropTable(
                name: "syllabi");

            migrationBuilder.DropTable(
                name: "subjects");

            migrationBuilder.DropIndex(
                name: "IX_users_email",
                table: "users");

            migrationBuilder.DropIndex(
                name: "IX_teachers_status",
                table: "teachers");

            migrationBuilder.DropIndex(
                name: "IX_teachers_user_id",
                table: "teachers");

            migrationBuilder.DropIndex(
                name: "IX_students_status",
                table: "students");

            migrationBuilder.DropIndex(
                name: "IX_students_user_id",
                table: "students");

            migrationBuilder.DropIndex(
                name: "IX_payments_payment_date",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "IX_payments_status",
                table: "payments");

            migrationBuilder.DropIndex(
                name: "IX_payments_teacher_id",
                table: "payments");

            migrationBuilder.DropColumn(
                name: "department",
                table: "teachers");

            migrationBuilder.DropColumn(
                name: "hire_date",
                table: "teachers");

            migrationBuilder.DropColumn(
                name: "major",
                table: "students");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "role",
                table: "users",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "users",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "users",
                newName: "PasswordHash");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "users",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_users_username",
                table: "users",
                newName: "IX_users_Username");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "teachers",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "teachers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "bio",
                table: "teachers",
                newName: "Bio");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "teachers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "teacher_id",
                table: "teachers",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "students",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "students",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "students",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "enrollment_date",
                table: "students",
                newName: "EnrollmentDate");

            migrationBuilder.RenameColumn(
                name: "student_id",
                table: "students",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "payments",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "amount",
                table: "payments",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "teacher_id",
                table: "payments",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "payment_method",
                table: "payments",
                newName: "Method");

            migrationBuilder.RenameColumn(
                name: "payment_date",
                table: "payments",
                newName: "PaymentDate");

            migrationBuilder.RenameColumn(
                name: "payment_id",
                table: "payments",
                newName: "PaymentId");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "users",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "teachers",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "students",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "students",
                type: "int",
                nullable: true);
        }
    }
}
