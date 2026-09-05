using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Notely.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminAndPageAccess : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "com_est_admin",
                schema: "public",
                table: "t_e_compte_com",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "t_e_compte_page_cpa",
                schema: "public",
                columns: table => new
                {
                    cpa_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cpa_code_page = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    com_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_e_compte_page_cpa", x => x.cpa_id);
                    table.ForeignKey(
                        name: "FK_t_e_compte_page_cpa_t_e_compte_com_com_id",
                        column: x => x.com_id,
                        principalSchema: "public",
                        principalTable: "t_e_compte_com",
                        principalColumn: "com_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_t_e_compte_page_cpa_com_id_cpa_code_page",
                schema: "public",
                table: "t_e_compte_page_cpa",
                columns: new[] { "com_id", "cpa_code_page" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_e_compte_page_cpa",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "com_est_admin",
                schema: "public",
                table: "t_e_compte_com");
        }
    }
}
