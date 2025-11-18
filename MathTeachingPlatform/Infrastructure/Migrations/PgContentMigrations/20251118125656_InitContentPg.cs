using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.PgContentMigrations
{
    /// <inheritdoc />
    public partial class InitContentPg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "subjects",
                columns: table => new
                {
                    subject_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacher_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subjects", x => x.subject_id);
                });

            migrationBuilder.CreateTable(
                name: "classes",
                columns: table => new
                {
                    class_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subject_id = table.Column<int>(type: "integer", nullable: false),
                    teacher_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    schedule = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classes", x => x.class_id);
                    table.ForeignKey(
                        name: "FK_classes_subjects_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "exam_matrices",
                columns: table => new
                {
                    matrix_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    subject_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    difficulty_distribution = table.Column<string>(type: "text", nullable: true),
                    total_questions = table.Column<int>(type: "integer", nullable: false),
                    generated_on = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_matrices", x => x.matrix_id);
                    table.ForeignKey(
                        name: "FK_exam_matrices_subjects_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "syllabi",
                columns: table => new
                {
                    syllabus_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    teacher_id = table.Column<int>(type: "integer", nullable: false),
                    subject_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    url = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_syllabi", x => x.syllabus_id);
                    table.ForeignKey(
                        name: "FK_syllabi_subjects_subject_id",
                        column: x => x.subject_id,
                        principalTable: "subjects",
                        principalColumn: "subject_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "activities",
                columns: table => new
                {
                    activity_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    class_id = table.Column<int>(type: "integer", nullable: false),
                    exam_attempt_id = table.Column<int>(type: "integer", nullable: true),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    activity_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "class_students",
                columns: table => new
                {
                    class_id = table.Column<int>(type: "integer", nullable: false),
                    student_id = table.Column<int>(type: "integer", nullable: false),
                    enrollment_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    enrolled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "exam_questions",
                columns: table => new
                {
                    question_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    syllabus_id = table.Column<int>(type: "integer", nullable: true),
                    matrix_id = table.Column<int>(type: "integer", nullable: true),
                    question_text = table.Column<string>(type: "text", nullable: false),
                    question_type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    options_json = table.Column<string>(type: "text", nullable: true),
                    answers = table.Column<string>(type: "text", nullable: false),
                    marks = table.Column<int>(type: "integer", nullable: true),
                    points = table.Column<decimal>(type: "numeric(5,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exam_questions", x => x.question_id);
                    table.ForeignKey(
                        name: "FK_exam_questions_exam_matrices_matrix_id",
                        column: x => x.matrix_id,
                        principalTable: "exam_matrices",
                        principalColumn: "matrix_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_exam_questions_syllabi_syllabus_id",
                        column: x => x.syllabus_id,
                        principalTable: "syllabi",
                        principalColumn: "syllabus_id",
                        onDelete: ReferentialAction.SetNull);
                });

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
                name: "IX_class_students_enrollment_status",
                table: "class_students",
                column: "enrollment_status");

            migrationBuilder.CreateIndex(
                name: "IX_class_students_student_id",
                table: "class_students",
                column: "student_id");

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
                name: "IX_exam_matrices_generated_on",
                table: "exam_matrices",
                column: "generated_on");

            migrationBuilder.CreateIndex(
                name: "IX_exam_matrices_subject_id",
                table: "exam_matrices",
                column: "subject_id");

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
                name: "IX_subjects_teacher_id",
                table: "subjects",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_subjects_title",
                table: "subjects",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "IX_syllabi_subject_id",
                table: "syllabi",
                column: "subject_id");

            migrationBuilder.CreateIndex(
                name: "IX_syllabi_teacher_id",
                table: "syllabi",
                column: "teacher_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "activities");

            migrationBuilder.DropTable(
                name: "class_students");

            migrationBuilder.DropTable(
                name: "exam_questions");

            migrationBuilder.DropTable(
                name: "classes");

            migrationBuilder.DropTable(
                name: "exam_matrices");

            migrationBuilder.DropTable(
                name: "syllabi");

            migrationBuilder.DropTable(
                name: "subjects");
        }
    }
}
