using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using long3x.Common.ConfigurationModels;
using long3x.Data.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using long3x.Data.Interfaces;
using Newtonsoft.Json;

namespace long3x.Data.ApiHandler
{
    public class BinanceApiHandler: IBinanceApiHandler
    {
        private readonly ILogger<BinanceApiHandler> logger;
        private readonly HttpClient client;
        private readonly ApiConnectionSettings apiConnectionSettings;

        public BinanceApiHandler(ILogger<BinanceApiHandler> logger, IOptions<ApiConnectionSettings> apiConnectionSettings)
        {
            this.logger = logger;
            this.apiConnectionSettings = apiConnectionSettings.Value;
            client = new HttpClient
            {
                BaseAddress = new Uri(this.apiConnectionSettings.BaseUrl)
            };
        }

        public Dictionary<string, decimal> GetPriceDictionary(IEnumerable<string> coins)
        {
            
            var errorList = new List<string>();
            var resultDictionary = new Dictionary<string, decimal>();
            var response = client.GetAsync(apiConnectionSettings.AllPriceUrl).GetAwaiter().GetResult();
            var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var jsonResult = JsonConvert.DeserializeObject<IList<BinanceApiPriceEntity>>(responseContent);
            foreach (var coin in coins)
            {
                var currentCoinPriceEntity = jsonResult.FirstOrDefault(x => x.Symbol.Equals(coin));
                if (currentCoinPriceEntity == null)
                {
                    errorList.Add(coin);
                }
                else
                {
                    resultDictionary.TryAdd(coin, currentCoinPriceEntity.Price);
                }
            }

            if (errorList.Count > 0)
            {
                logger.LogWarning($"[API] Can't find price for {String.Join(", ",errorList)}");
            }
            

            return resultDictionary;
        }

    }
}
