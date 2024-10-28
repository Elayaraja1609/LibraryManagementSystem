
CREATE PROCEDURE spBorrowBook
    @UserId INT,
    @BookId INT,
    @BorrowDate DATETIME,
    @DueDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Stock INT;

    -- Check the current stock of the book
    SELECT @Stock = BookQty FROM [dbo].[Books] WHERE BookId = @BookId;

    IF @Stock IS NULL
    BEGIN
        -- Return an empty result set for "Book not found"
        SELECT 'Book not found.' AS Message, NULL AS BookId, NULL AS Title, NULL AS BorrowDate, NULL AS DueDate;
        RETURN;
    END

    IF @Stock <= 0
    BEGIN
        -- Return an empty result set for "Book out of stock"
        SELECT 'Book out of stock.' AS Message, NULL AS BookId, NULL AS Title, NULL AS BorrowDate, NULL AS DueDate;
        RETURN;
    END

    -- Decrement stock
    UPDATE Books 
    SET BookQty = BookQty - 1 
    WHERE BookId = @BookId;

    -- Insert into Transactions table
    INSERT INTO Transactions (UserId, BookId, TransactionDate, DueDate, ReturnDate, [Status])
    VALUES (@UserId, @BookId, @BorrowDate, @DueDate, NULL, 1);

    -- Return details of the transaction with a success message
    SELECT 
        'Book borrowed successfully.' AS Message,
        b.BookId,
        b.BookTitle,
        @BorrowDate AS BorrowDate,
        @DueDate AS DueDate
    FROM 
        Books b
    WHERE 
        b.BookId = @BookId;
END;
GO;

CREATE PROCEDURE spReturnBook
    @UserId INT,
    @BookId INT,
    @ReturnDate DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if the user has borrowed this book
    IF NOT EXISTS (SELECT 1 FROM Transactions WHERE UserId = @UserId AND BookId = @BookId AND ReturnDate IS NULL)
    BEGIN
		SELECT 'This book was not borrowed by the user.' AS Message;
        RETURN;
    END

    -- Increment stock
    UPDATE Books SET BookQty = BookQty + 1 WHERE BookId = @BookId;

    -- Update the return date
    UPDATE Transactions 
    SET [ReturnDate] = @ReturnDate 
    WHERE UserId = @UserId AND BookId = @BookId AND [ReturnDate] IS NULL;

	-- Return success message
    SELECT 'Book returned successfully.' AS Message;
END;
GO;