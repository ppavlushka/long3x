namespace long3x.ViewModels
{
    public class SignalViewModel
    {
        public string ChannelId { get; set; }

        public string Coin1 { get; set; }

        public string Coin2 { get; set; }

        public string FullCoinDescription => string.Concat(Coin1, Coin2);

        public byte Leverage { get; set; }

        public string LongShort { get; set; }

        public string Risk { get; set; }

        public decimal EntryZoneMin { get; set; }

        public decimal EntryZoneMax { get; set; }

        public string Targets { get; set; }

        public decimal StopLoss { get; set; }

        public string TradingType { get; set; }

        public string AdditionalInfo { get; set; }

        public string Ts { get; set; }

        public decimal Price { get; set; }
    }
}
