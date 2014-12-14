CREATE TABLE [dbo].[Person]
(
    [FirstName] TEXT NOT NULL, 
    [LastName] TEXT NOT NULL, 
    [City] TEXT NULL, 
    [Pesel] [dbo].[Pesel] NOT NULL, 
	[BankAccount] [dbo].[BankAccount] NOT NULL,
    [NIP] [dbo].[NIP] NOT NULL, 
    PRIMARY KEY ([Pesel]) 
)

