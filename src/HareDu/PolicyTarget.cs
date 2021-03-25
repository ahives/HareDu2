namespace HareDu
{
    public interface PolicyTarget
    {
        /// <summary>
        /// Specify the virtual host for which the policy will be defined for.
        /// </summary>
        /// <param name="name">Name of the virtual host being targeted.</param>
        void VirtualHost(string name);
    }
}