using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace WarfaceShopOffersParser
{
    public class OffersExtractor
    {
        const int MaxOffers = 1000; //Max offers that can be reported by game

        public static List<Offer> ExtractOffers(string input)
        {
            var offersMatches = new Regex("<shop_get_offers[^>]+from=\'(\\d+)\' to=\'(\\d+)\'[^>]+>(.*)</shop_get_offers>").Matches(input);

            var offersXmlSb = new StringBuilder("<abstract-root>");
            var currAwaitingOffer = 0;
            for (var i = 0; i < Math.Ceiling(MaxOffers / 250f); i++)  //making sure we do not include same offers stanza multiple times
            {
                foreach (Match offersMatch in offersMatches)
                {
                    if (offersMatch.Groups[1].Value != currAwaitingOffer.ToString()) continue;
                    offersXmlSb.Append(offersMatch.Groups[3].Value.Replace(Environment.NewLine, ""));
                    break;
                }
                currAwaitingOffer += 250;
            }
            offersXmlSb.Append("</abstract-root>");

            string offersXml = offersXmlSb.ToString();
            var offersList = new List<Offer>();
            var reader = XmlReader.Create(new StringReader(offersXml));
            while (reader.Read())
            {
                if (!reader.HasAttributes || reader.AttributeCount != 18)
                    continue;
                var offer = ParseOffer(ref reader);
                if (!offersList.Any(o => o.Name == offer.Name && o.ExpirationTime == offer.ExpirationTime))
                    offersList.Add(offer);
                else
                    Console.WriteLine($"Duplicate offers detected (same name and time) - duplicates are not being added. " +
                                      $"Weapon: {offer.Name}, ExpirationTime: {offer.GetReadableRentTime()}");
            }
            return offersList;
        }

        static Offer ParseOffer(ref XmlReader reader)
        {
            var offer = new Offer();
            for (var i = 0; i < reader.AttributeCount; i++)
            {
                if (offer.Name == "ar08_shop")
                    ;
                reader.MoveToNextAttribute();
                string attrName = reader.Name;
                string attrValue = reader.Value;
                switch (attrName)
                {
                    //<offer id='10152' expirationTime='3 hour' durabilityPoints='0' repair_cost='0' 
                    //quantity='0' name='contract_07_easy' item_category_override='' offer_status='normal' 
                    //supplier_id='1' discount='0' rank='0' game_price='0' cry_price='0' crown_price='0' 
                    //game_price_origin='0' cry_price_origin='0' crown_price_origin='0' key_item_name=''/>
                    case "expirationTime":
                        switch (attrValue)
                        {
                            case "0":
                                offer.ExpirationTime = Offer.ExpirationTimeEnum.Permanent;
                                break;
                            case "1 hour":
                                offer.ExpirationTime = Offer.ExpirationTimeEnum.Hours1;
                                break;
                            case "1 day":
                                offer.ExpirationTime = Offer.ExpirationTimeEnum.Days1;
                                break;
                            case "7 day":
                                offer.ExpirationTime = Offer.ExpirationTimeEnum.Days7;
                                break;
                            case "30 day":
                                offer.ExpirationTime = Offer.ExpirationTimeEnum.Days30;
                                break;
                        }
                        break;
                    case "repair_cost":
                        if (attrValue.IsInt())
                            offer.RepairCost = Convert.ToInt32(attrValue);
                        else //this is probably a box TODO create a nested class for box and its items?
                        {
                            if (attrValue.Contains("pt14"))
                                ;
                            //repair_cost='sr14_shop,5400,36000;sr14_gold01_shop,5400,36000;'
                            return new Offer
                            {
                                Name = new Regex("([^,]+)").Match(attrValue).Groups[1].Value,
                                RepairCost =
                                    Convert.ToInt32(new Regex("[^,]+,([^,]+)").Match(attrValue).Groups[1].Value)

                            }; //overwriting original offer here, is it ok? - Yup, probably is <:
                            //we will lose ANYTHING except first item here, but gold version does not need repairs ATM (and skins/camos ofc), so i dont even care 
                        }
                        break;
                    case "name":
                        offer.Name = attrValue;
                        break;
                    case "game_price":
                        offer.GamePrice = Convert.ToInt32(attrValue);
                        break;
                    case "cry_price":
                        offer.CryPrice = Convert.ToInt32(attrValue);
                        break;
                    case "crown_price":
                        offer.CrownPrice = Convert.ToInt32(attrValue);
                        break;
                    case "quantity":
                        offer.Quantity = Convert.ToInt32(attrValue);
                        break;
                }
            }
            return offer;
        }
    }
}