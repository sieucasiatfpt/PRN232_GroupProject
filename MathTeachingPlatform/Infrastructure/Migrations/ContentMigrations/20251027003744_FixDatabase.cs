using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.ContentMigrations
{
    /// <inheritdoc />
    public partial class FixDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_syllabus",
                table: "syllabus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_exam_matrix",
                table: "exam_matrix");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "activities");

            migrationBuilder.RenameTable(
                name: "syllabus",
                newName: "syllabi");

            migrationBuilder.RenameTable(
                name: "exam_matrix",
                newName: "exam_matrices");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "subjects",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "subjects",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "subjects",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "subjects",
                newName: "teacher_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "subjects",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "subjects",
                newName: "subject_id");

            migrationBuilder.RenameColumn(
                name: "Marks",
                table: "exam_questions",
                newName: "marks");

            migrationBuilder.RenameColumn(
                name: "Answers",
                table: "exam_questions",
                newName: "answers");

            migrationBuilder.RenameColumn(
                name: "SyllabusId",
                table: "exam_questions",
                newName: "syllabus_id");

            migrationBuilder.RenameColumn(
                name: "QuestionType",
                table: "exam_questions",
                newName: "question_type");

            migrationBuilder.RenameColumn(
                name: "QuestionText",
                table: "exam_questions",
                newName: "question_text");

            migrationBuilder.RenameColumn(
                name: "OptionsJson",
                table: "exam_questions",
                newName: "options_json");

            migrationBuilder.RenameColumn(
                name: "MatrixId",
                table: "exam_questions",
                newName: "matrix_id");

            migrationBuilder.RenameColumn(
                name: "QuestionId",
                table: "exam_questions",
                newName: "question_id");

            migrationBuilder.RenameColumn(
                name: "Schedule",
                table: "classes",
                newName: "schedule");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "classes",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "classes",
                newName: "teacher_id");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "classes",
                newName: "subject_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "classes",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "classes",
                newName: "class_id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "activities",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "activities",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "DueDate",
                table: "activities",
                newName: "due_date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "activities",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "activities",
                newName: "class_id");

            migrationBuilder.RenameColumn(
                name: "ActivityId",
                table: "activities",
                newName: "activity_id");

            migrationBuilder.RenameColumn(
                name: "AttemptId",
                table: "activities",
                newName: "exam_attempt_id");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "syllabi",
                newName: "url");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "syllabi",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "syllabi",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "syllabi",
                newName: "teacher_id");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "syllabi",
                newName: "subject_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "syllabi",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "SyllabusId",
                table: "syllabi",
                newName: "syllabus_id");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "exam_matrices",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "TotalQuestions",
                table: "exam_matrices",
                newName: "total_questions");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "exam_matrices",
                newName: "subject_id");

            migrationBuilder.RenameColumn(
                name: "GeneratedOn",
                table: "exam_matrices",
                newName: "generated_on");

            migrationBuilder.RenameColumn(
                name: "MatrixId",
                table: "exam_matrices",
                newName: "matrix_id");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "subjects",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "answers",
                table: "exam_questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "question_text",
                table: "exam_questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "points",
                table: "exam_questions",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "schedule",
                table: "classes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "classes",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "teacher_id",
                table: "classes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "subject_id",
                table: "classes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "end_date",
                table: "classes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "start_date",
                table: "classes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "activities",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "class_id",
                table: "activities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "activity_type",
                table: "activities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "url",
                table: "syllabi",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "syllabi",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "exam_matrices",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "difficulty_distribution",
                table: "exam_matrices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_syllabi",
                table: "syllabi",
                column: "syllabus_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_exam_matrices",
                table: "exam_matrices",
                column: "matrix_id");

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

            migrationBuilder.CreateIndex(
                name: "IX_subjects_teacher_id",
                table: "subjects",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_subjects_title",
                table: "subjects",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "IX_exam_questions_matrix_id",
                table: "exam_questions",
                column: "matrix_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_questions_question_type",
                table: "exam_questions",
                column: "question_type");

            migrationBuilder.CreateIndex(
                name: "IX_exam_questions_syllabus_id",
                table: "exam_questions",
                column: "syllabus_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_start_date",
                table: "classes",
                column: "start_date");

            migrationBuilder.CreateIndex(
                name: "IX_classes_subject_id",
                table: "classes",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_teacher_id",
                table: "classes",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_activities_activity_type",
                table: "activities",
                column: "activity_type");

            migrationBuilder.CreateIndex(
                name: "IX_activities_class_id",
                table: "activities",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_activities_due_date",
                table: "activities",
                column: "due_date");

            migrationBuilder.CreateIndex(
                name: "IX_activities_exam_attempt_id",
                table: "activities",
                column: "exam_attempt_id");

            migrationBuilder.CreateIndex(
                name: "IX_syllabi_subject_id",
                table: "syllabi",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_syllabi_teacher_id",
                table: "syllabi",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_matrices_generated_on",
                table: "exam_matrices",
                column: "generated_on");

            migrationBuilder.CreateIndex(
                name: "IX_exam_matrices_subject_id",
                table: "exam_matrices",
                column: "subject_id");

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
                name: "IX_class_students_enrollment_status",
                table: "class_students",
                column: "enrollment_status");

            migrationBuilder.CreateIndex(
                name: "IX_class_students_student_id",
                table: "class_students",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_exam_attempts_student_id",
                table: "exam_attempts",
                column: "student_id");

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
                name: "IX_teachers_user_id",
                table: "teachers",
                column: "user_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_activities_classes_class_id",
                table: "activities",
                column: "class_id",
                principalTable: "classes",
                principalColumn: "class_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_activities_exam_attempts_exam_attempt_id",
                table: "activities",
                column: "exam_attempt_id",
                principalTable: "exam_attempts",
                principalColumn: "attempt_id");

            migrationBuilder.AddForeignKey(
                name: "FK_classes_subjects_subject_id",
                table: "classes",
                column: "subject_id",
                principalTable: "subjects",
                principalColumn: "subject_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_classes_teachers_teacher_id",
                table: "classes",
                column: "teacher_id",
                principalTable: "teachers",
                principalColumn: "teacher_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_exam_matrices_subjects_subject_id",
                table: "exam_matrices",
                column: "subject_id",
                principalTable: "subjects",
                principalColumn: "subject_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_exam_questions_exam_matrices_matrix_id",
                table: "exam_questions",
                column: "matrix_id",
                principalTable: "exam_matrices",
                principalColumn: "matrix_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_exam_questions_syllabi_syllabus_id",
                table: "exam_questions",
                column: "syllabus_id",
                principalTable: "syllabi",
                principalColumn: "syllabus_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_subjects_teachers_teacher_id",
                table: "subjects",
                column: "teacher_id",
                principalTable: "teachers",
                principalColumn: "teacher_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_syllabi_subjects_subject_id",
                table: "syllabi",
                column: "subject_id",
                principalTable: "subjects",
                principalColumn: "subject_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_syllabi_teachers_teacher_id",
                table: "syllabi",
                column: "teacher_id",
                principalTable: "teachers",
                principalColumn: "teacher_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_activities_classes_class_id",
                table: "activities");

            migrationBuilder.DropForeignKey(
                name: "FK_activities_exam_attempts_exam_attempt_id",
                table: "activities");

            migrationBuilder.DropForeignKey(
                name: "FK_classes_subjects_subject_id",
                table: "classes");

            migrationBuilder.DropForeignKey(
                name: "FK_classes_teachers_teacher_id",
                table: "classes");

            migrationBuilder.DropForeignKey(
                name: "FK_exam_matrices_subjects_subject_id",
                table: "exam_matrices");

            migrationBuilder.DropForeignKey(
                name: "FK_exam_questions_exam_matrices_matrix_id",
                table: "exam_questions");

            migrationBuilder.DropForeignKey(
                name: "FK_exam_questions_syllabi_syllabus_id",
                table: "exam_questions");

            migrationBuilder.DropForeignKey(
                name: "FK_subjects_teachers_teacher_id",
                table: "subjects");

            migrationBuilder.DropForeignKey(
                name: "FK_syllabi_subjects_subject_id",
                table: "syllabi");

            migrationBuilder.DropForeignKey(
                name: "FK_syllabi_teachers_teacher_id",
                table: "syllabi");

            migrationBuilder.DropTable(
                name: "ai_call_logs");

            migrationBuilder.DropTable(
                name: "ai_history_chats");

            migrationBuilder.DropTable(
                name: "class_students");

            migrationBuilder.DropTable(
                name: "exam_attempts");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "ai_configs");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "teachers");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropIndex(
                name: "IX_subjects_teacher_id",
                table: "subjects");

            migrationBuilder.DropIndex(
                name: "IX_subjects_title",
                table: "subjects");

            migrationBuilder.DropIndex(
                name: "IX_exam_questions_matrix_id",
                table: "exam_questions");

            migrationBuilder.DropIndex(
                name: "IX_exam_questions_question_type",
                table: "exam_questions");

            migrationBuilder.DropIndex(
                name: "IX_exam_questions_syllabus_id",
                table: "exam_questions");

            migrationBuilder.DropIndex(
                name: "IX_classes_start_date",
                table: "classes");

            migrationBuilder.DropIndex(
                name: "IX_classes_subject_id",
                table: "classes");

            migrationBuilder.DropIndex(
                name: "IX_classes_teacher_id",
                table: "classes");

            migrationBuilder.DropIndex(
                name: "IX_activities_activity_type",
                table: "activities");

            migrationBuilder.DropIndex(
                name: "IX_activities_class_id",
                table: "activities");

            migrationBuilder.DropIndex(
                name: "IX_activities_due_date",
                table: "activities");

            migrationBuilder.DropIndex(
                name: "IX_activities_exam_attempt_id",
                table: "activities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_syllabi",
                table: "syllabi");

            migrationBuilder.DropIndex(
                name: "IX_syllabi_subject_id",
                table: "syllabi");

            migrationBuilder.DropIndex(
                name: "IX_syllabi_teacher_id",
                table: "syllabi");

            migrationBuilder.DropPrimaryKey(
                name: "PK_exam_matrices",
                table: "exam_matrices");

            migrationBuilder.DropIndex(
                name: "IX_exam_matrices_generated_on",
                table: "exam_matrices");

            migrationBuilder.DropIndex(
                name: "IX_exam_matrices_subject_id",
                table: "exam_matrices");

            migrationBuilder.DropColumn(
                name: "points",
                table: "exam_questions");

            migrationBuilder.DropColumn(
                name: "end_date",
                table: "classes");

            migrationBuilder.DropColumn(
                name: "start_date",
                table: "classes");

            migrationBuilder.DropColumn(
                name: "activity_type",
                table: "activities");

            migrationBuilder.DropColumn(
                name: "difficulty_distribution",
                table: "exam_matrices");

            migrationBuilder.RenameTable(
                name: "syllabi",
                newName: "syllabus");

            migrationBuilder.RenameTable(
                name: "exam_matrices",
                newName: "exam_matrix");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "subjects",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "subjects",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "subjects",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "teacher_id",
                table: "subjects",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "subjects",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "subject_id",
                table: "subjects",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "marks",
                table: "exam_questions",
                newName: "Marks");

            migrationBuilder.RenameColumn(
                name: "answers",
                table: "exam_questions",
                newName: "Answers");

            migrationBuilder.RenameColumn(
                name: "syllabus_id",
                table: "exam_questions",
                newName: "SyllabusId");

            migrationBuilder.RenameColumn(
                name: "question_type",
                table: "exam_questions",
                newName: "QuestionType");

            migrationBuilder.RenameColumn(
                name: "question_text",
                table: "exam_questions",
                newName: "QuestionText");

            migrationBuilder.RenameColumn(
                name: "options_json",
                table: "exam_questions",
                newName: "OptionsJson");

            migrationBuilder.RenameColumn(
                name: "matrix_id",
                table: "exam_questions",
                newName: "MatrixId");

            migrationBuilder.RenameColumn(
                name: "question_id",
                table: "exam_questions",
                newName: "QuestionId");

            migrationBuilder.RenameColumn(
                name: "schedule",
                table: "classes",
                newName: "Schedule");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "classes",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "teacher_id",
                table: "classes",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "subject_id",
                table: "classes",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "classes",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "class_id",
                table: "classes",
                newName: "ClassId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "activities",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "activities",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "due_date",
                table: "activities",
                newName: "DueDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "activities",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "class_id",
                table: "activities",
                newName: "ClassId");

            migrationBuilder.RenameColumn(
                name: "activity_id",
                table: "activities",
                newName: "ActivityId");

            migrationBuilder.RenameColumn(
                name: "exam_attempt_id",
                table: "activities",
                newName: "AttemptId");

            migrationBuilder.RenameColumn(
                name: "url",
                table: "syllabus",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "syllabus",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "syllabus",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "teacher_id",
                table: "syllabus",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "subject_id",
                table: "syllabus",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "syllabus",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "syllabus_id",
                table: "syllabus",
                newName: "SyllabusId");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "exam_matrix",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "total_questions",
                table: "exam_matrix",
                newName: "TotalQuestions");

            migrationBuilder.RenameColumn(
                name: "subject_id",
                table: "exam_matrix",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "generated_on",
                table: "exam_matrix",
                newName: "GeneratedOn");

            migrationBuilder.RenameColumn(
                name: "matrix_id",
                table: "exam_matrix",
                newName: "MatrixId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "subjects",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Answers",
                table: "exam_questions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionText",
                table: "exam_questions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Schedule",
                table: "classes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "classes",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "TeacherId",
                table: "classes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SubjectId",
                table: "classes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "activities",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "activities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "TeacherId",
                table: "activities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "syllabus",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "syllabus",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "exam_matrix",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddPrimaryKey(
                name: "PK_syllabus",
                table: "syllabus",
                column: "SyllabusId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_exam_matrix",
                table: "exam_matrix",
                column: "MatrixId");
        }
    }
}
