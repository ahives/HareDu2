namespace HareDu.Model
{
    public interface AuthenticationMechanism
    {
        string Name { get; }

        string Description { get; }

        bool IsEnabled { get; }
    }
}