using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryParser
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Pass file to parse");
                Console.ReadLine();
                return;
            }
            string filename = args[0];
            string input = File.ReadAllText(filename);
            var leList = LocalizationEntitiesExtractor.ExtractLocalizationEntities(input);
            var outputSb = new StringBuilder();
            leList.ForEach(le =>
            {
                outputSb.Append(le.SystemName + "|" + le.RealName + "|" + le.FakeName + Environment.NewLine);
            });
            File.WriteAllText("dict.txt", outputSb.ToString());
        }
    }
}
