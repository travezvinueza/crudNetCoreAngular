CREATE PROCEDURE ActualizarProducto(
    IN p_id INT,
    IN p_nombre VARCHAR(255),
    IN p_precio DECIMAL(18,2)
)
BEGIN
    UPDATE Products
    SET Name = p_nombre,
        Price = p_precio
    WHERE Id = p_id;
END$$