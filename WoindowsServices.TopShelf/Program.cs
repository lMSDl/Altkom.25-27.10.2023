

using Topshelf;
using WoindowsServices.TopShelf;

HostFactory.Run(x =>
{
    x.Service<CustomService>();
    x.SetServiceName("CustomTopshelfService");
    x.SetDescription("viaTopshelf");

    x.EnableServiceRecovery(x =>
    {
        x
        .RestartService(TimeSpan.FromSeconds(1))
        .RestartService(TimeSpan.FromSeconds(3))
        .RestartService(TimeSpan.FromSeconds(5))
        .SetResetPeriod(5);
    });

    x.DependsOn("MicrosoftService");
    x.RunAsLocalSystem();

    x.StartAutomaticallyDelayed();
});