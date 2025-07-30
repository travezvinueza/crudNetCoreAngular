using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace webApi.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Product1", 10.0m },
                    { 2, "Product2", 20.0m },
                    { 3, "Product3", 30.0m },
                    { 4, "Product4", 40.0m },
                    { 5, "Product5", 50.0m },
                    { 6, "Product6", 60.0m },
                    { 7, "Product7", 70.0m },
                    { 8, "Product8", 80.0m },
                    { 9, "Product9", 90.0m },
                    { 10, "Product10", 100.0m }
                });

            migrationBuilder.Sql(@"
                     CREATE PROCEDURE AgregarProducto(
                         IN p_Name VARCHAR(255),
                         IN p_Price DECIMAL(18,2),
                         OUT p_Id INT
                     )
                     BEGIN
                         INSERT INTO Products (Name, Price)
                         VALUES (p_Name, p_Price);
                         SET p_Id = LAST_INSERT_ID();
                     END;
                     ");

            migrationBuilder.Sql(@"
                CREATE PROCEDURE ListarProductos()
                BEGIN
                    SELECT * FROM Products;
                END
                ");

            migrationBuilder.Sql(@"
    CREATE PROCEDURE ActualizarProducto(
        IN p_Id INT,
        IN p_Name VARCHAR(255),
        IN p_Price DECIMAL(18,2)
    )
    BEGIN
        UPDATE Products
        SET Name = p_Name,
            Price = p_Price
        WHERE Id = p_Id;
    END;
    ");

            migrationBuilder.Sql(@"
    CREATE PROCEDURE EliminarProducto(
        IN p_Id INT
    )
    BEGIN
        DELETE FROM Products
        WHERE Id = p_Id;
    END;
    ");
    
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS AgregarProducto;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS ListarProductos;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS ActualizarProducto;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS EliminarProducto;");
        }
    }
}
