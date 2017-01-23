using DictionaryParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WarfaceShopOffersParser
{
    public class LocalizationDictionary
    {
        static readonly string DictionaryFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "dict.txt");
        public static List<LocalizationEntity> ReadDictionary()
        {
            var dictInput = File.ReadAllLines(DictionaryFilename);
            return dictInput.Select(line => new Regex(@"(.+)\|(.*)\|(.*)").Match(line)).Select(leMatch => new LocalizationEntity()
            {
                SystemName = leMatch.Groups[1].Value,
                RealName = leMatch.Groups[2].Value,
                FakeName = leMatch.Groups[3].Value,
            }).ToList();
        }
    }
}