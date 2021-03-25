namespace HareDu
{
    public interface BindingCreateDefinition
    {
        /// <summary>
        /// Specify the binding source (i.e. queue/exchange).
        /// </summary>
        /// <param name="binding"></param>
        void Source(string binding);

        /// <summary>
        /// Specify the binding destination (i.e. queue/exchange).
        /// </summary>
        /// <param name="binding"></param>
        void Destination(string binding);

        /// <summary>
        /// Specify the binding type (i.e. queue or exchange).
        /// </summary>
        /// <param name="bindingType"></param>
        void Type(BindingType bindingType);
    }
}