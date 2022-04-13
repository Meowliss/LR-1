using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.IO;
using System.Xml.Serialization;
using ClosedXML.Excel;
using Microsoft.Office.Interop.Excel;
using Range = Microsoft.Office.Interop.Excel.Range;
using Microsoft.VisualBasic.FileIO;

namespace Лабораторная_работа_1
{
    public class Poem
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Year { get; set; }
        public string Body { get; set; }
    }

    class Program
    {
        public static void CreateTestElements(List<Poem> elements) // заполнение коллекции
        {
            Poem PoemPattern = new Poem();
            PoemPattern.Name = "Спасибо, музыка, за то...";
            PoemPattern.Author = "Владимир Соколов";
            PoemPattern.Year = "1960";
            PoemPattern.Body = "Содержание";
            elements.Add(PoemPattern);

            PoemPattern = new Poem();
            PoemPattern.Name = "Приход вдохновения";
            PoemPattern.Author = "Юнна Мориц";
            PoemPattern.Year = "1965";
            PoemPattern.Body = "Содержание";
            elements.Add(PoemPattern);

            PoemPattern = new Poem();
            PoemPattern.Name = "Когда мне встречается в людях дурное...";
            PoemPattern.Author = "Эдуард Асадов";
            PoemPattern.Year = "1966";
            PoemPattern.Body = "Содержание";
            elements.Add(PoemPattern);
        }

        public interface Ifmtfile
        {
            public abstract List<Poem> LoadFromFile(string filename);
            public abstract void SaveToFile(List<Poem> elements, string filename);
        }

        class TJSONfile : Ifmtfile
        {
            public List<Poem> LoadFromFile(string filename)
            {
                List<Poem> elements = JsonSerializer.Deserialize<List<Poem>>(filename);
                return elements;
            }
            public void SaveToFile(List<Poem> elements, string filename)
            {
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                    WriteIndented = true
                };
                string filejson = JsonSerializer.Serialize(elements, options);
                File.WriteAllText(filename, filejson);
            }
        }
        class TXMLfile : Ifmtfile
        {
            public List<Poem> LoadFromFile(string filename)
            {
                XmlSerializer formatter = new XmlSerializer(typeof(List<Poem>));
                using (FileStream filexml = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    List<Poem> elements = (List<Poem>)formatter.Deserialize(filexml);
                    return elements;
                }
            }
            public void SaveToFile(List<Poem> elements, string filename)
            {
                XmlSerializer formatter = new XmlSerializer(typeof(List<Poem>));
                using (FileStream filexml = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(filexml, elements);
                }
            }
        }
        class TXLSXfile : Ifmtfile
        {
            public List<Poem> LoadFromFile(string filename)
            {
                var elements = new List<Poem>();
                Application excelApp = new Application();
                Workbook excelBook = excelApp.Workbooks.Open(filename);
                _Worksheet excelSheet = (_Worksheet)excelBook.Sheets[1];
                Range excelRange = excelSheet.UsedRange;
                
                
                for (int i = 1; i <= excelRange.Rows.Count; i++)
                {
                    Poem PoemPattern = new Poem();                   
                    PoemPattern.Name = excelRange.Cells[i, 1].Value2.ToString();
                    PoemPattern.Author = excelRange.Cells[i, 2].Value2.ToString();
                    PoemPattern.Year = excelRange.Cells[i, 3].Value2.ToString();
                    PoemPattern.Body = excelRange.Cells[i, 4].Value2.ToString();
                    elements.Add(PoemPattern);
                }

                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);             
                return elements;
            }
            public void SaveToFile(List<Poem> elements, string filename)
            {
                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("Стихи");
                for (int i = 0; i < elements.Count; i++)
                {
                    ws.Cell("A" + Convert.ToString(i + 1)).Value = elements[i].Name;
                    ws.Cell("B" + Convert.ToString(i + 1)).Value = elements[i].Author;
                    ws.Cell("C" + Convert.ToString(i + 1)).Value = elements[i].Year;
                    ws.Cell("D" + Convert.ToString(i + 1)).Value = elements[i].Body;
                }
                wb.SaveAs(filename);
            }
        }
        class TCSVfile : Ifmtfile
        {
            public List<Poem> LoadFromFile(string filename)
            {
                
                List<Poem> elements = JsonSerializer.Deserialize<List<Poem>>(filename); // временный код
                return elements; // временный код
            }
            public void SaveToFile(List<Poem> elements, string filename)
            {
                StringBuilder csvcontent = new StringBuilder();
                for (int i = 0; i < elements.Count; i++)
                {
                    csvcontent.AppendLine(elements[i].Name + ";" + elements[i].Author + ";" + elements[i].Year + ";" + elements[i].Body); 
                }
                File.AppendAllText(filename, csvcontent.ToString(), Encoding.UTF8);
            }
        }


        public static void PrintResilt(List<Poem> elements)
        {
            for (int i = 0; i < elements.Count; i++)
            {
                Console.WriteLine(elements[i].Name);
                Console.WriteLine(elements[i].Author);
                Console.WriteLine(elements[i].Year);
                Console.WriteLine(elements[i].Body + "\n");
            }
        }



        static void Main(string[] args)
        {
            var elements = new List<Poem>(); // создание коллекции лист с объектами poem 
            CreateTestElements(elements); //создание тестовых элементов 
            //PrintResilt(elements); // вывод файла в консоль


            /*
            string choiceformat = Console.ReadLine(); // выбор формата
            string SaveFile = @"C:\Users\79859\Desktop\coding\files\filecsv."+ choiceformat; // место сохранения 
            Ifmtfile savefmtfile = GetFmt(choiceformat); // создание объекта типа Ifmtfile, выделение места для типа json
            savefmtfile.SaveToFile(elements, SaveFile); // сохранение файла в выбранном формате
            Console.ReadLine();
            */


            
            //string LoadFile = System.IO.File.ReadAllText(@"C:\Users\79859\Desktop\coding\files\filexlsx.xlsx"); // форматируемый файл
            string getextension = Path.GetExtension(@"C:\Users\79859\Desktop\coding\files\filecsv.csv").Substring(1); // определение формата файла
            Ifmtfile loadfmtfile = GetFmt(getextension);
            elements = loadfmtfile.LoadFromFile(@"C:\Users\79859\Desktop\coding\files\filecsv.csv");           
            PrintResilt(elements);
            Console.ReadLine();
                        
        }

        public static Ifmtfile GetFmt(string fileformat)
        {
            switch (fileformat)
            {
                case "json": return new TJSONfile();
                case "xml": return new TXMLfile();
                case "xlsx": return new TXLSXfile();
                case "csv": return new TCSVfile();
                default: return null;
            }
        }

        


    }
}
