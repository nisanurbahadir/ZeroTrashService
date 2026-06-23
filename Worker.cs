using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ZeroTrashService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private const string TrashPath = @"C:\$Recycle.Bin";

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ZeroTrash Core Engine started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (Directory.Exists(TrashPath))
                {
                    var subDirs = Directory.GetDirectories(TrashPath);
                    foreach (var dir in subDirs)
                    {
                        try
                        {
                            Directory.Delete(dir, true);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                    _logger.LogInformation("Recycle bin directory purged.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Purge execution failed: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}