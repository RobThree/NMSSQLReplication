using Dapper.ColumnMapper;

namespace NMSSQLReplication
{
    /// <summary>
    /// Represents a Publication Threshold.
    /// </summary>
    public class PublicationThreshold
    {
        /// <summary>
        /// ID of the replication performance metric, which can be one of the following.
        /// </summary>
        [ColumnMapping("Metric_Id")]
        public Metric MetricId { get; set; }

        /// <summary>
        /// Name of the replication performance metric.
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// The threshold value of the performance metric.
        /// </summary>
        public int Value { get; set; }
        
        /// <summary>
        /// Is if an alert should be generated when the metric exceeds the defined threshold for this publication; a value of 1 indicates that an alert should be raised.
        /// </summary>
        public bool ShouldAlert { get; set; }
        
        /// <summary>
        /// Is if monitoring is enabled for this replication performance metric for this publication; a value of 1 indicates that monitoring is enabled.
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}
