using System.Threading;
using System.Threading.Tasks;
using BaseApp.Web.Code.Scheduler.Queue.Workers;
using Microsoft.Extensions.Hosting;

namespace BaseApp.Web.Code.Scheduler.Queue;

public class SchedulerHostedService<T>(T workerService) : BackgroundService
    where T : IWorkerServiceBase
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();//To avoid blocking in the start core services code
        while (!stoppingToken.IsCancellationRequested)
        {
            await workerService.LoadAndProcessAsync();
            workerService.Delay();
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        workerService.WakeUp();
        await base.StopAsync(stoppingToken);
    }
}