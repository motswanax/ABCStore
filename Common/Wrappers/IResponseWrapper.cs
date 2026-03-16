using System.Text.Json.Serialization;

namespace Common.Wrappers;
public interface IResponseWrapper
{
    List<string> Messages { get; set; }
    bool IsSuccessful { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StackTrace { get; set; }
}

public interface IResponseWrapper<out T> : IResponseWrapper
{
    T? Data { get; }
}
