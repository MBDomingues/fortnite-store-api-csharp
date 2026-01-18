namespace Fortinite_Project.Web.Services;

public interface IFortniteSyncService
{
    Task RunInitialSyncAsync();
    Task SyncAllBaseCosmeticsAsync();
    Task SyncShopAndNewStatusAsync();
}