
namespace NMSSQLReplication
{
    /// <summary>
    /// Defines the types of metrics available.
    /// </summary>
    public enum Metric
    {
        /// <summary>Monitors for imminent expiration of subscriptions to transactional publications.</summary>
        Expiration = 1,
        /// <summary>Monitors for the performance of subscriptions to transactional publications.</summary>
        Latency = 2,
        /// <summary>Monitors for imminent expiration of subscriptions to merge publications.</summary>
        MergeExpiration = 4,
        /// <summary>Monitors the duration of merge synchronizations over low-bandwidth (dial-up) connections.</summary>
        MergeSlowRunduration = 5,
        /// <summary>Monitors the duration of merge synchronizations over high-bandwidth (LAN) connections.</summary>
        MergeFastRunduration = 6,
        /// <summary>Monitors the synchronization rate of merge synchronizations over high-bandwidth (LAN) connections.</summary>
        MergeFastRunspeed = 7,
        /// <summary>Monitors the synchronization rate of merge synchronizations over low-bandwidth (dial-up) connections.</summary>
        MergeSlowRunspeed = 8
    }
}
