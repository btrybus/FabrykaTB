using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FabrykaTB.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fabryka1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hale",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Adres = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hale", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operatorzy",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwisko = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Imie = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Placa = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operatorzy", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Maszyny",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DataUruchomienia = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HalaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maszyny", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maszyny_Hale_HalaId",
                        column: x => x.HalaId,
                        principalTable: "Hale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaszynaOperator",
                columns: table => new
                {
                    MaszynyId = table.Column<int>(type: "int", nullable: false),
                    OperatorzyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaszynaOperator", x => new { x.MaszynyId, x.OperatorzyId });
                    table.ForeignKey(
                        name: "FK_MaszynaOperator_Maszyny_MaszynyId",
                        column: x => x.MaszynyId,
                        principalTable: "Maszyny",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaszynaOperator_Operatorzy_OperatorzyId",
                        column: x => x.OperatorzyId,
                        principalTable: "Operatorzy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaszynaOperator_OperatorzyId",
                table: "MaszynaOperator",
                column: "OperatorzyId");

            migrationBuilder.CreateIndex(
                name: "IX_Maszyny_HalaId",
                table: "Maszyny",
                column: "HalaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaszynaOperator");

            migrationBuilder.DropTable(
                name: "Maszyny");

            migrationBuilder.DropTable(
                name: "Operatorzy");

            migrationBuilder.DropTable(
                name: "Hale");
        }
    }
}
