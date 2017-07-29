namespace Transact
{
    public class Account
    {
        public int PK { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Note { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal Balance { get; set; }
    }
}