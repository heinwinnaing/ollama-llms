using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OllamaAI;

const string llamaModel = "tinyllama";
try 
{
    var builder = Host.CreateApplicationBuilder();
    builder.Services.AddOllamaHttpClient(
        baseUrl: "http://localhost:11434",
        modelId: llamaModel);

    var app = builder.Build();
    var ollamaClient = app.Services.GetRequiredService<IOllamaHttpClient>();
    
    while (true)
    {
        await Console.Out.WriteAsync("Ask your question [Type 'q' to exist]: ");
        var input = await Console.In.ReadLineAsync();
        if (string.IsNullOrWhiteSpace(input))
            continue;
        else if (input == "q")
            break;

        var prompt = $"""
            {input}
            """;

        var results = await ollamaClient.GenerateStreamingResponseAsync(prompt);
        await Console.Out.WriteLineAsync("Response: ");
        foreach(var response in results)
        {
            await Console.Out.WriteAsync(response.Text);
        }
        await Console.Out.WriteLineAsync(Environment.NewLine);
    }
}
catch(Exception ex)
{
    Console.WriteLine(ex);
}