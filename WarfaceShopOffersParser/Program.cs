using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarfaceShopOffersParser
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
            string inputFilename = args[0];
            string outputFilename = Path.Combine(Path.GetDirectoryName(inputFilename),
                Path.GetFileNameWithoutExtension(inputFilename) + "_ParsedShopOffers.txt");
            Console.WriteLine("Input filename: " + inputFilename);
            Console.WriteLine("Output filename: " + outputFilename);
            string input = File.ReadAllText(inputFilename);
            var offersList = OffersExtractor.ExtractOffers(input);
            var dictionary = LocalizationDictionary.ReadDictionary();
            OffersProcessor.CreateOffersFile(offersList, dictionary, outputFilename);
            //Console.ReadLine();
        }
    }
}
