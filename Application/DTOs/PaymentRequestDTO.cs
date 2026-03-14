namespace Application.DTOs;

public record RegisterPaymentDto(
    int InvoiceId,
    string Customer,
    decimal Amount,
    string? Method
);