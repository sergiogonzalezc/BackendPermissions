-- SGC: Start Insert Example data
insert into [dbo].[PermissionTypes] (Descripcion) values ('Admin')
insert into [dbo].[PermissionTypes] (Descripcion) values ('User')
insert into [dbo].[Permissions] (NombreEmpleado,ApellidoEmpleado,TipoPermiso,FechaPermiso) values ('Stan','Lee',1,GETDATE())
insert into [dbo].[Permissions] (NombreEmpleado,ApellidoEmpleado,TipoPermiso,FechaPermiso) values ('John','Due',2,GETDATE())
insert into [dbo].[Permissions] (NombreEmpleado,ApellidoEmpleado,TipoPermiso,FechaPermiso) values ('Peter','Parker',2,GETDATE())
insert into [dbo].[Permissions] (NombreEmpleado,ApellidoEmpleado,TipoPermiso,FechaPermiso) values ('Doctor','Octopus',2,GETDATE())
insert into [dbo].[Permissions] (NombreEmpleado,ApellidoEmpleado,TipoPermiso,FechaPermiso) values ('John','Snow',2,GETDATE())

-- SGC: End Example data
-- SGC: Verify data inserted
select * from Permissions