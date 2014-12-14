CREATE TABLE [dbo].[Person]
(
    [FirstName] TEXT NOT NULL, 
    [LastName] TEXT NOT NULL, 
    [City] TEXT NOT NULL, 
    [Pesel] [dbo].[Pesel] NOT NULL, 
	[BankAccount] [dbo].[BankAccount] NULL,
    [NIP] [dbo].[NIP] NULL, 
	[Email] [dbo].[Email] NULL, 
    PRIMARY KEY ([Pesel]) 
)

