namespace HareDu
{
    using System;

    public interface ScopedParameterDeleteAction
    {
        /// <summary>
        /// Specify the name of the scoped parameter.
        /// </summary>
        /// <param name="name"></param>
        void Parameter(string name);

        /// <summary>
        /// Specify the targeted component and virtual host.
        /// </summary>
        /// <param name="target"></param>
        void Targeting(Action<ScopedParameterTarget> target);
    }
}