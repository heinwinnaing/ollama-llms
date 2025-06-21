using System.Text.Json.Serialization;

namespace OllamaAI;

public class GenerateResponse
{
    [JsonPropertyName("model")]
    public string? Model { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("response")]
    public string? Text { get; set; }

    [JsonPropertyName("done")]
    public bool Done { get; set; }

    [JsonPropertyName("done_reason")]
    public string? DoneReason { get; set; }

    [JsonPropertyName("total_duration")]
    public ulong TotalDuration { get; set; }

    [JsonPropertyName("load_duration")]
    public ulong LoadDuration { get; set; }

    [JsonPropertyName("prompt_eval_count")]
    public ulong PromptEvalCount { get; set; }

    [JsonPropertyName("prompt_eval_duration")]
    public ulong PromptEvalDuration { get; set; }

    [JsonPropertyName("eval_count")]
    public ulong EvalCount { get; set; }

    [JsonPropertyName("eval_duration")]
    public ulong EvalDuration { get; set; }

    [JsonPropertyName("context")]
    public float[]? Context { get; set; }
}