using Dapper.ColumnMapper;

namespace NMSSQLReplication
{
    /// <summary>
    /// Represents Pending Transactional Commands.
    /// </summary>
    public class PendingTransactionalCommands
    {
        /// <summary>
        /// The number of commands that are pending for the subscription.
        /// </summary>
        [ColumnMapping("PendingCmdCount")]
        public int PendingCommandCount { get; set; }

        /// <summary>
        /// Estimate of the number of seconds required to deliver all of the pending commands to the Subscriber.
        /// </summary>
        public int EstimatedProcessTime { get; set; }
    }
}
