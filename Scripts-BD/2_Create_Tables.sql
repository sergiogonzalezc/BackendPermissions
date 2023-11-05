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