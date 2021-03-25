namespace HareDu
{
    using System;

    public interface ScopedParameterCreateAction<T>
    {
        /// <summary>
        /// Specify the name of the scoped parameter.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void Parameter(string name, T value);

        /// <summary>
        /// Specify the targeted component and virtual host.
        /// </summary>
        /// <param name="target"></param>
        void Targeting(Action<ScopedParameterTarget> target);
    }
}