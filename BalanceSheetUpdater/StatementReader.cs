namespace BalanceSheetUpdater
{
    /// <summary>
    /// Statement reader
    /// </summary>
    internal class StatementReader
    {
        /// <summary>
        /// Statement path
        /// </summary>
        private string Path { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="path"></param>
        internal StatementReader(string path)
        {
            Path = path;
        }

        /// <summary>
        /// Generate statement
        /// </summary>
        /// <returns>Statement</returns>
        internal Statement GenerateStatement()
        {
            // Read file and remove first line
            Statement retVal = new();
            List<string> lines = File.ReadAllLines(Path).ToList();
            lines.RemoveAt(0);

            // Add cells to line
            foreach (string line in lines)
            {
                List<string> cells = line.Split(',').ToList();
                retVal.AddTransaction(cells[0], cells[4], cells[5], cells[6], cells[7]);
            }
            return retVal;
        }
    }
}