namespace HareDu
{
    using System;

    public interface BindingCreateAction
    {
        /// <summary>
        /// Specify the source, destination, and binding type.
        /// </summary>
        /// <param name="definition"></param>
        void Binding(Action<BindingCreateDefinition> definition);
        
        /// <summary>
        /// Specify how the binding should be configured.
        /// </summary>
        /// <param name="configuration"></param>
        void Configure(Action<BindingConfiguration> configuration);

        /// <summary>
        /// Specify the target for which the binding will be created.
        /// </summary>
        /// <param name="target">Define the location where the binding (i.e. virtual host) will be created</param>
        void Targeting(Action<BindingTarget> target);
    }
}