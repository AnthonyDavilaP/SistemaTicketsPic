# Sistema de Tickets PIC

Este proyecto corresponde a un sistema de gestión de tickets desarrollado como parte del Proyecto Integrador de Conocimientos (PIC).  
El objetivo es brindar una solución académica y práctica para la administración de incidencias, solicitudes y requerimientos dentro de una organización.

---

## Objetivo
Diseñar e implementar un sistema de tickets que permita registrar, dar seguimiento y resolver solicitudes de usuarios, optimizando la comunicación y la gestión interna.

---

## Tecnologías Utilizadas
- **Lenguaje:** C# (.NET)
- **Base de Datos:** SQL Server
- **ORM:** Entity Framework
- **Entorno:** Windows Forms
- **Control de Versiones:** Git y GitHub

---

## Funcionalidades Principales
- Módulo de Login y Roles de Usuario (Administrador / Usuario / Técnicos).
- Gestión de Usuarios.
- Registro y administración de Tickets (creación, edición, cierre).
- Seguimiento de estado de tickets.
- Reportes básicos de gestión.

---

## Arquitectura
El sistema está basado en **programación orientada a objetos (POO)**, con entidades principales:
- Usuario
- Ticket
- Administrador
- Categoría
- Estado

---

## Ejecución del Proyecto

1. **Clonar el repositorio:**
   ```bash
   git clone https://github.com/AnthonyDavilaP/SistemaTicketsPic.git
   ```

2. **Configurar la base de datos en SQL Server ejecutando el script:**
   ```sql
   -- ========================================
   -- Creación de la base de datos
   -- ========================================
   CREATE DATABASE PIC;
   GO

   USE PIC;
   GO

   -- ========================================
   -- Tabla Usuarios
   -- ========================================
   CREATE TABLE Usuarios (
       IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
       Nombre NVARCHAR(MAX) NULL,
       Correo NVARCHAR(MAX) NULL,
       Cargo NVARCHAR(MAX) NULL,
       Discriminator NVARCHAR(128) NULL,
       Clave NVARCHAR(255) NULL
   );

   -- ========================================
   -- Tabla Tickets
   -- ========================================
   CREATE TABLE Tickets (
       IdTicket INT IDENTITY(1,1) PRIMARY KEY,
       Descripcion NVARCHAR(MAX) NULL,
       FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),
       Estado NVARCHAR(MAX) NULL,
       Prioridad INT NOT NULL,
       TecnicoId INT NULL,
       UsuarioId INT NOT NULL,

       CONSTRAINT FK_Tickets_Usuarios FOREIGN KEY (UsuarioId) REFERENCES Usuarios(IdUsuario),
       CONSTRAINT FK_Tickets_Tecnico FOREIGN KEY (TecnicoId) REFERENCES Usuarios(IdUsuario)
   );
   ```

3. **Abrir el proyecto en Visual Studio.**  
4. **Compilar y ejecutar.**
5. **Al abrir por primera vez, se crea un usuario por defecto**
   Correo: admin@iti.edu.ec
   Contraseña: 1234

---

## Autor
**Anthony Dávila P.**
