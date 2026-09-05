using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Notely.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "t_e_compte_com",
                schema: "public",
                columns: table => new
                {
                    com_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    com_email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    com_mdp_hash = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    com_doit_changer_mdp = table.Column<bool>(type: "boolean", nullable: false),
                    com_date_creation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    com_date_derniere_connexion = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_compte_com", x => x.com_id);
                });

            migrationBuilder.CreateTable(
                name: "t_e_cours_cou",
                schema: "public",
                columns: table => new
                {
                    cou_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cou_nom = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    cou_date_creation = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    com_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_cours_cou", x => x.cou_id);
                    table.ForeignKey(
                        name: "FK_t_e_cours_cou_t_e_compte_com_com_id",
                        column: x => x.com_id,
                        principalSchema: "public",
                        principalTable: "t_e_compte_com",
                        principalColumn: "com_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_e_evenement_evt",
                schema: "public",
                columns: table => new
                {
                    evt_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    evt_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    evt_titre = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    evt_couleur = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    evt_date = table.Column<DateOnly>(type: "date", nullable: false),
                    evt_heure_debut = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    evt_heure_fin = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    evt_commentaire = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    com_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_evenement_evt", x => x.evt_id);
                    table.ForeignKey(
                        name: "FK_t_e_evenement_evt_t_e_compte_com_com_id",
                        column: x => x.com_id,
                        principalSchema: "public",
                        principalTable: "t_e_compte_com",
                        principalColumn: "com_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_e_note_not",
                schema: "public",
                columns: table => new
                {
                    not_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    not_texte = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    not_fait = table.Column<bool>(type: "boolean", nullable: false),
                    com_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_note_not", x => x.not_id);
                    table.ForeignKey(
                        name: "FK_t_e_note_not_t_e_compte_com_com_id",
                        column: x => x.com_id,
                        principalSchema: "public",
                        principalTable: "t_e_compte_com",
                        principalColumn: "com_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_e_chapitre_cha",
                schema: "public",
                columns: table => new
                {
                    cha_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cha_libelle = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    cha_etat = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    cha_date = table.Column<DateOnly>(type: "date", nullable: true),
                    cha_difficulte = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    cou_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_chapitre_cha", x => x.cha_id);
                    table.ForeignKey(
                        name: "FK_t_e_chapitre_cha_t_e_cours_cou_cou_id",
                        column: x => x.cou_id,
                        principalSchema: "public",
                        principalTable: "t_e_cours_cou",
                        principalColumn: "cou_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_e_todo_tod",
                schema: "public",
                columns: table => new
                {
                    tod_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tod_nom = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    tod_fait = table.Column<bool>(type: "boolean", nullable: false),
                    tod_date = table.Column<DateOnly>(type: "date", nullable: true),
                    cou_id = table.Column<int>(type: "integer", nullable: false),
                    com_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_todo_tod", x => x.tod_id);
                    table.ForeignKey(
                        name: "FK_t_e_todo_tod_t_e_compte_com_com_id",
                        column: x => x.com_id,
                        principalSchema: "public",
                        principalTable: "t_e_compte_com",
                        principalColumn: "com_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_t_e_todo_tod_t_e_cours_cou_cou_id",
                        column: x => x.cou_id,
                        principalSchema: "public",
                        principalTable: "t_e_cours_cou",
                        principalColumn: "cou_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_e_chapitre_cha_cou_id",
                schema: "public",
                table: "t_e_chapitre_cha",
                column: "cou_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_compte_com_com_email",
                schema: "public",
                table: "t_e_compte_com",
                column: "com_email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_t_e_cours_cou_com_id",
                schema: "public",
                table: "t_e_cours_cou",
                column: "com_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_evenement_evt_com_id",
                schema: "public",
                table: "t_e_evenement_evt",
                column: "com_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_note_not_com_id",
                schema: "public",
                table: "t_e_note_not",
                column: "com_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_todo_tod_com_id",
                schema: "public",
                table: "t_e_todo_tod",
                column: "com_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_todo_tod_cou_id",
                schema: "public",
                table: "t_e_todo_tod",
                column: "cou_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_e_chapitre_cha",
                schema: "public");

            migrationBuilder.DropTable(
                name: "t_e_evenement_evt",
                schema: "public");

            migrationBuilder.DropTable(
                name: "t_e_note_not",
                schema: "public");

            migrationBuilder.DropTable(
                name: "t_e_todo_tod",
                schema: "public");

            migrationBuilder.DropTable(
                name: "t_e_cours_cou",
                schema: "public");

            migrationBuilder.DropTable(
                name: "t_e_compte_com",
                schema: "public");
        }
    }
}
