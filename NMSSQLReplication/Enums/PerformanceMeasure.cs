
namespace NMSSQLReplication
{
    /// <summary>
    /// Defines the values for Transactional Replication performance quality indicator.
    /// </summary>
    public enum TransactionalPerformanceQuality
    {
        /// <summary>No indication available</summary>
        None = 0,
        /// <summary>Excellent</summary>
        Excellent = 1,
        /// <summary>Good</summary>
        Good = 2,
        /// <summary>Fair</summary>
        Fair = 3,
        /// <summary>Poor</summary>
        Poor = 4,
        /// <summary>Critical</summary>
        Critical = 5
    }

    /// <summary>
    /// Defines the values for Merge Replication performance quality indicator.
    /// </summary>
    public enum MergePerformanceQuality
    {
        /// <summary>No indication available</summary>
        None = TransactionalPerformanceQuality.None,
        /// <summary>Excellent</summary>
        Excellent = TransactionalPerformanceQuality.Excellent,
        /// <summary>Good</summary>
        Good = TransactionalPerformanceQuality.Good,
        /// <summary>Fair</summary>
        Fair = TransactionalPerformanceQuality.Fair,
        /// <summary>Poor</summary>
        Poor = TransactionalPerformanceQuality.Poor
    }
}
