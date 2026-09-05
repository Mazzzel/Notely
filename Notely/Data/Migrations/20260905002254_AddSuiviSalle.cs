using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Notely.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSuiviSalle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_e_seance_sea",
                schema: "public",
                columns: table => new
                {
                    sea_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sea_date = table.Column<DateOnly>(type: "date", nullable: false),
                    sea_commentaire = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    com_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_seance_sea", x => x.sea_id);
                    table.ForeignKey(
                        name: "FK_t_e_seance_sea_t_e_compte_com_com_id",
                        column: x => x.com_id,
                        principalSchema: "public",
                        principalTable: "t_e_compte_com",
                        principalColumn: "com_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_e_exercice_seance_exs",
                schema: "public",
                columns: table => new
                {
                    exs_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    exs_nom = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    sea_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_exercice_seance_exs", x => x.exs_id);
                    table.ForeignKey(
                        name: "FK_t_e_exercice_seance_exs_t_e_seance_sea_sea_id",
                        column: x => x.sea_id,
                        principalSchema: "public",
                        principalTable: "t_e_seance_sea",
                        principalColumn: "sea_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "t_e_serie_ser",
                schema: "public",
                columns: table => new
                {
                    ser_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ser_numero = table.Column<int>(type: "integer", nullable: false),
                    ser_reps = table.Column<int>(type: "integer", nullable: false),
                    ser_poids = table.Column<decimal>(type: "numeric(6,2)", nullable: true),
                    exs_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_serie_ser", x => x.ser_id);
                    table.ForeignKey(
                        name: "FK_t_e_serie_ser_t_e_exercice_seance_exs_exs_id",
                        column: x => x.exs_id,
                        principalSchema: "public",
                        principalTable: "t_e_exercice_seance_exs",
                        principalColumn: "exs_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_e_exercice_seance_exs_sea_id",
                schema: "public",
                table: "t_e_exercice_seance_exs",
                column: "sea_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_seance_sea_com_id",
                schema: "public",
                table: "t_e_seance_sea",
                column: "com_id");

            migrationBuilder.CreateIndex(
                name: "IX_t_e_serie_ser_exs_id",
                schema: "public",
                table: "t_e_serie_ser",
                column: "exs_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_e_serie_ser",
                schema: "public");

            migrationBuilder.DropTable(
                name: "t_e_exercice_seance_exs",
                schema: "public");

            migrationBuilder.DropTable(
                name: "t_e_seance_sea",
                schema: "public");
        }
    }
}
