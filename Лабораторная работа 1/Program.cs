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
            PoemPattern.Body = "Стих 1";
            elements.Add(PoemPattern);

            PoemPattern = new Poem();
            PoemPattern.Name = "Приход вдохновения";
            PoemPattern.Author = "Юнна Мориц";
            PoemPattern.Year = "1965";
            PoemPattern.Body = "Стих 2";
            elements.Add(PoemPattern);

            PoemPattern = new Poem();
            PoemPattern.Name = "Когда мне встречается в людях дурное...";
            PoemPattern.Author = "Эдуард Асадов";
            PoemPattern.Year = "1966";
            PoemPattern.Body = "Стих 3";
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

            //elements = LoadFromFileXml(); // получение и конвертация файла xml

            //PrintResilt(elements); // вывод файла в консоль

            //SaveToFileXml(elements); // сохранение файла xml

            string loadFile = @"C:\Users\mikl\Desktop\Алиска\file.json";
            string SaveFile = @"C:\Users\mikl\Desktop\Алиска\test";

            Ifmtfile fmtfile = GetFmt("JSON");

            fmtfile.SaveToFile(elements, SaveFile);            

            Console.ReadLine();

            
        }

        public static Ifmtfile GetFmt(string fileformat)
        {
            switch (fileformat)
            {
                case "JSON": return new TJSONfile();
                case "XML": return new TXMLfile();
                default: return null;
            }
        }
    }
}
