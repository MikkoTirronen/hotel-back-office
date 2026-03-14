using Domain.Aggregates.BookingAggregate;
using Domain.Abstractions.Repositories;
using Application.DTOs;

namespace Application.Invoices.Commands.CreateInvoice;

public class CreateInvoiceHandler
{
    private readonly IInvoiceRepository _invoiceRepo;

    public CreateInvoiceHandler(IInvoiceRepository invoiceRepo)
    {
        _invoiceRepo = invoiceRepo;
    }

    public async Task<InvoiceDto> Handle(CreateInvoiceCommand command)
    {
        // Create invoice using aggregate rules
        var invoice = new Invoice(command.BookingId, command.Amount, DateTime.UtcNow);

        await _invoiceRepo.AddAsync(invoice);
        await _invoiceRepo.SaveAsync();

        return new InvoiceDto(invoice);
    }
}