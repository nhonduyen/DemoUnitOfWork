CREATE TABLE [dbo].[User] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [Username]     NCHAR (50)       NULL,
    [Email]        NCHAR (50)       NULL,
    [DepartmentId] UNIQUEIDENTIFIER NULL,
    [IsDeleted]    BIT              NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC)
);

