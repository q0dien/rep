# ProductsApi3 — Dependency Injection и логирование

## Практическая работа №3

**Тема:** Внедрение зависимостей (Dependency Injection) и логирование в ASP.NET Core.

## Цель работы

В этой работе я изучила применение Dependency Injection в ASP.NET Core, регистрацию сервисов с разными временами жизни и использование `ILogger<T>` для ведения логов.

Также я реализовала простой Web API для работы с товарами без использования базы данных.

## Используемые технологии

* C#
* ASP.NET Core Web API
* .NET
* Swagger
* Dependency Injection
* ILogger
* In-memory список товаров

## Структура проекта

```text
ProductsApi3
├── Controllers
│   └── ProductsController.cs
├── Models
│   └── Product.cs
├── Services
│   ├── IProductService.cs
│   └── ProductService.cs
├── Properties
├── Screenshots
├── Program.cs
├── appsettings.json
├── appsettings.Development.json
├── ProductsApi3.csproj
└── README.md
```

## Модель Product

Для представления товара создан класс `Product` со следующими свойствами:

```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

В приложении используется список товаров в памяти, без подключения базы данных.

Начальные товары:

* Laptop — 450000
* Mouse — 15000
* Keyboard — 25000

## Сервис IProductService

Для работы с товарами создан интерфейс `IProductService`.

Он содержит методы:

```csharp
IEnumerable<Product> GetAll();
Product? GetById(int id);
Product Add(Product product);
bool Delete(int id);
```

Интерфейс позволяет отделить контроллер от конкретной реализации сервиса.

## Реализация ProductService

Интерфейс реализован классом `ProductService`.

Внутри сервиса используется список `List<Product>`, который хранит товары в памяти.

База данных в данной работе не используется.

## Dependency Injection

Сервис зарегистрирован в `Program.cs` с использованием `Scoped`:

```csharp
builder.Services.AddScoped<IProductService, ProductService>();
```

Я не создаю `ProductService` напрямую через `new ProductService()`. Вместо этого ASP.NET Core автоматически передаёт нужную зависимость в контроллер.

## Constructor Injection

В `ProductsController` используются две зависимости:

```csharp
private readonly IProductService _productService;
private readonly ILogger<ProductsController> _logger;
```

Они передаются через конструктор:

```csharp
public ProductsController(
    IProductService productService,
    ILogger<ProductsController> logger)
{
    _productService = productService;
    _logger = logger;
}
```

Таким образом, контроллер получает необходимые зависимости через Dependency Injection.

## Реализованный API

| Метод  | Endpoint             | Назначение               |
| ------ | -------------------- | ------------------------ |
| GET    | `/api/products`      | Получение всех товаров   |
| GET    | `/api/products/{id}` | Получение товара по ID   |
| POST   | `/api/products`      | Добавление нового товара |
| DELETE | `/api/products/{id}` | Удаление товара          |

Для успешных операций используются HTTP-коды `200 OK` и `201 Created`.

Если товар не найден, возвращается `404 Not Found`.

## Swagger

Для проверки API используется Swagger.

Через Swagger я проверила:

1. получение всех товаров;
2. получение товара по ID;
3. запрос несуществующего товара;
4. добавление нового товара;
5. получение созданного товара;
6. удаление товара;
7. повторное получение удалённого товара.

## Логирование

Для логирования используется:

```csharp
ILogger<ProductsController>
```

В контроллере добавлены сообщения разных уровней.

При получении всех товаров:

```csharp
_logger.LogInformation("Getting all products");
```

При успешном получении товара:

```csharp
_logger.LogInformation(
    "Product with ID {ProductId} was found",
    id);
```

Если товар не найден:

```csharp
_logger.LogWarning(
    "Product with ID {ProductId} was not found",
    id);
```

При создании товара:

```csharp
_logger.LogInformation(
    "Product {ProductName} was created",
    product.Name);
```

При удалении товара:

```csharp
_logger.LogInformation(
    "Product with ID {ProductId} was deleted",
    id);
```

Если удаляемый товар отсутствует, записывается сообщение уровня `Warning`.

## Настройка логирования

В `Program.cs` подключены Console и Debug:

```csharp
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
```

Это позволяет видеть сообщения логирования в консоли и в Debug.

## Проверка уровня логирования

Я провела эксперимент с настройкой уровня логирования.

Изначально в настройках использовался уровень:

```json
"Default": "Information"
```

При таком уровне в консоли отображаются сообщения `Information`, `Warning` и более серьёзные сообщения.

Затем я изменила уровень:

```json
"Default": "Warning"
```

После этого сообщения уровня `Information`, например:

```text
Getting all products
```

перестали отображаться.

Сообщения `Warning` при этом продолжили выводиться.

Это происходит потому, что установленный уровень `Warning` не позволяет выводить сообщения более низкого уровня, например `Information`.

После проведения эксперимента настройки были возвращены обратно на `Information`.

## Времена жизни Dependency Injection

В ходе работы я проверила три варианта регистрации `IProductService`.

### Transient

```csharp
builder.Services.AddTransient<IProductService, ProductService>();
```

При `Transient` создаётся новый экземпляр сервиса каждый раз, когда он запрашивается из контейнера зависимостей.

Такой вариант можно использовать для небольших сервисов без состояния, когда отдельный экземпляр нужен для каждой зависимости.

### Scoped

```csharp
builder.Services.AddScoped<IProductService, ProductService>();
```

При `Scoped` один экземпляр сервиса используется в рамках одного HTTP-запроса.

Такой lifetime часто используется для сервисов, работающих с данными в рамках запроса.

### Singleton

```csharp
builder.Services.AddSingleton<IProductService, ProductService>();
```

При `Singleton` создаётся один экземпляр сервиса, который используется в течение всего времени работы приложения.

Такой вариант можно применять для общих сервисов, если их состояние безопасно использовать между разными запросами.

### Результат эксперимента

Я по очереди использовала `Transient`, `Scoped` и `Singleton` и проверила работу API.

Во всех трёх случаях приложение запускалось и endpoint получения товаров работал.

Для итоговой версии проекта я оставила:

```csharp
builder.Services.AddScoped<IProductService, ProductService>();
```

## Тестирование API

В Swagger были выполнены следующие проверки.

### 1. GET всех товаров

Запрос:

```text
GET /api/products
```

Результат:

```text
200 OK
```

API вернул список товаров.

### 2. GET товара по ID

Запрос:

```text
GET /api/products/1
```

Результат:

```text
200 OK
```

Был получен товар с ID `1`.

### 3. GET несуществующего товара

Запрос:

```text
GET /api/products/999
```

Результат:

```text
404 Not Found
```

В консоли при этом появляется сообщение уровня `Warning`.

### 4. POST нового товара

Был отправлен новый товар:

```json
{
  "name": "Monitor",
  "price": 120000
}
```

Результат:

```text
201 Created
```

API автоматически присвоил новому товару следующий ID.

### 5. GET созданного товара

После добавления я повторно получила созданный товар по его ID.

Результат:

```text
200 OK
```

### 6. DELETE товара

Удаление созданного товара:

```text
DELETE /api/products/4
```

Результат:

```text
200 OK
```

### 7. Проверка после удаления

После удаления был выполнен повторный запрос:

```text
GET /api/products/4
```

Результат:

```text
404 Not Found
```

Таким образом, я проверила не только успешные операции, но и обработку ситуации, когда товар отсутствует.

## Обработка исключений

В качестве дополнительного задания в метод `GetById` была добавлена обработка исключений через `try-catch`.

При возникновении исключения ошибка записывается в лог:

```csharp
_logger.LogError(
    ex,
    "Error while processing product with ID {ProductId}",
    id);
```

Клиенту при этом не передаются технические подробности исключения.

Вместо этого возвращается общий ответ:

```text
500 Internal Server Error
```

с сообщением о том, что при обработке запроса произошла ошибка.

## Контрольные вопросы

### 1. Что такое Dependency Injection?

Dependency Injection (DI) — это способ передачи объекту необходимых зависимостей извне, вместо того чтобы создавать их внутри самого объекта.

### 2. Какую проблему решает Dependency Injection?

DI уменьшает связанность между классами и позволяет проще изменять, тестировать и повторно использовать отдельные части приложения.

### 3. Что такое слабая связанность?

Слабая связанность означает, что классы как можно меньше зависят от конкретных реализаций друг друга. Например, контроллер работает с интерфейсом `IProductService`, а не напрямую с классом `ProductService`.

### 4. Зачем использовать интерфейсы при Dependency Injection?

Интерфейс описывает, какие методы должен предоставлять сервис, но не определяет конкретную реализацию. Благодаря этому реализацию можно заменить без изменения контроллера.

### 5. Что такое Constructor Injection?

Constructor Injection — это передача зависимостей через конструктор класса. В моей работе `IProductService` и `ILogger<ProductsController>` передаются в конструктор `ProductsController`.

### 6. Почему зависимости регистрируются в Program.cs?

В `Program.cs` настраивается контейнер Dependency Injection приложения. Там указывается, какую реализацию нужно создавать для определённого интерфейса и какой lifetime использовать.

### 7. Чем отличаются Transient, Scoped и Singleton?

`Transient` создаёт новый экземпляр при каждом запросе зависимости.

`Scoped` создаёт один экземпляр в рамках одного HTTP-запроса.

`Singleton` создаёт один экземпляр на всё время работы приложения.

В итоговой версии проекта используется `Scoped`.

### 8. Что такое ILogger<T>?

`ILogger<T>` — стандартный механизм логирования в ASP.NET Core. С его помощью можно записывать информацию о работе приложения, предупреждения и ошибки.

### 9. Чем отличаются Information, Warning и Error?

`Information` используется для обычных информационных сообщений о работе приложения.

`Warning` сообщает о ситуации, которая не является критической, но требует внимания.

`Error` используется для сообщений об ошибках и исключениях.

### 10. Для чего нужен appsettings.json?

`appsettings.json` используется для хранения настроек приложения. В моей работе через него задаётся минимальный уровень логирования и другие параметры конфигурации.

## Скриншоты

В папке `Screenshots` находятся результаты выполнения и проверки работы проекта:

1. Регистрация Dependency Injection.
2. Constructor Injection.
3. Swagger с endpoint'ами.
4. GET всех товаров.
5. GET несуществующего товара — 404.
6. POST создание товара.
7. GET созданного товара.
8. DELETE товара.
9. GET после удаления — 404.
10. Логи уровня Information.
11. Логи уровня Warning.
12. Эксперимент с уровнем логирования Warning.

## Вывод

В ходе практической работы я научилась использовать Dependency Injection в ASP.NET Core и передавать зависимости в контроллер через конструктор.

Я проверила работу трёх вариантов lifetime: `Transient`, `Scoped` и `Singleton`, а для итоговой версии выбрала `Scoped`.

Также я добавила логирование с помощью `ILogger<ProductsController>` и проверила работу уровней `Information`, `Warning` и `Error`.

Дополнительно я реализовала обработку исключений, при которой технические подробности ошибки не передаются клиенту.

В результате у меня получилось Web API для работы с товарами с использованием Dependency Injection, логирования и обработки ошибок.
