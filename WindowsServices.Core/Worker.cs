namespace WindowsServices.Core
{
    public class Worker : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await WriteMessage("Starting...\n");

            while (!stoppingToken.IsCancellationRequested)
            {
                await WriteMessage("Working...\n");
                await Task.Delay(5000, stoppingToken);
            }
            await WriteMessage("Stopping...\n");
        }



        private readonly string? _filename = "c:\\CustomService\\CoreService.txt";
        private Task WriteMessage(string message)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filename));
            return File.AppendAllTextAsync(_filename, message);
        }
    }
}