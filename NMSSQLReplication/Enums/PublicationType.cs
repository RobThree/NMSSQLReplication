
namespace NMSSQLReplication
{
    /// <summary>
    /// Defines the types of publications.
    /// </summary>
    public enum PublicationType
    {
        /// <summary>Return any type of publication.</summary>
        /// <remarks>Never returned, only used in queries.</remarks>
        Any = -1,
        /// <summary>Transactional publication.</summary>
        Transactional = 0,
        /// <summary>Snapshot publication</summary>
        Snapshot = 1,
        /// <summary>Merge publication</summary>
        Merge = 2
    }
}
