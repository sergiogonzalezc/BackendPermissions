﻿USE [master]
go
IF EXISTS (SELECT * from dbo.sysobjects where id=OBJECT_ID('TEMP_DATA_BD_Challenge_Permission')) 
	DROP TABLE master.dbo.TEMP_DATA_BD_Challenge_Permission 

DECLARE @PATH_DATOS  VARCHAR(200)               /* Data donde se almacenará la BD*/

set @PATH_DATOS   = 'C:\MSSQL_BD'     /* IMPORTANTE: CARPETA DEBE ESTAR CREADA PREVIAMENTE!! */

/************************************************************************/
SET NOCOUNT ON;
USE [master]
CREATE TABLE TEMP_DATA_BD_Challenge_Permission (path_datos varchar(200))
INSERT INTO TEMP_DATA_BD_Challenge_Permission (path_datos) 
    VALUES (@PATH_DATOS)

DECLARE  @PATH_DATOS2 VARCHAR(200)
SELECT  @PATH_DATOS2=path_datos from master.dbo.TEMP_DATA_BD_Challenge_Permission
EXECUTE('CREATE DATABASE [BD_Challenge_Permission] ON  PRIMARY 
( NAME = ''BD_Challenge_Permission'', FILENAME = '''+ @PATH_DATOS2+'\BD_Challenge_Permission.mdf'' , SIZE = 200000KB , MAXSIZE = 1GB, FILEGROWTH = 512000KB )
 LOG ON 
( NAME = ''BD_Challenge_Permission_Log'', FILENAME = '''+ @PATH_DATOS2+'\BD_Challenge_Permission.ldf'' , SIZE = 200000KB , MAXSIZE = 1GB , FILEGROWTH = 10%)
');
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'BD_Challenge_Permission', @new_cmptlevel=130    /* IMPORTANTE: NIVEL 130 de Compatibility ES PARA SQL SERVER 2016 o SUPERIOR!! */
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BD_Challenge_Permission].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [BD_Challenge_Permission] SET ANSI_NULL_DEFAULT OFF
ALTER DATABASE [BD_Challenge_Permission] SET ANSI_NULLS OFF
ALTER DATABASE [BD_Challenge_Permission] SET ANSI_PADDING OFF
ALTER DATABASE [BD_Challenge_Permission] SET ANSI_WARNINGS OFF
ALTER DATABASE [BD_Challenge_Permission] SET ARITHABORT OFF
ALTER DATABASE [BD_Challenge_Permission] SET AUTO_CLOSE OFF
ALTER DATABASE [BD_Challenge_Permission] SET AUTO_CREATE_STATISTICS ON
ALTER DATABASE [BD_Challenge_Permission] SET AUTO_SHRINK OFF
ALTER DATABASE [BD_Challenge_Permission] SET AUTO_UPDATE_STATISTICS ON
ALTER DATABASE [BD_Challenge_Permission] SET CURSOR_CLOSE_ON_COMMIT OFF
ALTER DATABASE [BD_Challenge_Permission] SET CURSOR_DEFAULT  GLOBAL
ALTER DATABASE [BD_Challenge_Permission] SET CONCAT_NULL_YIELDS_NULL OFF
ALTER DATABASE [BD_Challenge_Permission] SET NUMERIC_ROUNDABORT OFF
ALTER DATABASE [BD_Challenge_Permission] SET QUOTED_IDENTIFIER OFF
ALTER DATABASE [BD_Challenge_Permission] SET RECURSIVE_TRIGGERS OFF
ALTER DATABASE [BD_Challenge_Permission] SET  DISABLE_BROKER
ALTER DATABASE [BD_Challenge_Permission] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
ALTER DATABASE [BD_Challenge_Permission] SET DATE_CORRELATION_OPTIMIZATION OFF
ALTER DATABASE [BD_Challenge_Permission] SET TRUSTWORTHY OFF
ALTER DATABASE [BD_Challenge_Permission] SET ALLOW_SNAPSHOT_ISOLATION OFF
ALTER DATABASE [BD_Challenge_Permission] SET PARAMETERIZATION SIMPLE
ALTER DATABASE [BD_Challenge_Permission] SET READ_COMMITTED_SNAPSHOT OFF
ALTER DATABASE [BD_Challenge_Permission] SET  READ_WRITE
ALTER DATABASE [BD_Challenge_Permission] SET RECOVERY SIMPLE
ALTER DATABASE [BD_Challenge_Permission] SET  MULTI_USER
ALTER DATABASE [BD_Challenge_Permission] SET PAGE_VERIFY CHECKSUM
ALTER DATABASE [BD_Challenge_Permission] SET DB_CHAINING OFF
GO
USE [BD_Challenge_Permission]
GO
/**************************** CREACION DE OBJETOS ******************************/


USE [BD_Challenge_Permission]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Permissions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Permissions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NombreEmpleado]  [varchar](50) NOT NULL,
	[ApellidoEmpleado] [varchar](50) NOT NULL,
	[TipoPermiso] [int] NOT NULL,
	[FechaPermiso] [datetime] NOT NULL,	
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PermissionTypes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PermissionTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Descripcion]  [varchar](50) NOT NULL,
 CONSTRAINT [PK_PermissionTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

ALTER TABLE [dbo].[Permissions] WITH CHECK ADD CONSTRAINT [FK_Permissions_IdType] FOREIGN KEY([TipoPermiso])
REFERENCES [dbo].[PermissionTypes] ([Id])

ALTER TABLE [dbo].[Permissions] CHECK CONSTRAINT [FK_Permissions_IdType]

GO