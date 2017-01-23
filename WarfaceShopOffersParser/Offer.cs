namespace WarfaceShopOffersParser
{
    public class Offer
    {
        //<offer id='10152' expirationTime='3 hour' durabilityPoints='0' repair_cost='0' 
        //quantity='0' name='contract_07_easy' item_category_override='' offer_status='normal' 
        //supplier_id='1' discount='0' rank='0' game_price='0' cry_price='0' crown_price='0' 
        //game_price_origin='0' cry_price_origin='0' crown_price_origin='0' key_item_name=''/>

        public string Name;
        public int Quantity = 0;
        public int GamePrice = 0;
        public int CryPrice = 0;
        public int CrownPrice = 0;
        public int RepairCost = 0;
        public ExpirationTimeEnum ExpirationTime = ExpirationTimeEnum.Permanent;

        public enum ExpirationTimeEnum { Permanent, Hours1, Days1, Days7, Days30 }

        public string GetReadableRentTime()
        {
            switch (ExpirationTime)
            {
                case Offer.ExpirationTimeEnum.Permanent:
                    return "Permanent";
                case Offer.ExpirationTimeEnum.Hours1:
                    return "1 hour";
                case Offer.ExpirationTimeEnum.Days1:
                    return "1 day";
                case Offer.ExpirationTimeEnum.Days7:
                    return "7 days";
                case Offer.ExpirationTimeEnum.Days30:
                    return "30 days";
            }
            return null;
        }
    }
}