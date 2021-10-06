namespace long3x.Data.Entities
{
    public class TraderInfoEntity
    {
        public string ChannelId { get; set; }

        public string Coin1 { get; set; }

        public string Coin2 { get; set; }

        public byte Leverage { get; set; }

        public string LongShort { get; set; }

        public decimal EntryZoneMin { get; set; }

        public decimal EntryZoneMax { get; set; }

        public string Targets { get; set; }

        public decimal StopLoss { get; set; }

        public string TradingType { get; set; }

        public string AdditionalInfo { get; set; }

        public string Ts { get; set; }
    }
}
