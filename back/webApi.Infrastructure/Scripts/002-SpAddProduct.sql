CREATE PROCEDURE AgregarProducto(
    IN p_nombre VARCHAR(255),
    IN p_precio DECIMAL(18,2)
)
BEGIN
    INSERT INTO Products (Name, Price)
    VALUES (p_nombre, p_precio);
    SELECT Id, Name, Price FROM Products WHERE Id = LAST_INSERT_ID();
END$$