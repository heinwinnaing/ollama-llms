using System.Net.Mime;
using System.Text.Json;
using System.Text;

namespace OllamaAI;

internal class OllamaHttpClient: IOllamaHttpClient
{
    private readonly string ollamaModel;
    private readonly string embeddingModel;
    private readonly HttpClient httpClient = new();
    public OllamaHttpClient(string baseUrl, string modelName)
    {
        ollamaModel = modelName;
        embeddingModel = modelName;
        httpClient.BaseAddress = new Uri(baseUrl);
    }

    public async Task<float[]> GenerateEmbeddings(string text,
        string? model = null,
        CancellationToken ctn = default)
    {
        var uri = "/api/embeddings";
        var payload = new
        {
            model = model ?? ollamaModel,
            prompt = text,
        };

        var content = new StringContent(
            content: JsonSerializer.Serialize(payload),
            encoding: Encoding.UTF8,
            mediaType: MediaTypeNames.Application.Json);

        var response = await httpClient.PostAsync(uri, content);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        _ = doc.RootElement.TryGetProperty("embedding", out JsonElement embedding);
        float[] result = new float[embedding.GetArrayLength()];
        int i = 0;
        foreach (var item in embedding.EnumerateArray())
            result[i++] = item.GetSingle();

        return result;
    }

    public async Task<GenerateResponse> GenerateResponseAsync(string prompt,
        CancellationToken ctn = default)
    {
        var uri = "/api/generate";
        var payload = new
        {
            prompt = prompt,
            model = ollamaModel,
            stream = false
        };
        var content = new StringContent(
            content: JsonSerializer.Serialize(payload),
            encoding: Encoding.UTF8,
            mediaType: MediaTypeNames.Application.Json);

        var response = await httpClient.PostAsync(uri, content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<GenerateResponse>(json);
        return result!;
    }

    public async Task<IEnumerable<GenerateResponse>> GenerateStreamingResponseAsync(string prompt,
        CancellationToken ctn = default)
    {
        var uri = "/api/generate";
        var payload = new
        {
            prompt = prompt,
            model = ollamaModel,
            stream = true
        };
        var content = new StringContent(
            content: JsonSerializer.Serialize(payload),
            encoding: Encoding.UTF8,
            mediaType: MediaTypeNames.Application.Json);

        var response = await httpClient.PostAsync(uri, content);
        response.EnsureSuccessStatusCode();

        return ReadLines<GenerateResponse>(response);
    }

    private IEnumerable<T> ReadLines<T>(HttpResponseMessage response) where T : class
    {
        using Stream st = response.Content.ReadAsStream();
        using StreamReader reader = new StreamReader(st);
        string? text;
        while ((text = reader.ReadLine()) is not null)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                var result = JsonSerializer.Deserialize<T>(text);
                yield return result!;
            }
        }
    }
}
