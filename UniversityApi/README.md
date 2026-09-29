# University Course Management API

## 1. Описание работы

В рамках итоговой практической работы по модулям 1–4 я разработала REST Web API для управления университетскими курсами.

Система позволяет работать со студентами, преподавателями, курсами и записями студентов на курсы.

В процессе работы я реализовала REST API на ASP.NET Core, подключила базу данных через Entity Framework Core, добавила DTO, AutoMapper, Repository Pattern, Dependency Injection, централизованную обработку ошибок, логирование и Swagger.

## 2. Цель работы

Целью работы было разработать полноценный Web API и на практике показать:

* клиент-серверную архитектуру;
* REST;
* HTTP-методы;
* маршрутизацию;
* Controller;
* Dependency Injection;
* многослойную архитектуру;
* DTO;
* AutoMapper;
* Repository Pattern;
* Entity Framework Core;
* работу с базой данных;
* CRUD-операции;
* валидацию;
* обработку ошибок;
* HTTP status codes;
* логирование;
* Swagger/OpenAPI.

## 3. Используемые технологии

* C#
* .NET 10
* ASP.NET Core Web API
* Entity Framework Core
* SQLite
* AutoMapper
* Swagger / OpenAPI
* Repository Pattern
* Dependency Injection
* Middleware
* Git / GitHub

## 4. Архитектура проекта

В проекте используется многослойная архитектура.

Общая схема:

```text
Client
   |
   v
Controllers
   |
   v
Services
   |
   v
Repositories
   |
   v
Entity Framework Core
   |
   v
SQLite Database
```

Контроллер получает HTTP-запрос и передаёт его в Service.

Service содержит основную бизнес-логику и проверки.

Repository отвечает за взаимодействие с базой данных.

Entity Framework Core используется как ORM для работы с таблицами базы данных.

## 5. Структура проекта

```text
UniversityApi
│
├── Common
│   └── ReturnResult.cs
│
├── Controllers
│   ├── StudentsController.cs
│   ├── TeachersController.cs
│   ├── CoursesController.cs
│   └── EnrollmentsController.cs
│
├── Data
│   └── ApplicationDbContext.cs
│
├── DTO
│   ├── Students
│   ├── Teachers
│   ├── Courses
│   └── Enrollments
│
├── Mapping
│   └── MappingProfile.cs
│
├── Middleware
│   └── ExceptionMiddleware.cs
│
├── Migrations
│   └── InitialCreate
│
├── Models
│   ├── Student.cs
│   ├── Teacher.cs
│   ├── Course.cs
│   └── Enrollment.cs
│
├── Repositories
│   ├── Interfaces
│   └── Implementations
│
├── Services
│   ├── IStudentService.cs
│   ├── StudentService.cs
│   ├── ITeacherService.cs
│   ├── TeacherService.cs
│   ├── ICourseService.cs
│   ├── CourseService.cs
│   ├── IEnrollmentService.cs
│   └── EnrollmentService.cs
│
├── Screenshots
│   ├── 1-swagger.png
│   ├── 2-student-create-201.png
│   ├── ...
│   └── 20-student-courses-200.png
│
├── appsettings.json
├── Program.cs
└── UniversityApi.csproj
```

## 6. Модели данных

В приложении используются четыре основные сущности:

* Student;
* Teacher;
* Course;
* Enrollment.

### Student

Модель студента содержит:

* Id;
* FirstName;
* LastName;
* Email;
* BirthDate;
* CreatedAt.

### Teacher

Модель преподавателя содержит:

* Id;
* FirstName;
* LastName;
* Email;
* Department.

### Course

Модель курса содержит:

* Id;
* Name;
* Description;
* Credits;
* TeacherId;
* CreatedAt.

### Enrollment

Модель записи на курс содержит:

* Id;
* StudentId;
* CourseId;
* EnrollmentDate;
* Grade.

## 7. Работа со студентами

Для студентов реализован полный CRUD:

* получение всех студентов;
* получение студента по ID;
* создание;
* изменение;
* удаление.

Также реализован отдельный endpoint для получения курсов конкретного студента.

### Создание студента

Используется:

`POST /api/Students`

Пример данных:

```json
{
  "firstName": "Медина",
  "lastName": "Мусаева",
  "email": "medina@example.com",
  "birthDate": "2005-12-12"
}
```

После успешного создания API возвращает HTTP `201 Created`.

![Создание студента — 201](Screenshots/2-student-create-201.png)

### Получение списка студентов

Используется:

`GET /api/Students`

При успешном запросе возвращается HTTP `200 OK`.

![Получение студентов — 200](Screenshots/3-students-get-200.png)

### Получение студента по ID

Используется:

`GET /api/Students/{id}`

![Получение студента по ID — 200](Screenshots/4-student-get-by-id-200.png)

### Студент не найден

При запросе несуществующего ID API возвращает `404 Not Found`.

![Студент не найден — 404](Screenshots/5-student-not-found-404.png)

### Проверка валидации

При передаче неправильных данных API возвращает `400 Bad Request`.

![Ошибка валидации — 400](Screenshots/6-student-validation-400.png)

## 8. Работа с преподавателями

Для преподавателей реализован полный CRUD:

* GET;
* POST;
* PUT;
* DELETE.

### Создание преподавателя

Используется:

`POST /api/Teachers`

В тестировании был создан преподаватель Айдана Серикова.

![Создание преподавателя — 201](Screenshots/7-teacher-create-201.png)

## 9. Работа с курсами

Для курсов реализованы:

* получение списка;
* получение по ID;
* создание;
* изменение;
* удаление;
* поиск по названию.

### Создание курса

Используется:

`POST /api/Courses`

Пример:

```json
{
  "name": "Web Development",
  "description": "Разработка веб-приложений и веб-сервисов",
  "credits": 5,
  "teacherId": 1
}
```

Курс связывается с преподавателем через `TeacherId`.

![Создание курса — 201](Screenshots/8-course-create-201.png)

### Поиск курсов

Для поиска используется параметр `search`.

Пример:

`GET /api/Courses?search=Web`

![Поиск курса](Screenshots/9-course-search-200.png)

### Проверка существования преподавателя

При создании курса система проверяет, существует ли преподаватель с указанным `TeacherId`.

Если преподаватель отсутствует, возвращается `404 Not Found`.

![Преподаватель не найден — 404](Screenshots/10-course-teacher-not-found-404.png)

### Изменение курса

Используется:

`PUT /api/Courses/{id}`

![Изменение курса — 200](Screenshots/11-course-update-200.png)

### Удаление курса

Используется:

`DELETE /api/Courses/{id}`

![Удаление курса](Screenshots/12-course-delete-200.png)

### Проверка после удаления

После удаления обращение к удалённому курсу возвращает `404 Not Found`.

![Курс не найден — 404](Screenshots/13-course-not-found-404.png)

## 10. Работа с Enrollment

Enrollment используется для записи студента на курс.

Для него реализованы:

* получение всех записей;
* получение записи по ID;
* создание записи;
* изменение оценки;
* удаление записи.

### Запись студента на курс

Используется:

`POST /api/Enrollments`

Пример:

```json
{
  "studentId": 2,
  "courseId": 2
}
```

При успешном создании возвращается `201 Created`.

![Создание записи — 201](Screenshots/14-enrollment-create-201.png)

### Проверка повторной записи

В системе нельзя записать одного студента на один и тот же курс дважды.

При повторной попытке API возвращает:

`409 Conflict`

с кодом ошибки `ALREADY_ENROLLED`.

![Повторная запись — 409](Screenshots/15-enrollment-duplicate-409.png)

### Изменение оценки

Оценка изменяется через:

`PUT /api/Enrollments/{id}`

В тестировании оценка была изменена на `95`.

![Изменение оценки — 200](Screenshots/16-enrollment-update-grade-200.png)

### Получение списка записей

Используется:

`GET /api/Enrollments`

Ответ содержит информацию о студенте, курсе, дате записи и оценке.

![Получение записей — 200](Screenshots/17-enrollments-get-200.png)

### Удаление записи

Используется:

`DELETE /api/Enrollments/{id}`

![Удаление записи](Screenshots/18-enrollment-delete-200.png)

### Проверка после удаления

После удаления запись становится недоступной.

При повторном запросе возвращается `404 Not Found`.

![Запись не найдена — 404](Screenshots/19-enrollment-not-found-404.png)

## 11. Курсы конкретного студента

Для получения курсов, на которые записан конкретный студент, был добавлен endpoint:

`GET /api/Students/{id}/courses`

Например:

`GET /api/Students/2/courses`

API возвращает список курсов, связанных со студентом через Enrollment.

![Курсы студента — 200](Screenshots/20-student-courses-200.png)

## 12. Entity Framework Core

Для работы с базой данных используется Entity Framework Core.

Основным классом является:

`ApplicationDbContext`

В нём определены:

```text
DbSet<Student> Students
DbSet<Teacher> Teachers
DbSet<Course> Courses
DbSet<Enrollment> Enrollments
```

База данных содержит таблицы:

* Students;
* Teachers;
* Courses;
* Enrollments.

Для создания структуры базы данных используются миграции Entity Framework Core.

## 13. Настройка моделей

В проекте используются два способа настройки моделей.

### Data Annotations

В моделях используются атрибуты:

* `[Required]`;
* `[MaxLength]`;
* `[EmailAddress]`;
* `[Range]`;
* `[Key]`.

Например, для Email студента используется проверка формата и максимальной длины.

### Fluent API

В `ApplicationDbContext` через Fluent API настроены:

* уникальность Email;
* связи между Student и Enrollment;
* связи между Course и Enrollment;
* связь Course и Teacher;
* внешние ключи;
* запрет удаления преподавателя, если у него есть связанные курсы;
* уникальность пары StudentId + CourseId.

## 14. Repository Pattern

Для работы с данными используются репозитории.

Например:

```text
IStudentRepository
       |
       v
StudentRepository
       |
       v
ApplicationDbContext
```

Для каждой основной сущности существует интерфейс репозитория и его реализация.

Репозитории содержат методы для:

* получения списка;
* получения объекта по ID;
* создания;
* изменения;
* удаления.

Бизнес-логика не смешивается непосредственно с кодом доступа к базе данных.

## 15. Service Layer

Между Controller и Repository используется Service Layer.

Например:

```text
StudentsController
       |
       v
IStudentService
       |
       v
StudentService
       |
       v
IStudentRepository
       |
       v
StudentRepository
```

В Service выполняются бизнес-проверки.

Например:

* существует ли студент;
* существует ли преподаватель;
* существует ли курс;
* не существует ли уже такая запись Enrollment;
* не занят ли уже Email.

## 16. Dependency Injection

Все основные зависимости регистрируются в `Program.cs`.

Репозитории и сервисы зарегистрированы как `Scoped`.

Например:

```text
IStudentRepository -> StudentRepository
IStudentService -> StudentService
```

Зависимости передаются через конструкторы классов.

Контроллеры не создают сервисы и репозитории вручную через `new`.

## 17. DTO

Для передачи данных используются DTO.

Например, для студентов:

```text
StudentCreateDto
StudentUpdateDto
StudentDto
```

Для преподавателей:

```text
TeacherCreateDto
TeacherUpdateDto
TeacherDto
```

Для курсов:

```text
CourseCreateDto
CourseUpdateDto
CourseDto
```

Для Enrollment:

```text
EnrollmentCreateDto
EnrollmentUpdateDto
EnrollmentDto
```

DTO позволяют отделить модели базы данных от данных, которые передаются через API.

## 18. AutoMapper

Для преобразования Entity и DTO используется AutoMapper.

Все основные настройки находятся в:

`Mapping/MappingProfile.cs`

Например:

```text
Student -> StudentDto
StudentCreateDto -> Student
StudentUpdateDto -> Student
```

Для Course также настроено получение имени преподавателя, а для Enrollment — имени студента и названия курса.

## 19. Единый формат ответа

Для ответов API используется общий класс:

`ReturnResult<T>`

Он содержит:

* `StatusCode`;
* `IsSuccess`;
* `Result`;
* `ErrorCode`;
* `ErrorMessage`;
* `TraceId`.

Пример успешного ответа:

```json
{
  "statusCode": 200,
  "isSuccess": true,
  "result": {},
  "errorCode": null,
  "errorMessage": null,
  "traceId": null
}
```

Пример ошибки:

```json
{
  "statusCode": 404,
  "isSuccess": false,
  "result": null,
  "errorCode": "STUDENT_NOT_FOUND",
  "errorMessage": "Student not found"
}
```

## 20. Обработка ошибок

Для централизованной обработки непредвиденных исключений используется:

`ExceptionMiddleware`

Если во время выполнения происходит необработанное исключение, Middleware перехватывает его и возвращает клиенту структурированный ответ с кодом `500`.

При этом клиенту не передаются внутренние детали приложения, stack trace или SQL-запросы.

## 21. HTTP Status Codes

В проекте используются следующие HTTP-коды:

| Код | Назначение                  |
| --- | --------------------------- |
| 200 | Успешное выполнение запроса |
| 201 | Объект успешно создан       |
| 400 | Ошибка валидации            |
| 404 | Объект не найден            |
| 409 | Конфликт данных             |
| 500 | Внутренняя ошибка сервера   |

## 22. Логирование

Для логирования используется стандартный `ILogger`.

В проекте записываются события:

* создание студента;
* изменение студента;
* удаление студента;
* создание преподавателя;
* изменение преподавателя;
* удаление преподавателя;
* создание курса;
* изменение курса;
* удаление курса;
* создание Enrollment;
* изменение оценки;
* удаление Enrollment;
* попытки обращения к отсутствующим объектам;
* необработанные исключения.

Используются уровни:

* Information;
* Warning;
* Error.

## 23. Swagger / OpenAPI

Для тестирования API используется Swagger.

После запуска приложения Swagger доступен по адресу:

`http://localhost:5203/swagger`

Через Swagger можно выполнять HTTP-запросы и проверять ответы API.

![Swagger](Screenshots/1-swagger.png)

Порт приложения может изменяться при запуске. Актуальный адрес отображается в консоли.

## 24. Основные endpoints

### Students

| Метод  | Endpoint                     | Назначение              |
| ------ | ---------------------------- | ----------------------- |
| GET    | `/api/Students`              | Получить всех студентов |
| GET    | `/api/Students/{id}`         | Получить студента       |
| GET    | `/api/Students/{id}/courses` | Получить курсы студента |
| POST   | `/api/Students`              | Создать студента        |
| PUT    | `/api/Students/{id}`         | Изменить студента       |
| DELETE | `/api/Students/{id}`         | Удалить студента        |

### Teachers

| Метод  | Endpoint             | Назначение                   |
| ------ | -------------------- | ---------------------------- |
| GET    | `/api/Teachers`      | Получить всех преподавателей |
| GET    | `/api/Teachers/{id}` | Получить преподавателя       |
| POST   | `/api/Teachers`      | Создать преподавателя        |
| PUT    | `/api/Teachers/{id}` | Изменить преподавателя       |
| DELETE | `/api/Teachers/{id}` | Удалить преподавателя        |

### Courses

| Метод  | Endpoint            | Назначение         |
| ------ | ------------------- | ------------------ |
| GET    | `/api/Courses`      | Получить все курсы |
| GET    | `/api/Courses/{id}` | Получить курс      |
| POST   | `/api/Courses`      | Создать курс       |
| PUT    | `/api/Courses/{id}` | Изменить курс      |
| DELETE | `/api/Courses/{id}` | Удалить курс       |

Поиск:

`GET /api/Courses?search=Web`

### Enrollments

| Метод  | Endpoint                | Назначение                |
| ------ | ----------------------- | ------------------------- |
| GET    | `/api/Enrollments`      | Получить все записи       |
| GET    | `/api/Enrollments/{id}` | Получить запись           |
| POST   | `/api/Enrollments`      | Записать студента на курс |
| PUT    | `/api/Enrollments/{id}` | Изменить оценку           |
| DELETE | `/api/Enrollments/{id}` | Удалить запись            |

## 25. Бизнес-правила

В проекте реализованы основные проверки:

1. Нельзя записать студента на несуществующий курс.
2. Нельзя создать Enrollment для несуществующего студента.
3. Нельзя дважды записать одного студента на один курс.
4. Курс не может ссылаться на несуществующего преподавателя.
5. Нельзя успешно получить объект, которого нет в базе.
6. Email студента должен быть уникальным.
7. Email преподавателя должен быть уникальным.
8. Количество кредитов курса должно быть от 1 до 10.
9. Оценка должна быть от 0 до 100.

## 26. Миграции базы данных

Для создания миграции использовалась команда:

```text
dotnet ef migrations add InitialCreate
```

Для применения миграции:

```text
dotnet ef database update
```

Файлы миграции находятся в папке:

`Migrations`

Локальная база данных создаётся на основе этих миграций.

## 27. Запуск проекта

Для запуска проекта необходимо перейти в папку:

`UniversityApi`

и выполнить:

```text
dotnet restore
```

Затем применить миграции:

```text
dotnet ef database update
```

После этого запустить приложение:

```text
dotnet run
```

После запуска открыть Swagger по адресу, который отображается в консоли.

## 28. Контрольные вопросы

### 1. Что такое REST API?

REST API — это способ взаимодействия между клиентом и сервером через HTTP. В моём проекте REST используется для работы со студентами, преподавателями, курсами и записями на курсы.

### 2. Какие HTTP-методы используются в проекте?

Используются GET для получения данных, POST для создания, PUT для изменения и DELETE для удаления.

### 3. Что такое Controller?

Controller принимает HTTP-запрос, определяет необходимый endpoint и передаёт выполнение в Service.

### 4. Почему бизнес-логику не стоит хранить в Controller?

Чтобы разделить ответственность между компонентами. Controller отвечает за HTTP-запросы, а Service содержит бизнес-логику.

### 5. Что такое Dependency Injection?

Dependency Injection — это механизм передачи зависимостей объекту извне. В проекте зависимости регистрируются в `Program.cs` и передаются через конструкторы.

### 6. Что такое Repository Pattern?

Repository Pattern отделяет работу с базой данных от бизнес-логики приложения. В проекте для этого используются интерфейсы и реализации репозиториев.

### 7. Что такое Entity Framework Core?

Entity Framework Core — это ORM для .NET, которая позволяет работать с базой данных через объекты C#.

### 8. Что такое DbContext?

`DbContext` является основным классом для взаимодействия приложения с базой данных. В проекте используется `ApplicationDbContext`.

### 9. Что такое DbSet?

`DbSet` представляет набор объектов определённой сущности в базе данных. Например, `DbSet<Student>` представляет студентов.

### 10. Что такое DTO?

DTO — Data Transfer Object. Это объект, предназначенный для передачи данных между клиентом и API.

### 11. Зачем нужен AutoMapper?

AutoMapper используется для автоматического преобразования Entity в DTO и DTO в Entity.

### 12. Какие способы настройки моделей используются?

В проекте используются Data Annotations и Fluent API.

### 13. Какие связи между сущностями есть в проекте?

Преподаватель может вести несколько курсов. Студент может иметь несколько записей на курсы. Курс также может иметь несколько записей студентов.

### 14. Как реализована валидация?

Для валидации используются атрибуты `[Required]`, `[MaxLength]`, `[EmailAddress]` и `[Range]`. Дополнительно бизнес-проверки выполняются в Service.

### 15. Какие HTTP-коды используются?

В проекте используются 200, 201, 400, 404, 409 и 500.

### 16. Как реализована обработка исключений?

Для этого используется `ExceptionMiddleware`, который перехватывает необработанные исключения и возвращает структурированный ответ с кодом 500.

### 17. Что такое Middleware?

Middleware — компонент ASP.NET Core, который участвует в обработке HTTP-запросов и ответов. В проекте он используется для централизованной обработки исключений.

### 18. Как реализовано логирование?

Для логирования используется `ILogger`. Записываются создание, изменение, удаление, ошибки и попытки обращения к несуществующим объектам.

### 19. Что такое миграции Entity Framework Core?

Миграции позволяют создавать и изменять структуру базы данных на основе изменений моделей. В проекте используются команды `dotnet ef migrations add` и `dotnet ef database update`.

### 20. Для чего используется Swagger?

Swagger предоставляет веб-интерфейс для просмотра и тестирования API. Через него можно выполнять GET, POST, PUT и DELETE запросы.

## 29. Результат работы

В результате я разработала REST Web API для управления университетскими курсами.

В проекте реализованы:

* CRUD студентов;
* CRUD преподавателей;
* CRUD курсов;
* работа с Enrollment;
* получение курсов студента;
* поиск курсов;
* проверка бизнес-правил;
* DTO;
* AutoMapper;
* Repository Pattern;
* Dependency Injection;
* Entity Framework Core;
* реляционная база данных;
* единый формат ответов;
* обработка ошибок;
* логирование;
* Swagger/OpenAPI;
* миграции базы данных.

Работа была проверена через Swagger с использованием успешных запросов и различных сценариев ошибок.
