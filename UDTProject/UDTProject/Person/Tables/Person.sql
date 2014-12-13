CREATE TABLE [dbo].[Person]
(
    [FirstName] TEXT NOT NULL, 
    [LastName] TEXT NOT NULL, 
    [City] TEXT NULL, 
    [Pesel] [dbo].[Pesel] NOT NULL, 
    PRIMARY KEY ([Pesel]) 
)
