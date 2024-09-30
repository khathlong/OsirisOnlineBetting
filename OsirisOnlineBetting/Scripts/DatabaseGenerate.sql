CREATE DATABASE OT_Assessment_DB;
GO

CREATE TABLE PlayerAccount
(
    AccountID UNIQUEIDENTIFIER PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL
);
GO

CREATE UNIQUE INDEX IX_PlayerAccount_Username ON PlayerAccount (Username);
GO

CREATE TABLE PlayerCasinoWager
(
    WagerID UNIQUEIDENTIFIER PRIMARY KEY,
    Game NVARCHAR(100),
    Provider NVARCHAR(100),
    AccountID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES PlayerAccount(AccountID),
    Amount DECIMAL(18, 2),
    CreatedDateTime DATETIME2,
	Theme nvarchar (100) NULL,
	TransactionID UNIQUEIDENTIFIER NULL,
	BrandID UNIQUEIDENTIFIER NULL,
	ExternalReferenceID UNIQUEIDENTIFIER NULL,
	TransactionTypeID UNIQUEIDENTIFIER NULL,
	NumberOfBets int NULL,
	CountryCode NVARCHAR (5) NULL,
	SessionData NVARCHAR (max) NULL,
	Duration BIGINT NULL,
);
GO

CREATE INDEX IX_PlayerCasinoWager_AccountID ON PlayerCasinoWager (AccountID);
GO

CREATE PROCEDURE [dbo].[InsertPlayerCasinoWager]
    @WagerID UNIQUEIDENTIFIER,
    @GameName NVARCHAR(100),
    @Provider NVARCHAR(100),
    @AccountID UNIQUEIDENTIFIER,
    @Amount DECIMAL(18, 2),
    @CreatedDateTime DATETIME2(7),
    @Theme NVARCHAR(100),
    @TransactionID UNIQUEIDENTIFIER,
    @BrandID UNIQUEIDENTIFIER,
    @ExternalReferenceID UNIQUEIDENTIFIER,
    @TransactionTypeID UNIQUEIDENTIFIER,
    @NumberOfBets INT,
    @CountryCode NVARCHAR(5),
    @SessionData NVARCHAR(MAX),
    @Duration BIGINT
AS
BEGIN

    INSERT INTO [dbo].[PlayerCasinoWager] (
        WagerID,
        GameName,
        Provider,
        AccountID,
        Amount,
        CreatedDateTime,
        Theme,
        TransactionID,
        BrandID,
        ExternalReferenceID,
        TransactionTypeID,
        NumberOfBets,
        CountryCode,
        SessionData,
        Duration
    )
    VALUES (
        @WagerID,
        @GameName,
        @Provider,
        @AccountID,
        @Amount,
        @CreatedDateTime,
        @Theme,
        @TransactionID,
        @BrandID,
        @ExternalReferenceID,
        @TransactionTypeID,
        @NumberOfBets,
        @CountryCode,
        @SessionData,
        @Duration
    );

END
GO

CREATE PROCEDURE [dbo].[InsertPlayer]
    @AccountId UNIQUEIDENTIFIER,
    @Username NVARCHAR(100)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM PlayerAccount WHERE AccountID = @AccountId)
    BEGIN
        INSERT INTO PlayerAccount (AccountID, Username)
        VALUES (@AccountId, @Username)
    END
END
GO

CREATE PROCEDURE [dbo].[GetPlayer]
    @AccountID UNIQUEIDENTIFIER
AS
BEGIN
    SELECT AccountID, Username
    FROM PlayerAccount
    WHERE AccountID = @AccountID;
END;
GO

CREATE PROCEDURE [dbo].[GetPlayerWagers]
    @PlayerID UNIQUEIDENTIFIER,
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SELECT WagerID, GameName, Provider, Amount, CreatedDateTime
    FROM PlayerCasinoWager
    WHERE AccountID = @PlayerID
    ORDER BY CreatedDateTime DESC;
END;
GO

CREATE PROCEDURE [dbo].[GetTopSpenders]
    @Count INT
AS
BEGIN
	SELECT TOP (@Count) a.AccountID, b.Username, SUM(a.Amount) AS TotalAmountSpend
	FROM PlayerCasinoWager a 
	INNER JOIN PlayerAccount b ON a.AccountID = b.AccountID
	GROUP BY a.AccountID, b.Username
	ORDER BY TotalAmountSpend DESC;
END;
GO
