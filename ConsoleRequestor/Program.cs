// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

partial class  Program
{   
    static async Task Main()
    {
        var httpClient = new HttpClient();
        int totalRequests = 10000;
        int maxParallelism = 1000; // Adjust based on server capacity

        string logFilePath = "stress_test_log.txt";
        using var logWriter = new StreamWriter(logFilePath, append: true);
        var semaphore = new SemaphoreSlim(1, 1); // Untuk sinkronisasi akses ke logWriter
        var stopwatch = Stopwatch.StartNew();

        await Parallel.ForEachAsync(Enumerable.Range(1, totalRequests), new ParallelOptions { MaxDegreeOfParallelism = maxParallelism }, async (id, _) =>
        {
            var requestStopwatch = Stopwatch.StartNew();
            try
            {
                var response = await httpClient.GetAsync($"https://localhost:7147/proxy/Butch");
                requestStopwatch.Stop();

                string log = $"Request {id}: Status {response.StatusCode}, Time {requestStopwatch.ElapsedMilliseconds} ms";
                Console.WriteLine(log);
                await semaphore.WaitAsync();
                try
                {
                    await logWriter.WriteLineAsync(log);
                }
                finally
                {
                    semaphore.Release();
                }
            }
            catch (Exception ex)
            {
                requestStopwatch.Stop();
                string errorLog = $"Request {id} failed: {ex.Message}, Time {requestStopwatch.ElapsedMilliseconds} ms";
                Console.WriteLine(errorLog);
                await semaphore.WaitAsync();
                try
                {
                    await logWriter.WriteLineAsync(errorLog);
                }
                finally
                {
                    semaphore.Release();
                }
            }
        });

        stopwatch.Stop();
        string summary = $"Completed {totalRequests} requests in {stopwatch.ElapsedMilliseconds} ms!";
        Console.WriteLine(summary);
        await logWriter.WriteLineAsync(summary);

        Console.WriteLine($"Logs saved to {logFilePath}");
        Console.ReadLine();
    }
}
