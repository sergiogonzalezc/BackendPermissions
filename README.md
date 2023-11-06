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




# Backend project configuration (English)

## Database Configuration ##
You must execute the scripts that are in the **Scripts-BD* folder in SQL Server 2016 or higher (Express, Standard, Enterprise, etc.), in the order indicated.
- **Note:** Script 1 creates a DB from scratch in the location **C:\MSSQL_BD** which must have been previously created. If you are not running SQL Server locally, you can skip this step and move on to script 2 and script 3.
- **Script 2**: Create only the tables.
- **Script 3**: Create only example permission records.

## Open project in Visual Studio ##
To run the backend in **DEBUG** mode, you must have installed:
- [.Net 7x SDK]
- [VS 2022 Community Edition]

## Connection string edit:##
You must go to the **appsettings.json** file and you must edit the **connectionstring**, you will see the values with **XXXXXX**.
-Server Host
- Server Port
- User
- Password

"connectionstring": "Server=**XXXXXX**,**XXXXXX**;Initial Catalog=BD_Challenge_Permission;User ID=**XXXXXX**;Password=**XXXXXX**;TrustServerCertificate=true"

To start debugging, open **sln** solution and run F5:

**Important:**
Project type: Api .Net core

Main dependencies:
- [MediatR]
- [Automapper]
- [NEST]
- [Swashbuckle.AspNetCore]
- [NLOG]
- [Confluent.Kafka]

## Supported project to create Docker image. Steps to create image:##
- [1] Start "Docker Desktop for Windows" v4.25.

- [2] Open Windows Explorer. You must go to the path where the **.sln** file is, open **Developer Powershell Visual Studio Community Edition 2022** and execute:

**docker image build -t backendchallenge:1.0 -f .\BackendPermissions.Api\Dockerfile .**

- [3] Open "Docker for Desktop Windows", go to the image list and get the **"GUID"** of the newly created new image,
  
- [4] Create container from the previously created image, go to **Developer Powershell Visual Studio** and execute:

   **docker container create backendpermissions-container -p 18000:8000 GUID**

- [5] Start the container:
   **docker container start backendpermissions-container**

- [6] The container will be started on port **18000**. To validate: open the browser at **http://localhost:18000/swagger/index.html** you should see the list of exposed methods to verify using **swagger**.

## Kafka configuration:##
- To produce **Kafka** messages, you must have Kafka running on port **9092** (localhost:9092), and the **topic** is **permission_challenge**

- To check **Kafka** messages from **topic** **permission_challenge**, you can use **Conduktor** software

## ElasticSearch configuration:##

- In order to create indexes on **ElasticSearch**, you must start it on port **9200**, for example **http://localhost:9200*
    
- The "Program" class of the "BackendPermissions.Api" project defines the access user and password for **ElasticSearch**. If you do not log in due to an authentication error, you must change the **password** using the command "elasticsearch-reset-password.bat"

- The default index is called **permission**.

- End.
