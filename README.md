# Configuración del proyecto Backend (español)

## Configuración de la Base de Datos ##
Abrir SQL Server 2016 o superior (Express, Estandar, Enterprise, etc). Debe ejecutar los scripts que están en la carpeta **Scripts-BD** en el orden señalado (1, 2 y 3).
- **Nota:** **el script 1** crea un BD **BD_Challenge_Permission** desde cero. Es muy importante que previamente se debe crear la carpeta **C:\MSSQL_BD**. Si no está ejecutando SQL Server localmente y tiene SQL, por ejemplo, en un servidor no local o en la nube, puede omitir este paso e ir al script 2 y al script 3.
- **Script 2**: Crea sólo las tablas en la BD **BD_Challenge_Permission**. Para este paso debe asegúrese que exista la **BD_Challenge_Permission**, y exista el esquema **dbo**.
- **Script 3**: cree solo registros de permisos de ejemplo.

## Abrir proyecto en Visual Studio ##
Para ejecutar el backend en modo **DEBUG**, se debe tener instalado:
- [.Net 7x SDK]
- [VS 2022 Community Edition]

## Edición de cadena de conexión: ##
Debe ir al archivo **appsettings.json** del proyecto **BackendPermissions.Api** y deberá editar la **cadenaConexion**, específicamente los valores marcados con **XXXXXX**:

- Server Host
- Server Port (Si utiliza el puerto TCP 1433, puede evitar este parámetro o definir 1433 directamente.)
- User
- Password

"cadenaConexion": "Server=**XXXXXX**,**XXXXXX**;Initial Catalog=BD_Challenge_Permission;User ID=**XXXXXX**;Password=**XXXXXX**;TrustServerCertificate=true"

## Depuración: ##
Después de editar la cadena de conexión, puede iniciar la depuración. Debe abrir solución **sln** y ejecutar F5:

**Importante:**
Tipo de proyecto: Api .Net core 

Principales dependencias:
- [MediatR]
- [Automapper]
- [NEST]
- [Swashbuckle.AspNetCore]
- [NLOG]
- [Confluent.Kafka]

**La solution iniciará en el puerto 8000:**

## Proyecto compatible para crear imagen Docker. Pasos para crear imagen: ##
- [1] Inicar "Docker Desktop for Windows" v4.25.

- [2] Abrir Windows Explorer. Debe ubicarse en el path donde está el archivo **.sln**, abrir **Developer Powershell Visual Studio Community Edition 2022** y ejecutar:

	**docker image build -t backendchallenge:1.0 -f .\BackendPermissions.Api\Dockerfile .**

- [3] Abrir "Docker for Desktop Windows", ir a la lista de imágenes y obtener el **"GUID"** de la nueva imagen recientemente creada,
  
- [4] Crear contenedor a partir de la imagen anteriormente creada, ir a **Developer Powershell Visual Studio** y ejecutar:

  **docker container create backendpermissions-container -p 18000:8000 GUID**

- [5] Iniciar el contenedor: 
  **docker container start backendpermissions-container**

- [6] El contenedor se iniciará en el puerto **18000**. Para validar: abrir al browser en **http://localhost:18000/swagger/index.html** debería ver la lista de métodos expuestos para verificar usando **swagger**.

## Configuración Kafka: ##
- Para producir mensajes **Kafka**, debe tener ejecutando Kafka en el puerto **9092** (localhost:9092), y el **topic** es **permission_challenge**

- Para verificar los mensajes **Kafka** del **topic** **permission_challenge**, puede usar el software **Conduktor** 

## Configuración ElasticSearch: ##

- Para poder crear índices en **ElasticSearch**, debe iniciarlo en el puerto **9200**, por ejemplo **http://localhost:9200**
    
- La clase "Program" del proyecto **BackendPermissions.Api** define el **usuario** y **contraseña** de acceso para **ElasticSearch**. En caso que no acceda por error de autenticación, deberá cambiar la **contraseña** utilizando el comando "elasticsearch-reset-password.bat" 

- El índice por defecto se llama **permission**.

- Fin.




# Backend project configuration (English)

## Database Configuration ##
Open SQL Server 2016 or higher (Express, Standard, Enterprise, etc.). You must execute the scripts that are in the **Scripts-BD** folder in the order indicated (1, 2 and 3).
- **Note:** **script 1** creates a DB from scratch in the location **C:\MSSQL_DB** which must have been previously created. If you are not running SQL Server locally, you can skip this step and move on to script 2 and script 3.
- **Script 2**: Create only the tables in the DB **BD_Challenge_Permission**. For this step you must ensure that the **BD_Challenge_Permission** exists, and the **dbo** schema exists.
- **Script 3**: Create only example permission records.

## Open project in Visual Studio ##
To run the backend in **DEBUG** mode, you must have installed:
- [.Net 7x SDK]
- [VS 2022 Community Edition]

## Connection string: ##
You need to go to the **appsettings.json** file of the **BackendPermissions.Api** project and you will need to edit the **connectionstring**, specifically the values marked with **XXXXXX**:

- Server Host
- Server Port (if you use TCP 1433 port, you can avoid this parameter, or define 1433 directly)
- User
- Password

"connectionstring": "Server=**XXXXXX**,**XXXXXX**;Initial Catalog=BD_Challenge_Permission;User ID=**XXXXXX**;Password=**XXXXXX**;TrustServerCertificate=true"

## Debugging: ##
After editing the connection string, you can start debugging. You must open solution **sln** and execute F5:

**Important:**
Project type: Api .Net core

Main dependencies:
- [MediatR]
- [Automapper]
- [NEST]
- [Swashbuckle.AspNetCore]
- [NLOG]
- [Confluent.Kafka]

**The solution will start in port 8000:**

## Supported project to create Docker image. Steps to create image: ##
- [1] Start "Docker Desktop for Windows" v4.25.

- [2] Open Windows Explorer. You must go to the path where the **.sln** file is, open **Developer Powershell Visual Studio Community Edition 2022** and execute:

**docker image build -t backendchallenge:1.0 -f .\BackendPermissions.Api\Dockerfile .**

- [3] Open "Docker for Desktop Windows", go to the image list and get the **"GUID"** of the newly created new image,
  
- [4] Create container from the previously created image, go to **Developer Powershell Visual Studio** and execute:

   **docker container create backendpermissions-container -p 18000:8000 GUID**

- [5] Start the container:
   **docker container start backendpermissions-container**

- [6] The container will be started on port **18000**. To validate: open the browser at **http://localhost:18000/swagger/index.html** you should see the list of exposed methods to verify using **swagger**.

## Kafka configuration: ##
- To produce **Kafka** messages, you must have Kafka running on port **9092** (localhost:9092), and the **topic** is **permission_challenge**

- To check **Kafka** messages from **topic** **permission_challenge**, you can use **Conduktor** software

## ElasticSearch configuration: ##

- In order to create indexes on **ElasticSearch**, you must start it on port **9200**, for example **http://localhost:9200**
    
- The "Program" class of the **BackendPermissions.Api** project defines the access **user** and **password** for **ElasticSearch**. If you do not log in due to an authentication error, you must change the **password** using the command "elasticsearch-reset-password.bat"

- The default index is called **permission**.

- End.
