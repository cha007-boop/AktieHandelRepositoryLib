namespace AktieHandelRepositoryLib
{
    public class AktieHandel
    {
        #region Instance fields
        private int _id;
        private string _name;
        private int _amount;
        private double _exchangePrice;
        #endregion

        #region Properties
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
        public double ExchangePrice
        {
            get { return _exchangePrice; }
            set
            {
                ValidateExchangePrice(value);
                _exchangePrice = value;
            }
        }
        #endregion

        #region Constructors
        public AktieHandel(string name, int amount, double exchangePrice)
        {
            ValidateName(name);
            ValidateAmount(amount);
            ValidateExchangePrice(exchangePrice);
            _id = 0; // Initialize with a default value
            _name = name;
            _amount = amount;
            _exchangePrice = exchangePrice;
        }
        public AktieHandel(int id, string name, int amount, double exchangePrice)
        {
            ValidateName(name);
            ValidateAmount(amount);
            ValidateExchangePrice(exchangePrice);
            _id = id;
            _name = name;
            _amount = amount;
            _exchangePrice = exchangePrice;
        }
        #endregion

        #region Methods
        private void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }
            if (name.Length < 4)
            {
                throw new ArgumentException("Name must be at least 4 characters");
            }
        }

        private void ValidateAmount(int amount)
        {
            if (amount == 0)
            {
                throw new ArgumentOutOfRangeException("amount", "Amount cannot be 0");
            }
        }

        private void ValidateExchangePrice(double exchangePrice)
        {
            if (exchangePrice <= 0)
            {
                throw new ArgumentOutOfRangeException("exchangePrice", "Exchange Price must be positive");
            }
        }
        #endregion
    }
}
