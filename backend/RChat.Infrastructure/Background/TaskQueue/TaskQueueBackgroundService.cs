using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RChat.Application.Services.BackgroundTaskQueue;

namespace RChat.Infrastructure.Background.TaskQueue;

public class TaskQueueBackgroundService(IBackgroundTaskQueue taskQueue, ILogger<TaskQueueBackgroundService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var workItem = await taskQueue.DequeueAsync(stoppingToken);
            try
            {
                await workItem(stoppingToken); 
                
                logger.LogInformation($"Queue task {workItem.Method.Name} executed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Queued hosted service error");
            }
        }
    }
}