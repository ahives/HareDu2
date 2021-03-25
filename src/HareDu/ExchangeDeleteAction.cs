namespace HareDu
{
    using System;

    public interface ExchangeDeleteAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        void Exchange(string name);

        /// <summary>
        /// Specify the target for which the exchange will be deleted.
        /// </summary>
        /// <param name="target">Define the location where the exchange (i.e. virtual host) will be deleted</param>
        void Targeting(Action<ExchangeTarget> target);

        /// <summary>
        /// Specify the conditions for which the exchange can be deleted.
        /// </summary>
        /// <param name="condition"></param>
        void When(Action<ExchangeDeleteCondition> condition);
    }
}