namespace BalanceSheetUpdater
{
    /// <summary>
    /// Transaction object
    /// </summary>
    internal class Transaction
    {
        /// <summary>
        /// Transaction date
        /// </summary>
        internal DateTime TransactionDate { get; set; }

        /// <summary>
        /// Transaction description
        /// </summary>
        internal string TransactionDescription { get; set; }

        /// <summary>
        /// Debit amount
        /// </summary>
        internal double Debit { get; set; }

        /// <summary>
        /// Credit amount
        /// </summary>
        internal double Credit { get; set; }

        /// <summary>
        /// Balance amount
        /// </summary>
        internal double Balance { get; set; }

        /// <summary>
        /// Difference
        /// </summary>
        internal double Amount => Credit - Debit;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="transactionDate">Datetime</param>
        /// <param name="transactionDescription">Description</param>
        /// <param name="debit">Debit</param>
        /// <param name="credit">Credit</param>
        /// <param name="balance">Balance</param>
        internal Transaction(DateTime transactionDate, string transactionDescription, double debit, double credit, double balance)
        {
            TransactionDate = transactionDate;
            TransactionDescription = transactionDescription;
            Debit = debit;
            Credit = credit;
            Balance = balance;
        }
    }
}