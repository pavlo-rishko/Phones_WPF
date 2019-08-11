using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Phones_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>easddz
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
            button1.IsEnabled = false;
            button2.IsEnabled = false;


        }
        txtFileHandler firms = new txtFileHandler();
        List<IPhone> arrayWithAllPhones;
        private void Button_Click(object sender, RoutedEventArgs e)
        { 
            
            arrayWithAllPhones = firms.ReadFromTXTWriteToArray(@"C:\Firm_A.txt", @"C:\Firm_B.txt");

            textBox1.Text = firms.CreateStringToOutputAllPhones(arrayWithAllPhones);

            button2.IsEnabled = true;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var sorted = arrayWithAllPhones.OrderBy(p => p.Price);//сортировка по цене
            textBox1.Clear();
            string output = firms.CreateStringWithAllPhonesSortedByPrice(sorted);
            textBox1.Text = output;

            using (StreamWriter sw = new StreamWriter(@"C:\New.txt", false, Encoding.Default))
            {
                sw.Write(output);
            }

            button2.IsEnabled = false;
            button1.IsEnabled = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string output = firms.CreateStringRadioPhonesWithAnswerphone(arrayWithAllPhones);
            textBox1.Text = output;
            using (StreamWriter sw = new StreamWriter(@"C:\New.txt", true))
            {
                sw.Write(output);
            }
            button2.IsEnabled = false;
            button1.IsEnabled = false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }
    }

    interface IPhone
    {

        string Name { get; set; }
        string Firm { get; set; }
        int Price { get; set; }
    }

    public class MobilePhone : IPhone
    {
        public MobilePhone() { }
        public MobilePhone(string Name, string Firm, string Colour, int MemoryCapacity, int Price)
        {
            this.Name = Name;
            this.Firm = Firm;
            this.Price = Price;
            this.Colour = Colour;
            this.MemoryCapacity = MemoryCapacity;
        }

        public string Name { get; set; }
        public string Firm { get; set; }
        public int Price { get; set; }

        public string Colour { get; set; }
        public int MemoryCapacity { get; set; }
    }

    public class RadioPhone : IPhone
    {
        public RadioPhone() { }
        public RadioPhone(string Name, string Firm, int Reach, bool Answerphone, int Price)
        {
            this.Name = Name;
            this.Firm = Firm;
            this.Price = Price;
            this.Reach = Reach;
            this.Answerphone = Answerphone;
        }

        public string Name { get; set; }
        public string Firm { get; set; }
        public int Price { get; set; }

        public int Reach { get; set; }
        public bool Answerphone { get; set; }
    }

    class txtFileHandler
    {
        List<IPhone> mainList;
        public List<IPhone> ReadFromTXTWriteToArray(string Path, string Path2)
        {
            //отдельный масив путей к файлам
            string[] paths = new string[2] { Path, Path2 };

            mainList = new List<IPhone>();

            //перебирает сначала txt файл по одному пути, затем по другому
            foreach (string path1 in paths)
            {
                //записывает все строчки в переменную
                var lines = File.ReadAllLines(path1);

                //перенорсит данные из файдла в массив
                foreach (var line in lines)
                {

                    if (line.Split(',')[3] == "true" || line.Split(',')[3] == "false")
                    {
                        mainList.Add(new RadioPhone(line.Split(',')[0], line.Split(',')[1],
                        Convert.ToInt32(line.Split(',')[2]), Convert.ToBoolean(line.Split(',')[3]),Convert.ToInt32(line.Split(',')[4])));
                    }
                    else
                    {
                        mainList.Add(new MobilePhone(line.Split(',')[0], line.Split(',')[1],
                        line.Split(',')[2], Convert.ToInt32(line.Split(',')[3]), Convert.ToInt32(line.Split(',')[4])));
                    }
                }
            }
            return mainList;
        }
        public string CreateStringToOutputAllPhones(List<IPhone> mainList)
        {    
            //Перебор масивa для вывода(склеивает строку)
            string output = "All phones: \n";
            foreach (var a in mainList)
            {
                var variableType = a.GetType();//определяет тип переменной в данной итерации и формирует соответствующую типу строку
                if (variableType.Name == "MobilePhone")
                {
                    MobilePhone i = (MobilePhone)a;
                    output += $"Name: {i.Name} Firm: {i.Firm} Colour: {i.Colour} Memory capacity{i.MemoryCapacity}  Price: {i.Price}\n";
                }
                else
                {
                    RadioPhone i = (RadioPhone)a;
                    output += $"Name: {i.Name} Firm: {i.Firm} Reach: {i.Reach} Anserphone: {i.Answerphone}  Price: {i.Price}\n";
                }
            }
            return output;
        }

        public string CreateStringRadioPhonesWithAnswerphone(List<IPhone> mainList)
        {
            string output = "Radio phones with Answerphone: \n";
            foreach (var a in mainList)
            {
                var variableType = a.GetType();
                if (variableType.Name == "RadioPhone") //отделает только радиотелефоны
                {
                    RadioPhone i = (RadioPhone)a;
                    if (i.Answerphone == true) // отделяет только радиотелефоны с автоответчиком
                    {
                        output += $"Name: {i.Name} Firm: {i.Firm} Reach: {i.Reach} Anserphone: {i.Answerphone}  Price: {i.Price}\n";
                    }
                }
            }
            return output;
        }

        public string CreateStringWithAllPhonesSortedByPrice(IOrderedEnumerable<IPhone> sorted)
        {
            string output = "All phones sorted by price:\n";
            int totalPrice = 0;

                foreach (var a in sorted)
                {
                    var variableType = a.GetType();

                    if (variableType.Name == "MobilePhone")
                    {
                            MobilePhone i = (MobilePhone)a;
                            output += ($"Name: {i.Name} Firm: {i.Firm} Colour: {i.Colour} " +
                                   $"Memory capacity{i.MemoryCapacity}  Price: {i.Price}\n");
                            totalPrice += i.Price;

                    }
                    else
                    {
                            RadioPhone i = (RadioPhone)a;
                            output += ($"Name: {i.Name} Firm: {i.Firm} Reach: {i.Reach} " +
                                        $"Anserphone: {i.Answerphone}  Price: {i.Price}\n");
                    }
                }

                return output + "Total rpice: " + Convert.ToString(totalPrice) + "\n";
            
        }
    }
}
