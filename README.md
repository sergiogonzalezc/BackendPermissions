# Configuración del proyecto Backend (español)

## Configuración de la Base de Datos ##
Debe ejecutar los scripts que están en la carpeta **Scripts-BD* en SQL Server 2016 o superior (Express, Estandar, Enterprise, etc), en el orden señalado.
- **Nota:** El script 1 crea una BD desde cero en la ubicación **C:\MSSQL_BD** la que debe de estar creada previamente. Si no está ejecutando SQL Server localmente, este paso lo puede omitir y pasar al script 2 y script 3.
- **Script 2**: Crea sólo las tablas.
- **Script 3**: Crea sólo registros de ejemplo de permisos.

## Abrir proyecto en Visual Studio ##
Para ejecutar el backend en modo **DEBUG**, se debe tener instalado:
- [.Net 7x SDK]
- [VS 2022 Community Edition]

## Edición de cadena de conexión:##
Debe ir al archivo **appsettings.json** y deberá editar la **cadenaConexion**, Valores los verá con **XXXXXX**.
- Server Host
- Server Port
- User
- Password

"cadenaConexion": "Server=**XXXXXX**,**XXXXXX**;Initial Catalog=BD_Challenge_Permission;User ID=**XXXXXX**;Password=**XXXXXX**;TrustServerCertificate=true"

Para iniciar la depuración, abrir solución **sln** y ejecutar F5:

**Importante:**
Tipo de proyecto: Api .Net core 

Principales dependencias:
- [MediatR]
- [Automapper]
- [NEST]
- [Swashbuckle.AspNetCore]
- [NLOG]
- [Confluent.Kafka]

## Proyecto compatible para crear imagen Docker. Pasos para crear imagen:##
- [1] Inicar "Docker Desktop for Windows" v4.25.

- [2] Abrir Windows Explorer. Debe ubicarse en el path donde está el archivo **.sln**, abrir **Developer Powershell Visual Studio Community Edition 2022** y ejecutar:

	**docker image build -t backendchallenge:1.0 -f .\BackendPermissions.Api\Dockerfile .**

- [3] Abrir "Docker for Desktop Windows", ir a la lista de imágenes y obtener el **"GUID"** de la nueva imagen recientemente creada,
  
- [4] Crear contenedor a partir de la imagen anteriormente creada, ir a **Developer Powershell Visual Studio** y ejecutar:

  **docker container create backendpermissions-container -p 18000:8000 GUID**

- [5] Iniciar el contenedor: 
  **docker container start backendpermissions-container**

- [6] El contenedor se iniciará en el puerto **18000**. Para validar: abrir al browser en **http://localhost:18000/swagger/index.html** debería ver la lista de métodos expuestos para verificar usando **swagger**.

## Configuración Kafka:##
- Para producir mensajes **Kafka**, debe tener ejecutando Kafka en el puerto **9092** (localhost:9092), y el **topic** es **permission_challenge**

- Para verificar los mensajes **Kafka** del **topic** **permission_challenge**, puede usar el software **Conduktor** 

## Configuración ElasticSearch:##

- Para poder crear índices en **ElasticSearch**, debe iniciarlo en el puerto **9200**, por ejemplo **http://localhost:9200*
    
- La clase "Program" del proyecto "BackendPermissions.Api" define el usuario y contraseña de acceso para **ElasticSearch**. En caso que no acceda por error de autenticación, deberá cambiar la **contraseña** utilizando el comando "elasticsearch-reset-password.bat" 

- El índice por defecto se llama **permission**.

- Fin.

