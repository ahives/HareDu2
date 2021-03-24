namespace HareDu
{
    public interface UserPermissionsConfigurator
    {
        /// <summary>
        /// Specify the pattern of what users to configure the permissions for.
        /// </summary>
        /// <param name="pattern"></param>
        void UsingConfigurePattern(string pattern);

        /// <summary>
        /// Specify the pattern of what types of writes are allowable for this permission.
        /// </summary>
        /// <param name="pattern"></param>
        void UsingWritePattern(string pattern);

        /// <summary>
        /// Specify the pattern of what types of reads are allowable for this permission.
        /// </summary>
        /// <param name="pattern"></param>
        void UsingReadPattern(string pattern);
    }
}