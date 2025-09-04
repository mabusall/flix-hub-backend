namespace Tasheer.Shared.Exceptions;

public class FeatureDisabledException : Exception
{
    public FeatureDisabledException(string message) : base(message)
    {
    }

    public FeatureDisabledException(string name, object key) : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }
}