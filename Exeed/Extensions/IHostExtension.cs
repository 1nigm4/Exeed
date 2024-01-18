using Exeed.Managers;

namespace Exeed.Extensions
{
    public static class IHostExtension
    {
        public static async Task RunApplicationAsync(this IHost app)
        {
            var cts = new CancellationTokenSource();
            var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

            lifetime.ApplicationStarted.Register(() =>
            {
                var scope = app.Services.CreateScope();
                var eventManager = scope.ServiceProvider.GetRequiredService<EventManager>().CheckWinnersAsync();
            });

            lifetime.ApplicationStopping.Register(() =>
            {
                cts.Cancel();
            });

            await app.RunAsync(cts.Token);
        }
    }
}
