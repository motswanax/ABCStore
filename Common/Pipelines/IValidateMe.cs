namespace Common.Pipelines;

/// <summary>
/// This interface is used to mark requests that should pass through the validation pipeline.
/// It saves time by preventing unnecessary validation of MediatR requests.
/// </summary>
public interface IValidateMe
{
}
