-- DROP SCHEMA dbo;

CREATE SCHEMA dbo;
-- Course.dbo.Students definition

-- Drop table

-- DROP TABLE Course.dbo.Students;

CREATE TABLE Course.dbo.Students (
	Id uniqueidentifier NOT NULL,
	Name nvarchar COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	NRIC nvarchar COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Birthday datetime2 NOT NULL,
	AvailableDate datetime2 NULL,
	Gender int NOT NULL,
	CONSTRAINT PK_Students PRIMARY KEY (Id)
);


-- Course.dbo.[__EFMigrationsHistory] definition

-- Drop table

-- DROP TABLE Course.dbo.[__EFMigrationsHistory];

CREATE TABLE Course.dbo.[__EFMigrationsHistory] (
	MigrationId nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProductVersion nvarchar(32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId)
);


-- Course.dbo.Subjects definition

-- Drop table

-- DROP TABLE Course.dbo.Subjects;

CREATE TABLE Course.dbo.Subjects (
	Id uniqueidentifier NOT NULL,
	Name nvarchar COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Value int NOT NULL,
	StudentId uniqueidentifier NOT NULL,
	CONSTRAINT PK_Subjects PRIMARY KEY (Id),
	CONSTRAINT FK_Subjects_Students_StudentId FOREIGN KEY (StudentId) REFERENCES Course.dbo.Students(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_Subjects_StudentId ON dbo.Subjects (  StudentId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
