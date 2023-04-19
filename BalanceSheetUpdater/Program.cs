namespace BalanceSheetUpdater
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string inputPath = args[0];
            string keyPath = args[1];
            string spreadsheetId = "1SKVWjqeUeJM5bwEE3gckqDQvifd8U1p6SFG7fUizMJw";

            StatementReader statementReader = new(inputPath);
            Statement statement = statementReader.GenerateStatement();

            GoogleSheetClient client = new(keyPath, spreadsheetId, DateTime.Now.ToString("yyyy"));
            client.UploadStatement(statement);
        }
    }
}