namespace HareDu
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using Model;

    public interface Policy :
        BrokerObject
    {
        /// <summary>
        /// Returns all policies on the current RabbitMQ node.
        /// </summary>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<ResultList<PolicyInfo>> GetAll(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Creates the specified policy on the target virtual host.
        /// </summary>
        /// <param name="action">Describes how the policy will be created.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result> Create(Action<PolicyCreateAction> action, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Delete the specified policy on the target virtual host.
        /// </summary>
        /// <param name="action">Describes how the policy will be deleted.</param>
        /// <param name="cancellationToken">Token used cancel the current thread</param>
        /// <returns>Asynchronous task of <see cref="Result{T}"/></returns>
        Task<Result> Delete(Action<PolicyDeleteAction> action, CancellationToken cancellationToken = default);

        /// <summary>
        /// Creates the specified policy on the target RabbitMQ virtual host.
        /// </summary>
        /// <param name="policy">The name of the policy.</param>
        /// <param name="pattern">The pattern for which the policy is to be applied.</param>
        /// <param name="vhost">The name of the virtual host.</param>
        /// <param name="configurator">Describes how the policy will be created by setting arguments through set methods.</param>
        /// <param name="appliedTo">The type of broker objects to apply the policy to.</param>
        /// <param name="priority"></param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Create(string policy, string pattern, string vhost, Action<PolicyConfigurator> configurator,
            PolicyAppliedTo appliedTo = PolicyAppliedTo.All, int priority = 0, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified policy on the target RabbitMQ virtual host.
        /// </summary>
        /// <param name="policy">The name of the policy.</param>
        /// <param name="vhost">The name of the virtual host.</param>
        /// <param name="cancellationToken">Token used to cancel the operation running on the current thread.</param>
        /// <returns></returns>
        Task<Result> Delete(string policy, string vhost, CancellationToken cancellationToken = default);
    }
}