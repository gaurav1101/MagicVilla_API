using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_VillaApi.Migrations
{
    public partial class addUsersToDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocalUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalUsers", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ImgUrl" },
                values: new object[] { new DateTime(2023, 5, 30, 9, 55, 23, 583, DateTimeKind.Local).AddTicks(9862), "https://dotnetmastery.com/bluevillaimages/villa3.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ImgUrl" },
                values: new object[] { new DateTime(2023, 5, 30, 9, 55, 23, 583, DateTimeKind.Local).AddTicks(9881), "https://dotnetmastery.com/bluevillaimages/villa1.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ImgUrl" },
                values: new object[] { new DateTime(2023, 5, 30, 9, 55, 23, 583, DateTimeKind.Local).AddTicks(9884), "https://dotnetmastery.com/bluevillaimages/villa4.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ImgUrl" },
                values: new object[] { new DateTime(2023, 5, 30, 9, 55, 23, 583, DateTimeKind.Local).AddTicks(9887), "https://dotnetmastery.com/bluevillaimages/villa5.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ImgUrl" },
                values: new object[] { new DateTime(2023, 5, 30, 9, 55, 23, 583, DateTimeKind.Local).AddTicks(9890), "https://dotnetmastery.com/bluevillaimages/villa2.jpg" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocalUsers");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ImgUrl" },
                values: new object[] { new DateTime(2023, 5, 29, 10, 18, 9, 820, DateTimeKind.Local).AddTicks(4400), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa3.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "ImgUrl" },
                values: new object[] { new DateTime(2023, 5, 29, 10, 18, 9, 820, DateTimeKind.Local).AddTicks(4419), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa1.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "ImgUrl" },
                values: new object[] { new DateTime(2023, 5, 29, 10, 18, 9, 820, DateTimeKind.Local).AddTicks(4423), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa4.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "ImgUrl" },
                values: new object[] { new DateTime(2023, 5, 29, 10, 18, 9, 820, DateTimeKind.Local).AddTicks(4426), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa5.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "ImgUrl" },
                values: new object[] { new DateTime(2023, 5, 29, 10, 18, 9, 820, DateTimeKind.Local).AddTicks(4428), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa2.jpg" });
        }
    }
}
