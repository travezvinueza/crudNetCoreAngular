using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace webApi.Infrastructure.Migrations
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
                    { 1, "Teclado", 12.99m },
                    { 2, "Parlantes", 20.25m },
                    { 3, "Monitor", 30.30m },
                    { 4, "USB", 40.25m },
                    { 5, "Laptop", 155.75m }
                });

            migrationBuilder.Sql(@"
                CREATE PROCEDURE AgregarProducto(
                    IN p_Name VARCHAR(255),
                    IN p_Price DECIMAL(18,2)
                )
                BEGIN
                    INSERT INTO Products (Name, Price)
                    VALUES (p_Name, p_Price);
                    SELECT Id, Name, Price FROM Products WHERE Id = LAST_INSERT_ID();
                    END;
                ");

            migrationBuilder.Sql(@"
    CREATE PROCEDURE ListarProductos(
        IN p_nombre VARCHAR(255),
        IN p_page INT,
        IN p_size INT
    )
    BEGIN
        DECLARE v_offset INT;
        SET v_offset = (p_page - 1) * p_size;

        IF p_nombre IS NULL OR p_nombre = '' THEN
            SELECT * FROM Products
            LIMIT p_size OFFSET v_offset;
            
            SELECT COUNT(*) AS total FROM Products;
        ELSE
            SELECT * FROM Products
            WHERE Name LIKE CONCAT('%', p_nombre, '%')
            LIMIT p_size OFFSET v_offset;

            SELECT COUNT(*) AS total FROM Products
            WHERE Name LIKE CONCAT('%', p_nombre, '%');
        END IF;
    END;
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
