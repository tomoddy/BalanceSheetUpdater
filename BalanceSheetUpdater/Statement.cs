namespace BalanceSheetUpdater
{
    /// <summary>
    /// Statement
    /// </summary>
    internal class Statement
    {
        /// <summary>
        /// List of transactions
        /// </summary>
        internal List<Transaction> Transactions { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Statement()
        {
            Transactions = new List<Transaction>();
        }

        /// <summary>
        /// Add transaction
        /// </summary>
        /// <param name="transactionDate">Transaction date</param>
        /// <param name="transactionDescription">Transactiond desciption</param>
        /// <param name="rawDebit">Debit amount</param>
        /// <param name="rawCredit">Credit amount</param>
        /// <param name="rawBalance">Balance</param>
        internal void AddTransaction(string transactionDate, string transactionDescription, string rawDebit, string rawCredit, string rawBalance)
        {
            double debit = FormatDouble(nameof(debit), rawDebit);
            double credit = FormatDouble(nameof(credit), rawCredit);
            double balance = FormatDouble(nameof(balance), rawBalance);
            Transactions.Add(new Transaction(DateTime.Parse(transactionDate), transactionDescription, debit, credit, balance));
        }

        /// <summary>
        /// Format double
        /// </summary>
        /// <param name="name">Name of variable</param>
        /// <param name="value">Value</param>
        /// <returns>Formatted value</returns>
        /// <exception cref="ArgumentException">Cannot format</exception>
        internal static double FormatDouble(string name, string value)
        {
            double retVal = 0;
            if (!string.IsNullOrEmpty(value) && !double.TryParse(value, out retVal))
                throw new ArgumentException($"Could not format \"{name}\"");
            else
                return retVal;
        }
    }
}