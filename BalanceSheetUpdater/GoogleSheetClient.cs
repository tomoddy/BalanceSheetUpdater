using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;

namespace BalanceSheetUpdater
{
    /// <summary>
    /// Google sheet clinet
    /// </summary>
    internal class GoogleSheetClient
    {
        /// <summary>
        /// Sheets service
        /// </summary>
        private SheetsService SheetsService { get; set; }

        /// <summary>
        /// Spreadsheet id
        /// </summary>
        private string SpreadsheetId { get; set; }

        /// <summary>
        /// Sheet name
        /// </summary>
        private string SheetName { get; set; }

        /// <summary>
        /// Start date reference
        /// </summary>
        private static DateTime StartDate => new(1900, 1, 1);

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="credentialFile">Credential file path</param>
        /// <param name="spreadsheetId">Spreadsheet id</param>
        /// <param name="sheetName">Sheet name</param>
        internal GoogleSheetClient(string credentialFile, string spreadsheetId, string sheetName)
        {
            GoogleCredential credential = GoogleCredential.FromStream(new FileStream(credentialFile, FileMode.Open)).CreateScoped(new string[] { SheetsService.Scope.Spreadsheets });
            SheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "BalanceSheetUpdater"
            });
            SpreadsheetId = spreadsheetId;
            SheetName = sheetName;
        }

        /// <summary>
        /// Upload statement
        /// </summary>
        /// <param name="statement">Statement</param>
        internal void UploadStatement(Statement statement)
        {
            // Get row index
            int lastRow = SheetsService.Spreadsheets.Values.Get(SpreadsheetId, $"{SheetName}!A:A").Execute().Values.Count;

            // Create startpoint
            GridCoordinate startPoint = new()
            {
                ColumnIndex = 0,
                RowIndex = lastRow,
                SheetId = GetSheetId()
            };

            // Iterate through transactions
            List<RowData> rowData = new();
            foreach (Transaction transaction in statement.Transactions)
            {
                // Create row and list of cells
                List<CellData> cells = new()
                {
                    GenerateCellData(transaction.TransactionDate),
                    new CellData { UserEnteredValue = new ExtendedValue { FormulaValue = $"=MONTH(A{++lastRow})" }, UserEnteredFormat = new CellFormat { } },
                    new CellData(),
                    GenerateCellData(transaction.TransactionDescription),
                    GenerateCellData(transaction.Amount),
                    GenerateCellData(transaction.Balance)
                };
                rowData.Add(new() { Values = cells });
            }

            // Create request
            Request request = new()
            {
                UpdateCells = new UpdateCellsRequest
                {
                    Start = startPoint,
                    Fields = "*",
                    Rows = rowData
                }
            };

            // Create batch and send request
            BatchUpdateSpreadsheetRequest requests = new() { Requests = new List<Request>() { request } };
            SheetsService.Spreadsheets.BatchUpdate(requests, SpreadsheetId).Execute();
        }

        /// <summary>
        /// Get sheet id
        /// </summary>
        /// <returns>Sheet id</returns>
        private int GetSheetId()
        {
            Spreadsheet spreadsheet = SheetsService.Spreadsheets.Get(SpreadsheetId).Execute();
            Sheet sheet = spreadsheet.Sheets.FirstOrDefault(s => s.Properties.Title == SheetName)!;
            return (int)sheet.Properties.SheetId!;
        }

        /// <summary>
        /// Generate cell data
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="value">Value</param>
        /// <returns>Cell data</returns>
        /// <exception cref="ArgumentException">Could not format</exception>
        private static CellData GenerateCellData<T>(T value)
        {
            CellData cellData = new();
            switch (value)
            {
                case double d:
                    cellData.UserEnteredValue = new ExtendedValue { NumberValue = d };
                    cellData.UserEnteredFormat = new CellFormat { NumberFormat = new NumberFormat { Type = "CURRENCY" } };
                    return cellData;
                case DateTime t:
                    cellData.UserEnteredValue = new ExtendedValue { NumberValue = (t - StartDate).TotalDays + 2 };
                    cellData.UserEnteredFormat = new CellFormat { NumberFormat = new NumberFormat { Type = "DATE" } };
                    return cellData;
                case string s:
                    cellData.UserEnteredValue = new ExtendedValue { StringValue = s! };
                    cellData.UserEnteredFormat = new CellFormat { TextFormat = new TextFormat() };
                    return cellData;
            }
            throw new ArgumentException("Could not parse");
        }
    }
}