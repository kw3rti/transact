using System;

namespace Transact
{
    public class Transaction
    {
        public int PK { get; set; }
        public int AccountPK { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public Decimal Amount { get; set; }
        public string Category { get; set; }
        public string Type_ToAccount { get; set; }
        public string Notes { get; set; }
    }
}