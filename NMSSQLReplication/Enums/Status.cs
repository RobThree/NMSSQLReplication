
namespace NMSSQLReplication
{
    /// <summary>
    /// Defines the replication agent statuses.
    /// </summary>
    public enum Status
    {
        /// <summary>Started</summary>
        Started = 1,
        /// <summary>Succeeded</summary>
        Succeeded = 2,
        /// <summary>In progress</summary>
        InProgress = 3,
        /// <summary>Idle</summary>
        Idle = 4,
        /// <summary>Retrying</summary>
        Retrying = 5,
        /// <summary>Failed</summary>
        Failed = 6
    }
}
