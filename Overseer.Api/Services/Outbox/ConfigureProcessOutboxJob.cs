using Microsoft.Extensions.Options;
using Quartz;

namespace Overseer.Api.Services.Outbox;

internal sealed class ConfigureProcessOutboxJob(IOptions<OutboxOptions> options) : IConfigureOptions<QuartzOptions>
{
    private readonly OutboxOptions _options = options.Value;

    public void Configure(QuartzOptions options) =>
        options
            .AddJob<ProcessOutboxJob>(c => c.WithIdentity(nameof(ProcessOutboxJob)))
            .AddTrigger(c =>
                c.ForJob(nameof(ProcessOutboxJob))
                    .WithSimpleSchedule(s =>
                        s.WithIntervalInSeconds(_options.IntervalInSeconds)
                            .RepeatForever()));
}
