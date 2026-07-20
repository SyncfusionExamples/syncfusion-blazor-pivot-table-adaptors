namespace Pivot_GraphQLAdaptor.Models
{
    public class GraphQLQuery
    {
        /// <summary>
        /// Retrieves all expense records and returns them along with the total record count.
        /// </summary>
        /// <param name="dataManager">The data manager request input containing query parameters.</param>
        /// <returns>An instance of <see cref="ExpenseRecordDataResponse"/> containing the expense records and count.</returns>
        public ExpenseRecordDataResponse GetExpenseRecordData(DataManagerRequestInput dataManager)
        {
            List<ExpenseRecord> dataSource = ExpenseRecord.GetAllRecords();

            return new ExpenseRecordDataResponse
            {
                Count = dataSource.Count,
                Result = dataSource
            };
        }
    }

    /// <summary>
    /// Represents the response structure for expense record queries.
    /// </summary>
    public class ExpenseRecordDataResponse
    {
        /// <summary>
        /// Gets or sets the total count of records.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the list of expense record records.
        /// </summary>
        public List<ExpenseRecord> Result { get; set; } = new List<ExpenseRecord>();
    }
}