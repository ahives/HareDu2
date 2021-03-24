namespace HareDu
{
    using System;

    public interface UserConfigurator
    {
        /// <summary>
        /// Specify the type of access the corresponding user has.
        /// </summary>
        /// <param name="tags"></param>
        void WithTags(Action<UserAccessOptions> tags);
    }
}