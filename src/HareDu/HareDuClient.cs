namespace HareDu
{
    using System;

    public interface HareDuClient
    {
        /// <summary>
        /// Creates a new instance of object implemented by T, which encapsulates a group of resources (e.g. Virtual Host, Exchange, Queue, User, etc.)
        /// that are exposed by the RabbitMQ server via its REST API.
        /// </summary>
        /// <typeparam name="TResource">Interface that derives from base interface ResourceClient.</typeparam>
        /// <param name="userCredentials">Username and password of valid user on a RabbitMQ server.</param>
        /// <returns>An interface of resources available on a RabbitMQ server.</returns>
        TResource Factory<TResource>(Action<UserCredentials> userCredentials)
            where TResource : Resource;
        
        /// <summary>
        /// 
        /// </summary>
        void CancelPendingRequest();
    }
}