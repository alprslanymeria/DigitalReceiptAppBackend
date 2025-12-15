# MediatR ve CQRS İmplementasyonu

Bu proje, **MediatR** kütüphanesi kullanılarak **CQRS (Command Query Responsibility Segregation)** pattern'i ile geliştirilmiştir.

## 📋 İçerik

- [MediatR Nedir?](#mediatr-nedir)
- [CQRS Nedir?](#cqrs-nedir)
- [Proje Yapısı](#proje-yapısı)
- [Kurulum](#kurulum)
- [Kullanım Örnekleri](#kullanım-örnekleri)
- [Validasyon](#validasyon)

## 🔍 MediatR Nedir?

MediatR, .NET uygulamalarında **Mediator Pattern**'i uygulamak için kullanılan bir kütüphanedir. Bu pattern, nesneler arasındaki bağımlılıkları azaltarak gevşek bağlı (loosely coupled) bir mimari oluşturmaya yardımcı olur.

### Avantajları:
- ✅ Separation of Concerns (Endişelerin Ayrılması)
- ✅ Daha test edilebilir kod
- ✅ Single Responsibility Principle (Tek Sorumluluk İlkesi)
- ✅ Pipeline Behaviors ile Cross-Cutting Concerns (Validasyon, Loglama, vb.)

## 🏗️ CQRS Nedir?

CQRS (Command Query Responsibility Segregation), okuma ve yazma işlemlerini birbirinden ayıran bir mimari pattern'dir.

- **Commands**: Veri değişikliği yapan işlemler (Create, Update, Delete)
- **Queries**: Veri okuma işlemleri (Get, List)

## 📁 Proje Yapısı

```
App.Application/
├── Abstractions/
│   ├── ICommand.cs              # Command interface'leri
│   ├── ICommandHandler.cs       # Command handler interface'leri
│   ├── IQuery.cs                # Query interface'i
│   └── IQueryHandler.cs         # Query handler interface'i
├── Behaviors/
│   └── ValidationBehavior.cs    # FluentValidation pipeline behavior
├── Features/
│   └── Receipts/
│       ├── Commands/
│       │   └── CreateReceipt/
│       │       ├── CreateReceiptCommand.cs
│       │       ├── CreateReceiptCommandHandler.cs
│       │       └── CreateReceiptCommandValidator.cs
│       ├── Queries/
│       │   └── GetReceiptById/
│       │       ├── GetReceiptByIdQuery.cs
│       │       ├── GetReceiptByIdQueryHandler.cs
│       │       └── GetReceiptByIdQueryValidator.cs
│       └── DTOs/
│           ├── ReceiptDto.cs
│           └── CreateReceiptDto.cs
└── Extensions/
    └── ServiceExtension.cs      # MediatR dependency injection
```

## 🚀 Kurulum

### 1. NuGet Package Yükleme

```bash
dotnet add package MediatR
```

### 2. Service Registration

`Program.cs` dosyasında MediatR servislerini kaydedin:

```csharp
using App.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

// MediatR ve diğer servisleri kaydet
builder.Services.AddServices();
```

### 3. ServiceExtension.cs

```csharp
public static class ServiceExtension
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // FLUENT VALIDATION
        services.AddValidatorsFromAssembly(typeof(ApplicationAssembly).Assembly);

        // MEDIATR
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(ApplicationAssembly).Assembly);
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return services;
    }
}
```

## 💡 Kullanım Örnekleri

### Command Oluşturma

```csharp
public sealed record CreateReceiptCommand(
    string StoreName,
    decimal TotalAmount,
    DateTime ReceiptDate
) : ICommand<CreateReceiptDto>;
```

### Command Handler

```csharp
public sealed class CreateReceiptCommandHandler : ICommandHandler<CreateReceiptCommand, CreateReceiptDto>
{
    public async Task<ServiceResult<CreateReceiptDto>> Handle(
        CreateReceiptCommand request, 
        CancellationToken cancellationToken)
    {
        // Business logic
        var receiptDto = new CreateReceiptDto
        {
            Id = Random.Shared.Next(1, 1000),
            StoreName = request.StoreName,
            TotalAmount = request.TotalAmount
        };

        return ServiceResult<CreateReceiptDto>.Success(receiptDto, HttpStatusCode.Created);
    }
}
```

### Query Oluşturma

```csharp
public sealed record GetReceiptByIdQuery(int Id) : IQuery<ReceiptDto>;
```

### Query Handler

```csharp
public sealed class GetReceiptByIdQueryHandler : IQueryHandler<GetReceiptByIdQuery, ReceiptDto>
{
    public async Task<ServiceResult<ReceiptDto>> Handle(
        GetReceiptByIdQuery request, 
        CancellationToken cancellationToken)
    {
        if (request.Id <= 0)
        {
            return ServiceResult<ReceiptDto>.Fail("Receipt not found", HttpStatusCode.NotFound);
        }

        var receiptDto = new ReceiptDto
        {
            Id = request.Id,
            StoreName = "Sample Store",
            TotalAmount = 99.99m,
            ReceiptDate = DateTime.Now.AddDays(-1),
            CreatedAt = DateTime.Now
        };

        return ServiceResult<ReceiptDto>.Success(receiptDto);
    }
}
```

### Controller'da Kullanım

```csharp
[ApiController]
[Route("api/[controller]")]
public class ReceiptsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReceiptsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetReceiptByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result.IsFail)
        {
            return StatusCode((int)result.Status, new { errors = result.ErrorMessage });
        }

        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReceiptRequest request)
    {
        var command = new CreateReceiptCommand(
            request.StoreName,
            request.TotalAmount,
            request.ReceiptDate
        );

        var result = await _mediator.Send(command);

        if (result.IsFail)
        {
            return StatusCode((int)result.Status, new { errors = result.ErrorMessage });
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }
}
```

## ✅ Validasyon

FluentValidation ile otomatik validasyon sağlanmaktadır. ValidationBehavior, MediatR pipeline'ında çalışır.

### Validator Örneği

```csharp
public sealed class CreateReceiptCommandValidator : AbstractValidator<CreateReceiptCommand>
{
    public CreateReceiptCommandValidator()
    {
        RuleFor(x => x.StoreName)
            .NotEmpty().WithMessage("Store name is required")
            .MaximumLength(200).WithMessage("Store name must not exceed 200 characters");

        RuleFor(x => x.TotalAmount)
            .GreaterThan(0).WithMessage("Total amount must be greater than 0");

        RuleFor(x => x.ReceiptDate)
            .NotEmpty().WithMessage("Receipt date is required")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Receipt date cannot be in the future");
    }
}
```

## 🧪 Test Örnekleri

### Başarılı Fişi Oluşturma

```bash
curl -X POST http://localhost:5258/api/receipts \
  -H "Content-Type: application/json" \
  -d '{"storeName": "Test Store", "totalAmount": 150.50, "receiptDate": "2024-12-14T10:00:00"}'
```

**Response (201 Created):**
```json
{
  "id": 606,
  "storeName": "Test Store",
  "totalAmount": 150.50
}
```

### Fişi ID ile Getirme

```bash
curl -X GET http://localhost:5258/api/receipts/5
```

**Response (200 OK):**
```json
{
  "id": 5,
  "storeName": "Sample Store",
  "totalAmount": 99.99,
  "receiptDate": "2025-12-14T10:43:36.6749213+00:00",
  "createdAt": "2025-12-15T10:43:36.6749745+00:00"
}
```

### Validasyon Hatası

```bash
curl -X POST http://localhost:5258/api/receipts \
  -H "Content-Type: application/json" \
  -d '{"storeName": "", "totalAmount": -10, "receiptDate": "2024-12-14T10:00:00"}'
```

**Response (400 Bad Request):**
```json
{
  "errors": [
    "Store name is required",
    "Total amount must be greater than 0"
  ]
}
```

## 📚 Kaynaklar

- [MediatR GitHub](https://github.com/jbogard/MediatR)
- [CQRS Pattern](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)
- [FluentValidation](https://docs.fluentvalidation.net/)

## 🎯 Sonuç

Bu implementasyon ile:
- ✅ Clean Architecture prensiplerine uygun bir yapı
- ✅ Test edilebilir ve bakımı kolay kod
- ✅ Otomatik validasyon
- ✅ Separation of Concerns
- ✅ SOLID prensipleri

elde edilmiştir.
