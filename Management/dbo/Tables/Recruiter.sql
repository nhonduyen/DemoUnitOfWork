CREATE TABLE [dbo].[Recruiter] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [Name]          NVARCHAR (50)    NULL,
    [IsActive]      BIT              NULL,
    [LastSavedUser] UNIQUEIDENTIFIER NULL,
    [CreatedUser]   UNIQUEIDENTIFIER NULL,
    [CreatedTime]   DATETIME2 (7)    NULL,
    [LastSavedTime] DATETIME2 (7)    NULL,
    CONSTRAINT [PK_Recruiter] PRIMARY KEY CLUSTERED ([Id] ASC)
);

