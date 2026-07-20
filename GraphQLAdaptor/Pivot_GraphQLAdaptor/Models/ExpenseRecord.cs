using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Pivot_GraphQLAdaptor.Models
{
    public class ExpenseRecord
    {
        /// <summary>
        /// Static in-memory store for expense records.
        /// This persists data across mutations and queries during application lifetime.
        /// </summary>
        private static List<ExpenseRecord> _expenseStore = null;

        /// <summary>
        /// Initializes the expense store with seed data if not already initialized.
        /// </summary>
        private static void InitializeStore()
        {
            if (_expenseStore != null) return;

            // Seed with a small set of expense records
            _expenseStore = new List<ExpenseRecord>
            {
                new ExpenseRecord
                {
                    ExpenseId = "EXP1001",
                    Department = "Finance",
                    Category = "Travel & Mileage",
                    Amount = 250.75
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1002",
                    Department = "Engineering",
                    Category = "Software & SaaS",
                    Amount = 1200.00
                }
            };
        }

        /// <summary>
        /// Retrieves all expense records from the in-memory store.
        /// This list persists across mutations and queries.
        /// </summary>
        public static List<ExpenseRecord> GetAllRecords()
        {
            InitializeStore();
            return _expenseStore;
        }

        /// <summary>
        /// Clears the expense store and reinitializes with seed data.
        /// Useful for testing or resetting the application state.
        /// </summary>
        public static void ResetStore()
        {
            _expenseStore = null;
            InitializeStore();
        }

        [JsonPropertyName("expenseId")]
        public string? ExpenseId { get; set; }

        [JsonPropertyName("department")]
        public string? Department { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("amount")]
        public double Amount { get; set; }
    }
}
