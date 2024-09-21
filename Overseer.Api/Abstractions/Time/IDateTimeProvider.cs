namespace Overseer.Api.Abstractions.Time;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }

    public DateTime Now { get; }

    public DateOnly Today { get; }
}
