using System;

namespace WarfaceShopOffersParser
{
    public static class Extensions
    {
        public static bool IsInt(this string input)
        {
            int x;
            return int.TryParse(input, out x);
        }
    }
}