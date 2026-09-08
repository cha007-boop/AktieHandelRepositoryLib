namespace AktieHandelRepositoryLib
{
    public class AktieHandel
    {
        private static int _nextId = 1;
        private int _id;
        private string _name;
        private int _amount;
        private decimal _exchangePrice;
        public int Id
        {
            get { return _id; }
        }
        public string Name 
        { 
            get { return _name; }
            set
            {
                ValidateName(value);
                _name = value;
            }
        }
        public int Amount
        {
            get { return _amount; }
            set
            {
                ValidateAmount(value);
                _amount = value;
            }
        }
        public decimal ExchangePrice
        {
            get { return _exchangePrice; }
            set
            {
                ValidateExchangePrice(value);
                _exchangePrice = value;
            }
        }


        public AktieHandel(string name, int amount, decimal exchangePrice)
        {
            ValidateName(name);
            ValidateAmount(amount);
            ValidateExchangePrice(exchangePrice);
            _id = _nextId++;
            _name = name;
            Amount = amount;
            ExchangePrice = exchangePrice;
        }

        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length > 4)
            {
                throw new ArgumentException("Name must be at most 4 characters");
            }
        }

        private void ValidateAmount(int amount)
        {
            if (amount == 0)
            {
                throw new ArgumentOutOfRangeException("amount", "Amount cannot be 0");
            }
        }

        private void ValidateExchangePrice(decimal exchangePrice)
        {
            if (exchangePrice < 0)
            {
                throw new ArgumentOutOfRangeException("exchangePrice", "Exchange Price must be positive");
            }
        }
    }
}
