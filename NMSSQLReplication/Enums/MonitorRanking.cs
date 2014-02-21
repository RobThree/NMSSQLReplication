
namespace NMSSQLReplication
{
    /// <summary>
    /// Defines the values for Monitor Ranking
    /// </summary>
    enum MonitorRankingCommonValues
    {
        /// <summary>Error</summary>
        Error = 60,
        /// <summary>Warning: performance critical</summary>
        PerformanceCriticalWarning = 56,
        /// <summary>Warning: subscription uninitialized</summary>
        SubscriptionUninitializedWarning = 50,
        /// <summary>Retrying failed command</summary>
        RetryingFailedCommand = 40,
    }

    /// <summary>
    /// Defines the values for Monitor Ranking for transactional publications.
    /// </summary>
    public enum TransactionalPublicationMonitorRanking
    {
        /// <summary>Error</summary>
        Error = MonitorRankingCommonValues.Error,
        /// <summary>Warning: performance critical</summary>
        PerformanceCriticalWarning = MonitorRankingCommonValues.PerformanceCriticalWarning,
        /// <summary>Warning: expiring soon or expired</summary>
        ExpiringSoonOrExpiredWarning = 52,
        /// <summary>Warning: subscription uninitialized</summary>
        SubscriptionUninitializedWarning = MonitorRankingCommonValues.SubscriptionUninitializedWarning,
        /// <summary>Retrying failed command</summary>
        RetryingFailedCommand = MonitorRankingCommonValues.RetryingFailedCommand,
        /// <summary>Not running (success)</summary>
        NotRunning = 30,
        /// <summary>Running (starting, running, or idle)</summary>
        RunningStartingOrIdle = 20
    }

    /// <summary>
    /// Defines the values for Monitor Ranking for merge publications.
    /// </summary>
    public enum MergePublicationMonitorRanking
    {
        /// <summary>Error</summary>
        Error = MonitorRankingCommonValues.Error,
        /// <summary>Warning: performance critical</summary>
        PerformanceCriticalWarning = MonitorRankingCommonValues.PerformanceCriticalWarning,
        /// <summary>Warning: long-running merge</summary>
        LongRunningMergeWarning = 54,
        /// <summary>Warning: expiring soon</summary>
        ExpiringSoonWarning = 52,
        /// <summary>Warning: subscription uninitialized</summary>
        SubscriptionUninitializedWarning = MonitorRankingCommonValues.SubscriptionUninitializedWarning,
        /// <summary>Retrying failed command</summary>
        RetryingFailedCommand = MonitorRankingCommonValues.RetryingFailedCommand,
        /// <summary>Running (starting, running, or idle)</summary>
        Running = 30,
        /// <summary>Not running (success)</summary>
        NotRunning = 20
    }
}
