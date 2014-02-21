using System;

namespace NMSSQLReplication
{
    /// <summary>
    /// Defines the types of threshold warning.
    /// </summary>
    [Flags]
    public enum Warning
    {
        /// <summary>None</summary>
        None = 0x0,
        /// <summary>A subscription to a transactional publication has not been synchronized within the retention period threshold.</summary>
        Expiration = 0x1,
        /// <summary>The time taken to replicate data from a transactional Publisher to the Subscriber exceeds the threshold, in seconds.</summary>
        Latency = 0x2,
        /// <summary>A subscription to a merge publication has not been synchronized within the retention period threshold.</summary>
        MergeExpiration = 0x4,
        /// <summary>The time taken to complete synchronization of a merge subscription exceeds the threshold, in seconds, over a fast network connection.</summary>
        MergeFastRunduration = 0x8,
        /// <summary>The time taken to complete synchronization of a merge subscription exceeds the threshold, in seconds, over a slow or dial-up network connection.</summary>
        MergeSlowRunduration = 0x10,
        /// <summary>The delivery rate for rows during synchronization of a merge subscription has failed to maintain the threshold rate, in rows per second, over a fast network connection.</summary>
        MergeFastRunspeed = 0x20,
        /// <summary>The delivery rate for rows during synchronization of a merge subscription has failed to maintain the threshold rate, in rows per second, over a slow or dial-up network connection.</summary>
        MergeSlowRunspeed = 0x40
    }
}
