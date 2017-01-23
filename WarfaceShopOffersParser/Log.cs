using System;
using System.Collections.Generic;
using System.IO;

namespace WarfaceShopOffersParser
{
    public class Log
    {
        public static void Info(string message)
        {
            DoLog("[INFO]" + " " + message);
        }

        public static void Warning(string message)
        {
            DoLog("[WARNING]" + " " + message);
        }

        public static void Error(string message, bool isCritical = false)
        {
            DoLog(isCritical ? "[CRITICAL ERROR]" : "[ERROR]" + " " + message);
        }

        static void DoLog(string message)
        {
            string line = DateTime.Now.ToString("[dd-MM HH:mm:ss]") + " " + message;
            Console.WriteLine(line);
        }

    }
}