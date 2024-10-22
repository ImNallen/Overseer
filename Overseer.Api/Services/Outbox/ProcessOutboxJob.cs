using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Overseer.Api.Features.Abstractions;
using Overseer.Api.Persistence;
using Overseer.Api.Services.Time;
using Overseer.Api.Utilities.Serialization;
using Quartz;

namespace Overseer.Api.Services.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxJob(
    IUnitOfWork unitOfWork,
    IPublisher publisher,
    IDateTimeProvider dateTimeProvider,
    IOptions<OutboxOptions> options,
    ILogger<ProcessOutboxJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Beginning processing of outbox messages");

        List<OutboxMessage> outboxMessages = await unitOfWork
            .OutboxMessages
            .Where(outboxMessage => outboxMessage.ProcessedOnUtc == null)
            .OrderBy(outboxMessage => outboxMessage.OccurredOnUtc)
            .Take(options.Value.BatchSize)
            .ToListAsync();

        foreach (OutboxMessage outboxMessage in outboxMessages)
        {
            logger.LogInformation("Processing outbox message {OutboxMessageId}", outboxMessage.Id);
            Exception? exception = null;
            try
            {
                IDomainEvent domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, SerializerSettings.Instance)!;

                await publisher.Publish(domainEvent);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing outbox message {OutboxMessageId}", outboxMessage.Id);

                exception = ex;
            }

            outboxMessage.Proccess(dateTimeProvider.UtcNow, exception);
        }

        await unitOfWork.SaveChangesAsync();
    }
}
