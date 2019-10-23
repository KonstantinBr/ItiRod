using System;
using System.IO;

namespace CSV
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1. Custom path \n2. Default path");
            var choise = Console.ReadKey().KeyChar;
            Console.Clear();
            switch (choise)
            {
                case '1':
                    Console.WriteLine("Input custom path");
                    var customPath = Console.ReadLine();
                    while (!File.Exists(customPath))
                    {
                        Console.Clear();
                        Console.WriteLine("Input custom path");
                        customPath = Console.ReadLine();
                    }
                    CustomPath(customPath);
                    break;
                case '2':
                    DefaultPath();
                    break;

            }
        }

        static void DefaultPath()
        {
            CSVTransformer cSVTransformer = new CSVTransformer();
            char choise = '0';

            while (choise != '5')
            {
                Console.WriteLine("\n\n\n1. Upload SCV File to Console \n2. Create XLSX File from CSV \n3. Create PDF File from CSV\n4. Create JSON File from CSV\n5. Exit");

                choise = Console.ReadKey().KeyChar;
                Console.Clear();

                switch (choise)
                {
                    case '1':
                        cSVTransformer.ConsoleOutput();
                        Console.WriteLine("SCV File Uploaded");
                        break;
                    case '2':
                        cSVTransformer.XLSXOutputGroupByStudent();
                        Console.WriteLine("XLSX File Created");

                        break;
                    case '3':
                        cSVTransformer.PDFOutput();
                        Console.WriteLine("PDF File Created");
                        break;
                    case '4':
                        cSVTransformer.JSONOutput();
                        Console.WriteLine("JSON File Created");
                        break;
                }
            }
        }

        static void CustomPath(string path)
        {
            CSVTransformer cSVTransformer = new CSVTransformer();
            char choise = '0';

            while (choise != '4')
            {
                Console.WriteLine("\n\n\n1. Upload SCV File to Console \n2. Create XLSX File from CSV \n3. Create PDF File from CSV\n4. Exit");

                choise = Console.ReadKey().KeyChar;
                Console.Clear();

                switch (choise)
                {
                    case '1':
                        cSVTransformer.ConsoleOutput(path);
                        Console.WriteLine("SCV File Uploaded");
                        break;
                    case '2':
                        cSVTransformer.XLSXOutputGroupByStudent();
                        Console.WriteLine("XLSX File Created");

                        break;
                    case '3':
                        cSVTransformer.PDFOutput();
                        Console.WriteLine("PDF File Created");
                        break;
                    case '4':
                        cSVTransformer.JSONOutput();
                        Console.WriteLine("JSON File Created");
                        break;
                }
            }
        }
    }
}
