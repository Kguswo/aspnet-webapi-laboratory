CREATE DATABASE SampleDB;
GO

USE SampleDB;
GO

-- 테이블 생성
CREATE TABLE [dbo].[Products] (
    [ProductId]          UNIQUEIDENTIFIER NOT NULL,
    [ProductName]        NVARCHAR (100)   NULL,
    [Price]              DECIMAL (18, 2)  NULL,
    [ProductDescription] NVARCHAR (MAX)   NULL,
    [CreatedOn]          DATETIME         NULL,
    [UpdatedOn]          DATETIME         NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([ProductId] ASC)
);

-- EXEC sp_rename 'Products.UpdateOn', 'UpdatedOn', 'COLUMN';