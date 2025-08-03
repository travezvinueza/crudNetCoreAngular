CREATE PROCEDURE ListarProductos(
    IN p_nombre VARCHAR(255),
    IN p_page INT,
    IN p_size INT
)
BEGIN
    DECLARE v_offset INT;
    SET v_offset = (p_page - 1) * p_size;

    -- Resultado principal: Productos paginados
    IF p_nombre IS NULL OR p_nombre = '' THEN
        SELECT * FROM Products
        LIMIT p_size OFFSET v_offset;
        
        -- Total de productos
        SELECT COUNT(*) AS total FROM Products;
    ELSE
        SELECT * FROM Products
        WHERE Name LIKE CONCAT('%', p_nombre, '%')
        LIMIT p_size OFFSET v_offset;

        -- Total de productos filtrados
        SELECT COUNT(*) AS total FROM Products
        WHERE Name LIKE CONCAT('%', p_nombre, '%');
    END IF;
END$$