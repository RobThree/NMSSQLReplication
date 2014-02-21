
namespace NMSSQLReplication
{
    /// <summary>
    /// Defines the subscription types
    /// </summary>
    public enum SubscriptionType
    {
        /// <summary>Push</summary>
        Push = 0,
        /// <summary>Pull</summary>
        Pull = 1
    }

    /// <summary>
    /// Defines the subscription types
    /// </summary>
    public enum SubType
    {
        /// <summary>Push</summary>
        Push = SubscriptionType.Push,
        /// <summary>Pull</summary>
        Pull = SubscriptionType.Pull,
        /// <summary>Anonymous</summary>
        Anonymous = 2
    }
}
