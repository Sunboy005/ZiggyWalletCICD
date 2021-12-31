namespace ZiggyZiggyWallet.DTOs.Currency
{
    public class CurrencyToAdd
    {
        public string Name { get; set; }
        public string ShortCode { get; set; }

        //Abbrevation should be in accordance to API to use
        //Abbrevation should be in lowercase 
        //It should be 3-letters
        public string Abbrevation { get; set; }
    }
}
