# Mi Agenda ADO.NET - Sistema de Gestión de Contactos

Sistema web de gestión de contactos desarrollado con ASP.NET Core MVC y ADO.NET para acceso directo a base de datos mediante procedimientos almacenados. Implementa autenticación segura sin Identity, gestión completa de contactos (CRUD) y recuperación de contraseñas mediante correo electrónico.

---

## Características Principales

### Acceso a Datos con ADO.NET
- **Acceso directo** a SQL Server mediante ADO.NET
- **Procedimientos almacenados** para toda la lógica de datos
- **Operaciones asíncronas** con SqlCommand
- **Control total** sobre queries SQL
- **Alto rendimiento** y mínimo overhead
- **Gestión explícita** de conexiones y comandos

### Seguridad y Autenticación
- **Autenticación personalizada** sin ASP.NET Identity
- **Cookie-based authentication** con Claims Principal
- **Hashing de contraseñas** con Argon2id
- **Recuperación de contraseña** mediante tokens seguros por correo electrónico
- **Tokens hasheados** en base de datos (nunca almacenados en texto plano)
- **Validación de propiedad** de recursos (usuarios solo acceden a sus propios contactos)
- **Protección CSRF** con Anti-Forgery Tokens
- **Mensajes genéricos** para prevenir enumeración de usuarios
- **Parámetros tipados** para prevenir SQL Injection

### Sistema de Correo Electrónico
- **Envío de correos** mediante SMTP
- **Recuperación de contraseña** con enlaces seguros
- **Configuración segura** mediante User Secrets

### Gestión de Contactos
- **CRUD completo** (Crear, Leer, Actualizar, Eliminar)
- **Información del contacto**: nombre, apellidos, teléfono, fecha de nacimiento
- **Cálculo automático** de edad
- **Redes sociales** asociadas a cada contacto
- **Ordenamiento** alfabético automático
- **Validación de datos** tanto en cliente como en servidor

---

## Responsabilidades por Capa

### ServicesUI (Presentation Layer)
**Responsabilidad**: Interacción con el usuario

- Recibir y validar entrada del usuario
- Mapear entre ViewModels y Entities
- Gestionar cookies de autenticación
- Trabajar con Claims Principal
- Manejar try-catch y mostrar mensajes al usuario
- Renderizar vistas con Razor
- Aplicar validaciones del lado del cliente

**NO**:
- Lógica de negocio
- Accede directamente a la base de datos
- Conoce detalles de implementación de capas inferiores

---

### Infrastructure (Business Logic Layer)
**Responsabilidad**: Lógica de negocio

- Implementar reglas de negocio
- Validar que las operaciones sean permitidas
- Múltiples operaciones del repositorio
- Proveer servicios de aplicación (Email, Hashing)
- Calcular valores derivados (edad a partir de fecha de nacimiento)

**NO**:
- No conoce ViewModels (pertenecen a la capa UI)
- No maneja HTTP requests/responses
- No contiene lógica de presentación

---

### DataAccess (Data Layer)
**Responsabilidad**: Acceso a datos mediante ADO.NET

- Implementar operaciones CRUD con ADO.NET
- Ejecutar procedimientos almacenados
- Manejar conexiones y comandos SQL de forma asíncrona
- Gestionar parámetros SQL con tipos específicos
- Manejo de valores NULL con DBNull.Value
- Usar `await using` para liberación automática de recursos

**NO**:
- No contiene lógica de negocio
- No conoce ViewModels
- No maneja try-catch (excepciones suben a capas superiores)

---

## Tecnologías y Patrones Implementados

### Stack Tecnológico

| Categoría | Tecnología | Versión | Propósito |
|-----------|------------|---------|-----------|
| **Backend** | ASP.NET Core | 8.0 | Framework web |
| **Acceso a Datos** | ADO.NET | - | Acceso directo a BD |
| **Base de Datos** | SQL Server | 2019+ | Persistencia |
| **Seguridad** | Argon2id | - | Hashing de contraseñas |
| **Frontend** | Razor Views (MVC) | - | Motor de vistas |
| **Frontend** | jQuery | - | Validaciones del lado del cliente |
| **CSS** | Bootstrap | 5.3 | Diseño responsive |
| **Validación** | jQuery Validation | 1.19 | Validación cliente |
| **Email** | System.Net.Mail | - | Envío SMTP |

### Patrones de Diseño
- **Repository Pattern** - Abstracción del acceso a datos
- **Dependency Injection** - Inyección de dependencias nativa de .NET
- **ViewModel Pattern** - Separación entre modelos de dominio y presentación
- **Base Controller Pattern** - Reutilización de funcionalidad común

### Principios SOLID
- **Single Responsibility** - Cada clase tiene una única responsabilidad
- **Dependency Inversion** - Dependencia de abstracciones (interfaces)
- **Separation of Concerns** - Separación clara entre capas

---

## Implementación de ADO.NET

*** Gestión de Conexiones**
- Connection Strings en User Secrets
- Uso de `await using` para liberación automática de recursos
- Apertura y cierre eficiente de conexiones

*** Prevención de SQL Injection**
- Parámetros tipados con `SqlDbType`
- Especificación de tamaño para tipos de longitud variable
- Nunca concatenación de strings en SQL

---

##  Implementación de Seguridad

### Tokens de Recuperación
- Generación con `RandomNumberGenerator` (criptográficamente seguro)
- Hash SHA256 antes de almacenar en base de datos
- Expiración de 1 hora
- Invalidación de tokens previos al generar uno nuevo
- Marcado como "usado" después de resetear contraseña
- Almacenamiento mediante procedimiento almacenado

###  Autenticación
- Autenticación basada en Cookies con Claims
- Sin ASP.NET Identity (implementación personalizada)
- Hashing de contraseñas con Argon2id
- Protección CSRF con Anti-Forgery Tokens
- Validación de propiedad de recursos
- Mensajes genéricos para prevenir enumeración de usuarios

---

##  Flujo de Usuario

###  Registro y Autenticación
1. Usuario se registra con email y contraseña
2. Contraseña se hashea con Argon2id antes de guardarse
3. DataAccess ejecuta procedimiento almacenado `RegisterUser`
4. Stored Procedure valida que el usuario no exista
5. SQL Server genera UsuarioId automáticamente
6. Usuario inicia sesión con credenciales
7. Se crea cookie de autenticación con Claims
8. Usuario accede a su agenda de contactos

###  Recuperación de Contraseña
1. Usuario solicita recuperación desde "Forgot Password"
2. Sistema genera token seguro y lo hashea
3. DataAccess ejecuta `CreatePasswordResetToken` stored procedure
4. Se envía email con enlace temporal
5. Usuario hace clic en el enlace (válido 1 hora)
6. Ingresa nueva contraseña
7. DataAccess ejecuta `ResetPassword` stored procedure
8. Token se marca como usado y contraseña se actualiza

###  Gestión de Contactos
1. Usuario autenticado ve solo sus contactos
2. DataAccess ejecuta `GetContactsByUserId` stored procedure
3. Usuario puede crear contacto (ejecuta `CreateContact`)
4. Usuario puede editar contacto (ejecuta `UpdateContact`)
5. Usuario puede eliminar contacto (ejecuta `DeleteContact`)
6. Sistema valida que el contacto pertenezca al usuario
7. Edad se calcula automáticamente en la capa de negocio

---

##  Seguridad - Checklist

-  Contraseñas hasheadas con Argon2id
-  Tokens de recuperación hasheados con SHA256
-  Cookies HttpOnly y Secure
-  Protección CSRF con Anti-Forgery Tokens
-  Validación de propiedad de recursos
-  Mensajes genéricos (anti-enumeración)
-  User Secrets para datos sensibles
-  SQL injection prevenido (parámetros tipados)
-  XSS prevenido (Razor encode automático)
-  Procedimientos almacenados para toda la lógica de datos

###  Implementación pendiente
-  Rate limiting
-  Two-Factor Authentication
-  Captcha en login
-  Logging centralizado

---

##  Mejoras Futuras

###  Corto Plazo
- Sistema de roles y permisos
- Confirmación de email al registrarse
- Gestión de redes sociales (agregar/eliminar)
- Subida de fotos de contactos
- Exportar contactos a CSV/Excel
- Logging con Serilog

###  Mediano Plazo
- Autenticación de dos factores (2FA)
- Rate limiting para prevenir ataques
- Historial de cambios en contactos
- Búsqueda y filtrado avanzado
- Paginación de resultados
- API REST para consumo externo

---

##  Licencia

Este proyecto está bajo la Licencia MIT. Ver `LICENSE` para más información.

---

##  Autor

**Carlos García** - Software Developer (.NET)

-  **LinkedIn**: [carlosdevelopp](https://linkedin.com/in/carlosdevel)
-  **GitHub**: [Carlosdevelopp](https://github.com/Carlosdevelopp)
-  **Email**: carlosdevelopp@gmail.com

---

##  Agradecimientos

- Documentación oficial de [ASP.NET Core](https://docs.microsoft.com/aspnet/core)
- Guías de seguridad de [OWASP](https://owasp.org/)
- Comunidad de desarrolladores .NET
