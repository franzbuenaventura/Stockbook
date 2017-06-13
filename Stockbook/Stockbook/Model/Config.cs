using System;

namespace Stockbook.Model
{
    /// <summary>
    /// The config model.
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Gets or sets the last modified.
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// Gets or sets the last backup.
        /// </summary>
        public DateTime LastBackup { get; set; }

        /// <summary>
        /// Gets or sets the auto backup location.
        /// </summary>
        public string AutoBackupLocation{ get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is auto backup on.
        /// </summary>
        public bool IsAutoBackupOn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is auto backup on.
        /// </summary>
        public bool IsRetainHistoryOn { get; set; }

        /// <summary>
        /// Gets or sets the retain history count.
        /// </summary>
        public int RetainHistoryCount { get; set; }

        /// <summary>
        /// Gets or sets the time interval auto backup.
        /// </summary>
        public string TimeIntervalAutoBackup { get; set; }

        /// <summary>
        /// Gets or sets the company name.
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        public string Currency { get; set; }
    }
}
