using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace DictionaryParser
{
    public static class LocalizationEntitiesExtractor
    {
        public static List<LocalizationEntity> ExtractLocalizationEntities(string input)
        {
            string leXml = new Regex("[\r\n]| {2,}").Replace(input, "");
            var leMatches =
                new Regex(
                        "<Row ss:AutoFitHeight=\"0\">" +
                        "<Cell><Data ss:Type=\"String\">([^<]*)</Data></Cell>" +
                        "<Cell><Data ss:Type=\"String\">([^<]*)</Data></Cell>" +
                        "<Cell><Data ss:Type=\"String\">([^<]*)</Data></Cell>" +
                        "<Cell><Data ss:Type=\"String\">([^<]*)</Data></Cell>" +
                        "</Row>")
                    .Matches(leXml);
            return leMatches.Cast<Match>().Select(match => new LocalizationEntity
            {
                Type = match.Groups[1].Value,
                SystemName = match.Groups[2].Value,
                RealName = match.Groups[3].Value,
                FakeName = match.Groups[4].Value
            }).ToList();
        }
    }
}