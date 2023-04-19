using System.Configuration;

namespace BalanceSheetUpdater
{
    /// <summary>
    /// Program
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">Command line args</param>
        static void Main(string[] args)
        {
            // Create statement
            StatementReader statementReader = new(args[0]);
            Statement statement = statementReader.GenerateStatement();

            // Upload statement
            GoogleSheetClient client = new(ConfigurationManager.AppSettings.Get("KeyPath")!, ConfigurationManager.AppSettings.Get("SpreadsheetId")!, DateTime.Now.ToString("yyyy"));
            client.UploadStatement(statement);
        }
    }
}