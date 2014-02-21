
namespace NMSSQLReplication
{
    /// <summary>
    /// Defines the filtering modes available when returning subscription monitoring information.
    /// </summary>
    public enum Mode
    {
        /// <summary>Returns all subscriptions.</summary>
        All = 0,
        /// <summary>Returns only subscriptions with errors.</summary>
        SubscriptionsWithErrors = 1,
        /// <summary>Returns only subscriptions that have generated threshold metric warnings.</summary>
        SubscriptionsWithThresholdMetricWarnings = 2,
        /// <summary>Returns only subscriptions that either have errors or have generated threshold metric warnings.</summary>
        SubscriptionsWithErrorsOrThresholdMetricWarnings = 3,
        /// <summary>Returns the top 25 worst performing subscriptions.</summary>
        Top25WorstPerformingSubscriptions = 4,
        /// <summary>Returns the top 50 worst performing subscriptions.</summary>
        Top50WorstPerformingSubscriptions = 5,
        /// <summary>Returns only subscriptions that are currently being synchronized.</summary>
        CurrentlySynchronizingSubscriptions = 6,
        /// <summary>Returns only subscriptions that are not currently being synchronized.</summary>
        CurrentlyNotSynchronizingSubscription = 7
    }
}
