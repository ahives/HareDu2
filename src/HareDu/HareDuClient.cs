namespace HareDu
{
    using System;

    public interface HareDuClient
    {
        TResource Factory<TResource>(Action<UserCredentials> userCredentials)
            where TResource : Resource;
        
        void CancelPendingRequest();
    }
}