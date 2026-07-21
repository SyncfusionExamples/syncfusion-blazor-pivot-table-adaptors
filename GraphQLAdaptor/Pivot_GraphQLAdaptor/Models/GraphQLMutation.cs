namespace Pivot_GraphQLAdaptor.Models
{
    public class GraphQLMutation
    {
        /// <summary>
        /// Creates a new expense record.
        /// </summary>
        public ExpenseRecord CreateExpense(ExpenseRecord record, int index, string action, [GraphQLType(typeof(AnyType))] IDictionary<string, object> additionalParameters)
        {
            var expenses = ExpenseRecord.GetAllRecords();

            // Generate ExpenseId as Prefix + (max numeric + 1) when adding
            // Determine prefix from existing IDs (letters at start), fallback to "EXP"
            if (string.IsNullOrWhiteSpace(record.ExpenseId))
            {
                record.ExpenseId = GenerateExpenseId(expenses);
            }

            if (index >= 0 && index <= expenses.Count)
            {
                expenses.Insert(index, record);
            }
            else
            {
                expenses.Add(record);
            }

            return record;
        }

        /// <summary>
        /// Updates an existing expense record.
        /// </summary>
        public ExpenseRecord UpdateExpense(ExpenseRecord record, string action, string primaryColumnName, string primaryColumnValue, [GraphQLType(typeof(AnyType))] IDictionary<string, object> additionalParameters)
        {
            var expenses = ExpenseRecord.GetAllRecords();
            var existingExpense = expenses.FirstOrDefault(x => x.ExpenseId == primaryColumnValue);

            if (existingExpense != null)
            {
                UpdateExpenseProperties(existingExpense, record);
            }

            return existingExpense;
        }

        /// <summary>
        /// Deletes an expense record.
        /// </summary>
        public bool DeleteExpense(string primaryColumnValue, [GraphQLType(typeof(AnyType))] IDictionary<string, object> additionalParameters)
        {
            var expenses = ExpenseRecord.GetAllRecords();
            var expenseToDelete = expenses.FirstOrDefault(x => x.ExpenseId == primaryColumnValue);

            if (expenseToDelete != null)
            {
                expenses.Remove(expenseToDelete);
                return true;
            }

            return false;
        }


        public List<ExpenseRecord> BatchUpdate(List<ExpenseRecord>? changed, List<ExpenseRecord>? added,
            List<ExpenseRecord>? deleted, string action, string primaryColumnName,
            [GraphQLType(typeof(AnyType))] IDictionary<string, object> additionalParameters, int? dropIndex)
        {
            var expenses = ExpenseRecord.GetAllRecords();

            // Update existing expenses
            if (changed != null)
            {
                foreach (var changedItem in changed)
                {
                    var existing = expenses.FirstOrDefault(e => e.ExpenseId == changedItem.ExpenseId);
                    if (existing != null)
                    {
                        UpdateExpenseProperties(existing, changedItem);
                    }
                }
            }

            // Add new expenses
            if (added != null)
            {
                foreach (var newItem in added)
                {
                    if (string.IsNullOrWhiteSpace(newItem.ExpenseId))
                    {
                        newItem.ExpenseId = GenerateExpenseId(expenses);
                    }

                    if (dropIndex.HasValue && dropIndex >= 0 && dropIndex <= expenses.Count)
                        expenses.Insert(dropIndex.Value, newItem);
                    else
                        expenses.Add(newItem);
                }
            }

            // Delete expenses
            if (deleted != null)
            {
                foreach (var del in deleted)
                {
                    var toRemove = expenses.FirstOrDefault(e => e.ExpenseId == del.ExpenseId);
                    if (toRemove != null) expenses.Remove(toRemove);
                }
            }

            return expenses;
        }

        /// <summary>
        /// Generates a unique ExpenseId by extracting prefix from existing IDs and incrementing the sequence number.
        /// </summary>
        /// <param name="expenses">The list of existing expense records.</param>
        /// <returns>A newly generated unique ExpenseId.</returns>
        private string GenerateExpenseId(List<ExpenseRecord> expenses)
        {
            string detectedPrefix = "EXP";
            var firstWithLetters = expenses
                .Select(e => e.ExpenseId)
                .FirstOrDefault(id => !string.IsNullOrWhiteSpace(id) && char.IsLetter(id[0]));
            if (!string.IsNullOrWhiteSpace(firstWithLetters))
            {
                // Extract leading letters as prefix
                int i = 0;
                while (i < firstWithLetters.Length && char.IsLetter(firstWithLetters[i])) i++;
                if (i > 0) detectedPrefix = firstWithLetters.Substring(0, i);
            }

            int maxSeq = expenses
                .Select(e => e.ExpenseId)
                .Where(id => !string.IsNullOrWhiteSpace(id))
                .Select(id =>
                {
                    // Extract trailing digits
                    int j = id.Length - 1;
                    while (j >= 0 && char.IsDigit(id[j])) j--;
                    var numPart = id.Substring(j + 1);
                    return int.TryParse(numPart, out var n) ? n : 0;
                })
                .DefaultIfEmpty(1000) // start sequence at 1001 if nothing found
                .Max();

            return $"{detectedPrefix}{maxSeq + 1}";
        }

        /// <summary>
        /// Updates all properties of an existing expense record with values from a source record.
        /// </summary>
        /// <param name="target">The existing expense record to update.</param>
        /// <param name="source">The source record containing new values.</param>
        private void UpdateExpenseProperties(ExpenseRecord target, ExpenseRecord source)
        {
            target.Department = source.Department;
            target.Category = source.Category;
            target.Amount = source.Amount;
        }
    }
}
