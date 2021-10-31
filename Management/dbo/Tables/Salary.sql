CREATE TABLE [dbo].[Salary] (
    [Id]                 UNIQUEIDENTIFIER NOT NULL,
    [UserId]             UNIQUEIDENTIFIER NULL,
    [CoefficientsSalary] DECIMAL (18, 2)  NULL,
    [WorkingDays]        DECIMAL (18, 2)  NULL,
    [TotalSalary]        DECIMAL (18, 2)  NULL,
    [IsDeleted]          BIT              NULL,
    [CreatedDate]        DATETIME2 (7)    NULL,
    [UpdatedDate]        DATETIME2 (7)    NULL,
    [CreatedBy]          UNIQUEIDENTIFIER NULL,
    [UpdatedBy]          UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Salary] PRIMARY KEY CLUSTERED ([Id] ASC)
);

