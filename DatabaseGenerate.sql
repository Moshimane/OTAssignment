IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'OT_Assessment_DB')
BEGIN
	CREATE DATABASE OT_Assessment_DB;

    IF EXISTS (SELECT name FROM sys.databases WHERE name = 'OT_Assessment_DB')
    BEGIN
        PRINT 'Database created successfully.';
    END
    ELSE
    BEGIN
        PRINT 'Database creation failed.';
    END
END
ELSE
BEGIN
	PRINT 'Database already exists.';
END
GO


IF NOT EXISTS (SELECT name FROM sys.tables WHERE name = 'OT_Assessment_DB')
BEGIN
CREATE TABLE [OT_Assessment_DB].[DBO].[CasinoWagers] (
    WagerId NVARCHAR(50) PRIMARY KEY,
    Theme NVARCHAR(50) NOT NULL,
    Provider NVARCHAR(50) NOT NULL,
    GameName NVARCHAR(50) NOT NULL,
    TransactionId NVARCHAR(50) NULL,
    BrandId NVARCHAR(50) NULL,
    AccountId NVARCHAR(50) NULL,
    Username NVARCHAR(50) NULL,
    ExternalReferenceId NVARCHAR(50) NULL,
    TransactionTypeId NVARCHAR(50) NULL,
    Amount DECIMAL NULL,
    CreatedDateTime DATETIME2 NULL,
    NumberOfBets INT NULL,
    CountryCode NVARCHAR(50) NULL,
    SessionData NVARCHAR(500) NULL,
    Duration BIGINT NULL,

);
END
ELSE
BEGIN
	PRINT 'Table already exists.';
END
GO

-- Create a nonclustered index called IX_CasinoWagers_PlayerId if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.indexes WHERE name = 'IX_CasinoWagers_PlayerId')
BEGIN
	CREATE NONCLUSTERED INDEX IX_CasinoWagers_PlayerId
    ON [OT_Assessment_DB].[DBO].[CasinoWagers]  ([AccountId]);
END
ELSE
BEGIN
    PRINT 'Non Clustered Id already exists.';
END
GO

CREATE OR ALTER PROCEDURE sp_AddCasinoWager 
    (@WagerId NVARCHAR(50), @Theme NVARCHAR(50), @Provider VARCHAR,@GameName NVARCHAR(50), @TransactionId NVARCHAR(50), 
	@BrandId NVARCHAR(50), @AccountId NVARCHAR(50), @Username NVARCHAR(50), @ExternalReferenceId NVARCHAR(50), 
	@TransactionTypeId NVARCHAR(50), @Amount DECIMAL, @CreatedDateTime DATETIME2, @NumberOfBets INT, @CountryCode NVARCHAR(50),
	@SessionData NVARCHAR(50), @Duration BIGINT) 
AS  
BEGIN    
    IF NOT EXISTS(SELECT 1 FROM [OT_Assessment_DB].[dbo].[CasinoWagers] WHERE WagerId = @WagerId)
    BEGIN
        INSERT INTO [OT_Assessment_DB].[dbo].[CasinoWagers] (WagerId, Theme, Provider,GameName, TransactionId, BrandId, AccountId, Username, 
        ExternalReferenceId, TransactionTypeId, Amount, CreatedDateTime, NumberOfBets, CountryCode, SessionData, Duration) 
                                VALUES 
        (@WagerId, @Theme, @Provider,@GameName, @TransactionId, @BrandId, @AccountId, @Username, 
        @ExternalReferenceId, @TransactionTypeId, @Amount, @CreatedDateTime, @NumberOfBets, @CountryCode, @SessionData, @Duration);
    END
END 
GO

CREATE OR ALTER PROCEDURE sp_GetTopSpenders 
    (@count int)  
AS  
BEGIN    
    SELECT TOP(@count) AccountId, Username, SUM(Amount) AS TotalAmountSpend FROM [OT_Assessment_DB].[dbo].[CasinoWagers] WITH (NOLOCK) 
    GROUP BY AccountId, Username
    ORDER BY TotalAmountSpend DESC  
END 