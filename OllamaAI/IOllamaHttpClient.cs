namespace OllamaAI;

public interface IOllamaHttpClient
{
    Task<float[]> GenerateEmbeddings(string text, string? model = null, CancellationToken ctn = default);
    Task<GenerateResponse> GenerateResponseAsync(string prompt, CancellationToken ctn = default);
    Task<IEnumerable<GenerateResponse>> GenerateStreamingResponseAsync(string prompt, CancellationToken ctn = default);
}
