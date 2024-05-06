Create database LibraryDb;
GO

USE "LibraryDb";

Create Table Clients (
Id INT IDENTITY(1,1) PRIMARY KEY,
FirstName NVARCHAR(MAX) NOT NULL,
LastName NVARCHAR(MAX) DEFAULT '',
Phone NVARCHAR(MAX) DEFAULT '',
Email NVARCHAR(MAX) DEFAULT '');

CREATE TABLE Books (
Id INT IDENTITY(1,1) PRIMARY KEY,
Title NVARCHAR(MAX) NOT NULL,
Description NVARCHAR(MAX) NOT NULL);

CREATE TABLE Issuancies(
Id INT IDENTITY(1,1) PRIMARY KEY,
ClientId INT REFERENCES Clients(Id) NOT NULL,
BookId INT REFERENCES Books(Id) NOT NULL,
IssuanceDate DATE NOT NULL,
RequiredReturningDate DATE NOT NULL,
ActualReturningDate DATE,
Status INT);
GO

CREATE PROCEDURE EditBookDescription
	@Id INT,
	@Title NVARCHAR(MAX),
	@Description NVARCHAR(MAX)
AS
	UPDATE Books SET Title=@Title, 
					 Description=@Description
			     where Id=@Id

