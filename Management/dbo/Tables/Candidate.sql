CREATE TABLE [dbo].[Candidate] (
    [Id]            UNIQUEIDENTIFIER NOT NULL,
    [Name]          NVARCHAR (50)    NULL,
    [IsActive]      BIT              NULL,
    [RecruiterId]   UNIQUEIDENTIFIER NULL,
    [LastSavedUser] UNIQUEIDENTIFIER NULL,
    [CreatedUser]   UNIQUEIDENTIFIER NULL,
    [CreatedTime]   DATETIME2 (7)    NULL,
    [LastSavedTime] DATETIME2 (7)    NULL,
    CONSTRAINT [PK_Candidate] PRIMARY KEY CLUSTERED ([Id] ASC)
);

