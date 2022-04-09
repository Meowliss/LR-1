﻿using System;
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

        public static List<Poem> LoadFromFileJson()
        {
            string filejson = System.IO.File.ReadAllText(@"C:\Users\79859\Desktop\coding\Лабораторная работа 1\files\filejson.json");
            List<Poem> elements = JsonSerializer.Deserialize<List<Poem>>(filejson);
            return elements;
        }
        /*
        public static List<Poem> LoadFromFileXml()
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Poem>));
            using (FileStream filexml = new FileStream(@"C:\Users\79859\Desktop\coding\Лабораторная работа 1\files\filexml.xml", FileMode.OpenOrCreate))
            {
                List<Poem> elements = (List<Poem>)formatter.Deserialize(filexml);
                return elements;
            }
        }
        */

        public static void SaveToFileJson(List<Poem> elements)
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            string filejson = JsonSerializer.Serialize(elements, options);
            File.WriteAllText(@"C:\Users\79859\Desktop\coding\Лабораторная работа 1\files\newfilejson.json", filejson);
        }
        /*
        public static void SaveToFileXml(List<Poem> elements)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Poem>));
            using (FileStream filexml = new FileStream(@"C:\Users\79859\Desktop\coding\Лабораторная работа 1\files\newfilexml.xml",FileMode.OpenOrCreate))
            {
                formatter.Serialize(filexml, elements);
            }
        }
        */

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

            //CreateTestElements(elements); //создание тестовых элементов 

            //elements = LoadFromFileXml(); // получение и конвертация файла xml

            //PrintResilt(elements); // вывод файла в консоль

            //SaveToFileXml(elements); // сохранение файла xml
             
            Console.ReadLine();

            
        }
    }
}