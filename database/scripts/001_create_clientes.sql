IF DB_ID('clientes_db') IS NULL
BEGIN
    CREATE DATABASE clientes_db;
END
GO

USE clientes_db;
GO

IF OBJECT_ID('dbo.Clientes', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Clientes
    (
        Id              UNIQUEIDENTIFIER NOT NULL,
        Nome            NVARCHAR(150)    NOT NULL,
        Cpf             CHAR(11)         NOT NULL,
        Email           VARCHAR(200)     NOT NULL,
        DataNascimento  DATE             NOT NULL,
        DataCadastro    DATETIME2(3)     NOT NULL,

        CONSTRAINT PK_Clientes       PRIMARY KEY (Id),
        CONSTRAINT UQ_Clientes_Cpf   UNIQUE (Cpf),
        CONSTRAINT UQ_Clientes_Email UNIQUE (Email)
    );
END
GO
