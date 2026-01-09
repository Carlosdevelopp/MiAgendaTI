# Mi Agenda ADO.NET - Sistema de Gestión de Contactos

Sistema web de gestión de contactos desarrollado con ASP.NET Core 8 y ADO.NET para acceso directo a base de datos mediante procedimientos almacenados. El proyecto está enfocado en buenas prácticas de desarrollo backend, diseño limpio y alto rendimiento.

---

## Características Principales

### Acceso a Datos con ADO.NET
- Acceso directo a SQL Server mediante ADO.NET
- Uso de procedimientos almacenados
- Operaciones asíncronas con SqlCommand
- Control total sobre queries SQL
- Alto rendimiento y mínimo overhead

### Arquitectura
- Arquitectura en N capas (Services → Infrastructure → DataAccess)
- Separación clara de responsabilidades
- Inyección de dependencias
- Principios SOLID aplicados

### Configuración
- Uso de User Secrets para datos sensibles
- Sin credenciales en el repositorio

---

## Responsabilidades por Capa

### 01-Services (Presentation Layer)
**Responsabilidad**: Interacción con el usuario

- Recibir y validar entrada del usuario
- Mapear entre ViewModels y Entities
- Gestionar cookies de autenticación
- Trabajar con Claims Principal
- Manejar try-catch y mostrar mensajes al usuario
- Renderizar vistas con Razor
- Aplicar validaciones del lado del cliente

**NO**:
- No lógica de negocio
- No accede directamente a la base de datos
- No conoce detalles de implementación de capas inferiores

---

### 02-Infrastructure (Business Logic Layer)
**Responsabilidad**: Lógica de negocio

- Implementar reglas de negocio
- Validar que las operaciones sean permitidas
- Múltiples operaciones del repositorio
- Proveer servicios de aplicación (Email, Hashing)
- Calcular valores derivados

**NO**:
- No conoce ViewModels (pertenecen a la capa UI)
- No maneja HTTP requests/responses
- No contene lógica de presentación

---

### 03-DataAccess (Data Layer)
**Responsabilidad**: Acceso a datos mediante ADO.NET

- Implementar operaciones CRUD con ADO.NET
- Ejecutar procedimientos almacenados
- Manejar conexiones y comandos SQL de forma asíncrona
- Gestionar parámetros SQL con tipos específicos
- Manejo de valores NULL con DBNull.Value
- Retornar resultados de operaciones

**NO**:
- No contiene lógica de negocio
- No conoce ViewModels
- No maneja try-catch (excepciones suben a capas superiores)

---

## Tecnologías Utilizadas

| Categoría | Tecnología | Versión |
|-----------|------------|---------|
| Backend | ASP.NET Core | 8.0 |
| Lenguaje | C# | 12 |
| Acceso a Datos | ADO.NET | - |
| Base de Datos | SQL Server | 2019+ |
| Seguridad | Argon2id | - |
| Frontend | Razor Views (MVC) | - |
| CSS | Bootstrap | 5.3 |
| Validación | jQuery Validation | 1.19 |

---

## Buenas Prácticas Implementadas

- Programación asíncrona (async/await)
- Uso de procedimientos almacenados
- Inyección de dependencias
- Separación de responsabilidades
- Principios SOLID
- Código limpio y mantenible
- Uso de `await using` para liberar recursos
- Parámetros tipados en SQL para prevenir inyección
- Manejo correcto de valores NULL

---

## Implementación de ADO.NET

### Características Clave

**Procedimientos Almacenados**
- Toda la lógica SQL está en la base de datos
- Reutilización de código SQL
- Mejor rendimiento mediante planes de ejecución
- Mayor seguridad

**Operaciones Asíncronas**
- Uso de `SqlConnection` con `await using`
- `OpenAsync()` para conexiones no bloqueantes
- `ExecuteScalarAsync()`, `ExecuteReaderAsync()`, `ExecuteNonQueryAsync()`
- Liberación automática de recursos

**Manejo de Parámetros**
- Parámetros tipados con `SqlDbType`
- Especificación de tamaño para tipos de longitud variable
- Manejo de valores NULL con `DBNull.Value`
- Prevención de SQL Injection

**Gestión de Conexiones**
- Connection Strings en User Secrets
- Apertura y cierre automático con `await using`
- Manejo eficiente de recursos

---

## Implementación de Seguridad

### Tokens de Recuperación
- Generación con `RandomNumberGenerator` (criptográficamente seguro)
- Hash SHA256 antes de almacenar en base de datos
- Expiración de 1 hora
- Invalidación de tokens previos al generar uno nuevo
- Marcado como "usado" después de resetear contraseña

### Autenticación
- Autenticación basada en Cookies con Claims
- Sin ASP.NET Identity
- Hashing de contraseñas con Argon2id
- Protección CSRF con Anti-Forgery Tokens
- Validación de propiedad de recursos
- Mensajes genéricos para prevenir enumeración de usuarios

---

## Flujo de Usuario

### Registro de Usuario
1. Usuario completa formulario de registro
2. Controller valida datos de entrada
3. Service hashea la contraseña con Argon2id
4. DataAccess ejecuta procedimiento almacenado `RegisterUser`
5. Stored Procedure valida que el usuario no exista
6. SQL Server genera UsuarioId automáticamente
7. Se retorna el ID del usuario creado o -1 si ya existe
8. Service maneja el resultado
9. Usuario recibe confirmación

### Autenticación
1. Usuario ingresa credenciales
2. DataAccess ejecuta procedimiento para validar usuario
3. Service verifica hash de contraseña con Argon2id
4. Se crea cookie de autenticación con Claims
5. Usuario accede a su agenda de contactos

### Recuperación de Contraseña
1. Usuario solicita recuperación desde "Forgot Password"
2. Sistema genera token seguro y lo hashea
3. DataAccess guarda token hasheado mediante stored procedure
4. Se envía email con enlace temporal
5. Usuario hace clic en el enlace (válido 1 hora)
6. Ingresa nueva contraseña
7. DataAccess actualiza contraseña y marca token como usado
8. Sistema confirma actualización

### Gestión de Contactos
1. Usuario autenticado ve solo sus contactos
2. Puede crear, editar y eliminar contactos
3. Cada operación ejecuta su procedimiento almacenado correspondiente
4. Sistema valida que el contacto pertenezca al usuario

---

## Seguridad - Checklist

- Contraseñas hasheadas con Argon2id
- Tokens de recuperación hasheados con SHA256
- Cookies HttpOnly y Secure
- Protección CSRF con Anti-Forgery Tokens
- Validación de propiedad de recursos
- Mensajes genéricos (anti-enumeración)
- User Secrets para datos sensibles
- SQL injection prevenido (parámetros tipados)
- Procedimientos almacenados para lógica de datos

## Implementación pendiente
- Autenticación personalizada sin ASP.NET Identity
- Cookie-based authentication con Claims Principal
- Hashing de contraseñas con Argon2id
- Recuperación de contraseña mediante tokens seguros
- User Secrets para cadenas de conexión
- Protección contra SQL Injection mediante parámetros
- Rate limiting
- Two-Factor Authentication
- Captcha en login
- Logging centralizado

---

## Mejoras Futuras

### Corto Plazo
- Sistema de roles y permisos
- Confirmación de email al registrarse
- Gestión de redes sociales (agregar/eliminar)
- Subida de fotos de contactos
- Exportar contactos a CSV/Excel

### Mediano Plazo
- Autenticación de dos factores (2FA)
- Rate limiting para prevenir ataques
- Historial de cambios en contactos
- Búsqueda y filtrado avanzado
- Logging estructurado con Serilog

### Largo Plazo
- API REST para consumo externo
- Aplicación móvil (Xamarin/MAUI)
- Sincronización con Google Contacts
- Grupos de contactos
- Recordatorios de cumpleaños
- Dashboard con estadísticas

---

## Ventajas de ADO.NET en este Proyecto

### Rendimiento
- Acceso directo a la base de datos sin ORM overhead
- Control total sobre las queries ejecutadas
- Planes de ejecución optimizados en stored procedures

### Control
- Gestión explícita de conexiones y comandos
- Control sobre tipos de datos SQL
- Manejo granular de transacciones

### Seguridad
- Lógica de negocio SQL en la base de datos
- Menor superficie de ataque
- Parámetros tipados previenen SQL Injection

### Mantenibilidad
- Procedimientos almacenados centralizados
- Cambios en SQL sin recompilar aplicación
- Reutilización de lógica de datos

---

## Instalación

### Requisitos Previos
```
✔ .NET 8.0 SDK
✔ SQL Server 2019+
✔ Visual Studio 2022 / VS Code / Rider
```

### Pasos de Instalación

**1. Clonar el repositorio**
```bash
git clone https://github.com/tu-usuario/mi-agenda-adonet.git
cd mi-agenda-adonet
```

**2. Configurar User Secrets**
```bash
cd 01-Services
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Database=AgendaDB;Integrated Security=True;TrustServerCertificate=True;"
```

**3. Crear la base de datos y procedimientos almacenados**
- Ejecutar los scripts SQL incluidos en el proyecto

**4. Restaurar y ejecutar**
```bash
dotnet restore
dotnet run
```

---

## Licencia

Este proyecto está bajo la Licencia MIT. Ver `LICENSE` para más información.

---

## Autor

**Carlos García** - Software Developer (.NET)

- **GitHub**: [https://github.com/Carlosdevelopp](https://github.com/Carlosdevelopp)
- **LinkedIn**: [https://linkedin.com/in/carlosdevel](https://linkedin.com/in/carlosdevel)
- **Email**: carlosdevelopp@gmail.com

---

## Agradecimientos

- Documentación oficial de [ASP.NET Core](https://docs.microsoft.com/aspnet/core)
- Guías de seguridad de [OWASP](https://owasp.org/)
- Comunidad de desarrolladores .NET

---

## Screenshots

### Página de Login
![Login]()

### Registro de Usuario
![Registro]()

### Recuperación de Contraseña
![Recuperación]()

### Agenda de Contactos
![Agenda]()