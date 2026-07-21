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
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1003",
                    Department = "Human Resources",
                    Category = "Training & Development",
                    Amount = 450.00
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1004",
                    Department = "Marketing",
                    Category = "Advertising",
                    Amount = 875.50
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1005",
                    Department = "Sales",
                    Category = "Client Entertainment",
                    Amount = 620.25
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1006",
                    Department = "Operations",
                    Category = "Office Supplies",
                    Amount = 180.75
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1007",
                    Department = "Finance",
                    Category = "Audit Fees",
                    Amount = 950.00
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1008",
                    Department = "Engineering",
                    Category = "Hardware",
                    Amount = 2100.00
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1009",
                    Department = "Marketing",
                    Category = "Event Sponsorship",
                    Amount = 1450.00
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1010",
                    Department = "Sales",
                    Category = "Travel & Mileage",
                    Amount = 780.50
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1011",
                    Department = "Human Resources",
                    Category = "Recruitment",
                    Amount = 525.00
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1012",
                    Department = "Operations",
                    Category = "Utilities",
                    Amount = 340.60
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1013",
                    Department = "Engineering",
                    Category = "Software & SaaS",
                    Amount = 1325.75
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1014",
                    Department = "Finance",
                    Category = "Consulting",
                    Amount = 890.25
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1015",
                    Department = "Sales",
                    Category = "Client Gifts",
                    Amount = 275.00
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1016",
                    Department = "Marketing",
                    Category = "Market Research",
                    Amount = 680.40
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1017",
                    Department = "Operations",
                    Category = "Maintenance",
                    Amount = 410.80
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1018",
                    Department = "Human Resources",
                    Category = "Employee Benefits",
                    Amount = 990.00
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1019",
                    Department = "Legal",
                    Category = "Compliance",
                    Amount = 650.00
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1020",
                    Department = "Customer Support",
                    Category = "Training",
                    Amount = 420.50
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1021",
                    Department = "Procurement",
                    Category = "Vendor Management",
                    Amount = 1100.00
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1022",
                    Department = "Administration",
                    Category = "Office Supplies",
                    Amount = 320.75
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1023",
                    Department = "Research",
                    Category = "Innovation",
                    Amount = 2500.00
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1024",
                    Department = "Quality Assurance",
                    Category = "Testing Tools",
                    Amount = 875.25
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1025",
                    Department = "Product Management",
                    Category = "Market Analysis",
                    Amount = 780.00
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1026",
                    Department = "Business Development",
                    Category = "Partnerships",
                    Amount = 1450.50
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1027",
                    Department = "IT Infrastructure",
                    Category = "Cloud Services",
                    Amount = 3200.00
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1028",
                    Department = "Security",
                    Category = "Monitoring",
                    Amount = 950.00
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1029",
                    Department = "Logistics",
                    Category = "Transportation",
                    Amount = 680.50
                },
                new ExpenseRecord
                {
                    ExpenseId = "EXP1030",
                    Department = "Facilities",
                    Category = "Building Maintenance",
                    Amount = 840.75
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