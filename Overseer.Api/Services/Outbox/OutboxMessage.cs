namespace Overseer.Api.Services.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; init; }

    public string Type { get; init; } = null!;

    public string Content { get; init; } = null!;

    public DateTime OccurredOnUtc { get; init; }

    public DateTime? ProcessedOnUtc { get; private set; }

    public string? Error { get; private set; }

    public OutboxMessage Proccess(DateTime dateTimeUtc, Exception? exception)
    {
        ProcessedOnUtc = dateTimeUtc;
        Error = exception?.ToString() ?? null;

        return this;
    }
}
