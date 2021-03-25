namespace HareDu
{
    using System;

    public interface BindingDeleteAction
    {
        /// <summary>
        /// Specify the source, destination, and binding type.
        /// </summary>
        /// <param name="definition"></param>
        void Binding(Action<BindingDeleteDefinition> definition);

        /// <summary>
        /// Specify the target for which the binding will be deleted.
        /// </summary>
        /// <param name="target">Define the location where the binding (i.e. virtual host) will be deleted</param>
        void Targeting(Action<BindingTarget> target);
    }
}