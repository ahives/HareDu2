namespace HareDu
{
    public interface QueueResource
    {
        BindingResource Binding { get; }

        void GetAll();
    }
}