using ZeroTrashService;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService(o =>
{
    o.ServiceName = "ZeroTrashService";
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();