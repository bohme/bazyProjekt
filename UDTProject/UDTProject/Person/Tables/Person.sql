CREATE TABLE [dbo].[Person]
(
    [id] INT NOT NULL IDENTITY(1,1), 
	[FirstName] TEXT NOT NULL, 
    [LastName] TEXT NOT NULL, 
    [City] TEXT NOT NULL, 
    [Pesel] [dbo].[Pesel] NOT NULL UNIQUE, 
	[BankAccount] [dbo].[BankAccount] NULL,
    [NIP] [dbo].[NIP] NULL, 
	[Email] [dbo].[Email] NULL, 
    PRIMARY KEY ([id]) 
)

