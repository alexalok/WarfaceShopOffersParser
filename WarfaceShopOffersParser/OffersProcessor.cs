using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DictionaryParser;

namespace WarfaceShopOffersParser
{
    public class OffersProcessor
    {
        public static void CreateOffersFile(List<Offer> offers, List<LocalizationEntity> dictionary, string outputFilename = "output.txt")
        {
            var outputSb = new StringBuilder();
            outputSb.Append("Eu Name|Ru name|Rent time|Repair cost|W$ cost|K cost|Crown cost" + Environment.NewLine);
            foreach (var offer in offers)
            {
                if (offer.Name.Contains("ar08"))
                ;
                var le = dictionary.FirstOrDefault(le_ => le_.SystemName.Contains(offer.Name));
                outputSb.Append((le?.FakeName ?? offer.Name) + "|" + (le?.RealName ?? offer.Name) + "|" + offer.GetReadableRentTime() +
                    "|" + offer.RepairCost + "|" + offer.GamePrice + "|" + offer.CryPrice + "|" + offer.CrownPrice + Environment.NewLine);
            }
            File.WriteAllText(outputFilename, outputSb.ToString());
        }
    }
}