using Fortinite_Project.Web.Services;

namespace Fortinite_Project.Web.Workers;
public class FortniteSyncWorker : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<FortniteSyncWorker> _logger;

    public FortniteSyncWorker(IServiceProvider services, ILogger<FortniteSyncWorker> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Iniciando Sincronizador do Fortnite...");

        using (var scope = _services.CreateScope())
        {
            var syncService = scope.ServiceProvider.GetRequiredService<IFortniteSyncService>();
            await syncService.RunInitialSyncAsync();
        }

        // Loop de Agendamento (Equivalente ao @Scheduled)
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);

            using (var scope = _services.CreateScope())
            {
                var syncService = scope.ServiceProvider.GetRequiredService<IFortniteSyncService>();
                await syncService.SyncShopAndNewStatusAsync();
            }
        }
    }
}