namespace HareDu
{
    public interface UserAccessOptions
    {
        /// <summary>
        /// Specify that the corresponding user has no associated privileges.
        /// </summary>
        void None();
        
        /// <summary>
        /// Specify that the corresponding user has administrative privileges.
        /// </summary>
        void Administrator();
        
        /// <summary>
        /// Specify that the corresponding user has monitoring privileges.
        /// </summary>
        void Monitoring();
        
        /// <summary>
        /// Specify that the corresponding user has management privileges.
        /// </summary>
        void Management();

        /// <summary>
        /// Specify that the corresponding user has policymaker privileges.
        /// </summary>
        void PolicyMaker();

        /// <summary>
        /// Specify that the corresponding user has impersonator privileges.
        /// </summary>
        void Impersonator();
    }
}