CREATE PROCEDURE EliminarProducto(
    IN p_id INT
)
BEGIN
    DELETE FROM Products
    WHERE Id = p_id;
END$$