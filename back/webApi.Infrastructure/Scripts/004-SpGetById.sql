CREATE PROCEDURE BuscarProductoPorId(
    IN p_id INT
)
BEGIN
    SELECT Id, Name, Price
    FROM Products
    WHERE Id = p_id;
END$$