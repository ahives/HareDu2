namespace HareDu
{
    public interface BindingDeleteDefinition
    {
        /// <summary>
        /// Specify the name of the binding.
        /// </summary>
        /// <param name="name"></param>
        void Name(string name);
        
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