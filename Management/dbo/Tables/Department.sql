CREATE TABLE [dbo].[Department] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [DepartmentName] NVARCHAR (100)   NULL,
    [IsDeleted]      BIT              NULL,
    [CreatedDate]    DATETIME2 (7)    NULL,
    [UpdatedDate]    DATETIME2 (7)    NULL,
    [CreatedBy]      UNIQUEIDENTIFIER NULL,
    [UpdatedBy]      UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED ([Id] ASC)
);

