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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            firms.ReadFromTXTWriteToArray(@"C:\Users\Odmen\Desktop\Firm_A.txt", @"C:\Users\Odmen\Desktop\Firm_B.txt");
            
            string output = firms.OutputArrayValues(firms.mobilePhones, firms.radioPhones, false);
            textBox1.Text = output;

            using (StreamWriter sw = new StreamWriter(@"C:\Users\Odmen\Desktop\New.txt", false, Encoding.Default))
            {
                sw.WriteLine(output);
            }
            button1.IsEnabled = true;
            button2.IsEnabled = true;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            firms.PutVariousValuesToOneArray(firms.mobilePhones, firms.radioPhones);
            var sorted = firms.PutVariousValuesToOneArray(firms.mobilePhones,
                                     firms.radioPhones).OrderBy(p => p.Price);
            foreach (var i in sorted)
            {
                Console.WriteLine();
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            textBox1.Text = firms.OutputArrayValues(firms.mobilePhones, firms.radioPhones, true);
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
        public RadioPhone(string Name, string Firm, int Reach, bool AnswerPhone, int Price)
        {
            this.Name = Name;
            this.Firm = Firm;
            this.Price = Price;
            this.Reach = Reach;
            this.AnswerPhone = AnswerPhone;
        }

        public string Name { get; set; }
        public string Firm { get; set; }
        public int Price { get; set; }

        public int Reach { get; set; }
        public bool AnswerPhone { get; set; }
    }

    class txtFileHandler
    {
        public MobilePhone[] mobilePhones;
        public RadioPhone[] radioPhones;
        public (Array,Array) ReadFromTXTWriteToArray(string Path, string Path2)
        {
            //отдельный масив путей к файлам
            string[] paths = new string[2] { Path, Path2 };
            //совместное количество строк в путях, для того, чтобы задавать размерность масивов
            int count = File.ReadAllLines(Path).Length + File.ReadAllLines(Path2).Length; 
            
            mobilePhones = new MobilePhone[count];
            radioPhones = new RadioPhone[count];

            //щетчики ячеек для последовательного заполнения масивов
            int mobilePhoneCount = 0; 
            int radioPhoneCount = 0;

            //перебирает сначала txt файл по одному пути, затем по другому
            foreach (string path1 in paths)
            {
                //записывает все строчки в переменную
                var lines = File.ReadAllLines(path1);

                //перенорсит данные из файдла в масивы, одновременно сортирует на "Мобильные Телефоны" и "Радиотелефоны"
                foreach (var line in lines)
                {
                    if (line.Split('_')[0] == "Mobile Phone")
                    {
                        mobilePhones[mobilePhoneCount] = new MobilePhone(line.Split(',')[0], line.Split(',')[1],
                        line.Split(',')[2], Convert.ToInt32(line.Split(',')[3]), Convert.ToInt32(line.Split(',')[4]));
                        mobilePhoneCount++;
                    }
                    if (line.Split('_')[0] == "Radio Phone")
                    {
                        radioPhones[radioPhoneCount] = new RadioPhone(line.Split(',')[0], line.Split(',')[1],
                        Convert.ToInt32(line.Split(',')[2]), Convert.ToBoolean(line.Split(',')[3]),
                        Convert.ToInt32(line.Split(',')[4]));
                        radioPhoneCount++;
                    }
                }
            }

            // перебираем масивы удаляя пустые ячейки
            mobilePhones = mobilePhones.Where(x => x != null).ToArray();
            radioPhones = radioPhones.Where(x => x != null).ToArray();

            return (mobilePhones, radioPhones);
        }
        public string OutputArrayValues(MobilePhone[] mobilesArray , RadioPhone[] radiosArray, bool onlyRadioPhones)// если значение true, то выводит только радиотелефоны
        {
            string output = "";

            int countMobilePhone = 0;
            int countRadioPhone = 0;
            //Перебор масивов для вывода 
            if (onlyRadioPhones == true)
            { goto onlyRadioPhones; }
            output = "Mobile phones: \n";
            foreach (var i in mobilesArray)
            {
                output += $"{i.Name}. {i.Firm} {i.Price} {i.Colour} {i.MemoryCapacity}\n";
                countMobilePhone++;
            }
            onlyRadioPhones:
            output += "Radio phones: \n";
            foreach (var i in radiosArray)
            {
                output += $"{i.Name}. {i.Firm} {i.Price} {i.Reach} {i.AnswerPhone}\n";
                countRadioPhone++;
            }
            return output;
        }
        public List<IPhone> PutVariousValuesToOneArray(MobilePhone[] mobiles, RadioPhone[] radios)
        {
            List<IPhone> mainList = new List<IPhone>();
            //ArrayList mainList = new ArrayList();
            foreach (var mobs in mobiles)
            {
                mainList.Add(mobs);
            }
            foreach (var rad in radios)
            {
                mainList.Add(rad);
            }

            foreach(var i in mainList)
            {
                Console.WriteLine();
            }
            Console.WriteLine();
            return mainList;
            
        }
    }



}
