using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;

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
            //выключаю кнопки
            button1.IsEnabled = false;
            button2.IsEnabled = false;
            button3.IsEnabled = false;
        }

        readonly TxtFileHandler firms = new TxtFileHandler();
        List<IPhone> arrayWithAllPhones;
        string savePath;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //вызываем диалог для выбора текстовых файлов с данными.
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Text documents (*.txt)|*.txt"
            };

        firstPath:
            MessageBox.Show("Будь ласка вкажіть шлях до 1-го файлу\a");

            bool? result = dialog.ShowDialog();

            string filePath1;
            string filePath2;
            ///если пользователь указал путь то оно передается в filePath1, в противном случае 
            //пользователю еще раз предлагается указать путь
            if (result == true)
            {
                filePath1 = dialog.FileName;

                MessageBox.Show("Будь ласка вкажіть шлях до 2-го файлу");
            secondPath:
                bool? result2 = dialog.ShowDialog();
                if (result2 == true)
                {
                    filePath2 = dialog.FileName;
                }
                else
                {
                    MessageBox.Show("Ви не вказали шлях до 2-го файлу, будь ласка спробуйте ще раз");
                    goto secondPath;
                }
            }
            else
            {
                MessageBox.Show("Ви не вказали шлях до 1-го файлу, будь ласка спробуйте ще раз");
                goto firstPath;
            }

            //Обработка ошибок чтения идет в методе ReadFromTXTWriteToArray
            arrayWithAllPhones = firms.ReadFromTXTWriteToArray(filePath1, filePath2);
            //Если метод возвращает null, значит внутри ошибка связанная с файлом,
            //соответственно пользователь должен снова указать путь к файлам
            if (arrayWithAllPhones == null)
            {
                goto firstPath;
            }

            //вызов метода возвращающего строку со всеми телефонами
            textBox1.Text = firms.CreateStringToOutputAllPhones(arrayWithAllPhones);

            button2.IsEnabled = true;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //сортировка по цене
            var sorted = arrayWithAllPhones.OrderBy(p => p.Price);

            //вывоз метода который создает строку и затем вывод
            string output = firms.CreateStringWithAllPhonesSortedByPrice(sorted);
            textBox1.Text = output;

            //диалог сохранения файла
            Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Text documents (*.txt)|*.txt"
            };

            bool? result = dialog.ShowDialog();

            if(result == true)
            {
                savePath = dialog.FileName;
            }

            //запись в txt файл
            using (StreamWriter sw = new StreamWriter(savePath, false, Encoding.Default))
            {
                sw.Write(output);
            }

            button2.IsEnabled = false;
            button1.IsEnabled = true;
            button3.IsEnabled = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //вызов метода возвращающего только радиотелефоны с автоответчиком
            string output = firms.CreateStringRadioPhonesWithAnswerphone(arrayWithAllPhones);
            textBox1.Text = output;

            //запись в txt файл
            using (StreamWriter sw = new StreamWriter(savePath, true))
            {
                sw.Write(output);
            }

            button2.IsEnabled = false;
            button1.IsEnabled = false;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string jsonString = JsonConvert.SerializeObject(firms);
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(TxtFileHandler));

            using (FileStream cr = new FileStream("firms.json", FileMode.OpenOrCreate))
            {
                jsonFormatter.WriteObject(cr, jsonString);
            }
            button3.IsEnabled = false;
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

    [DataContract]
    class TxtFileHandler
    {
        [DataMember]
        public List<IPhone> MainList { get; set; }
        [DataMember]
        public string Output1 { get; set; }
        [DataMember]
        public string Output2 { get; set; }

        public List<IPhone> ReadFromTXTWriteToArray(string Path1, string Path2)
        {
            //отдельный масив путей к файлам
            string[] paths = new string[2] { Path1, Path2 };

            MainList = new List<IPhone>();
            int fileCount = 0;

            //перебирает сначала txt файл по одному пути, затем по другому
            foreach (string path1 in paths)
            {
                ++fileCount;
                //записывает все строчки из файла в переменную lines
                try
                {
                    var lines = File.ReadAllLines(path1);

                    //перенорсит данные из строчки lines в массив
                    foreach (var line in lines)
                    {
                        //Идентификация а затем и распознавание строки
                        if (line.Split(',')[3] == "true" || line.Split(',')[3] == "false")
                        {
                            MainList.Add(new RadioPhone(line.Split(',')[0], line.Split(',')[1],
                            Convert.ToInt32(line.Split(',')[2]), Convert.ToBoolean(line.Split(',')[3]), Convert.ToInt32(line.Split(',')[4])));
                        }
                        else
                        {
                            MainList.Add(new MobilePhone(line.Split(',')[0], line.Split(',')[1],
                            line.Split(',')[2], Convert.ToInt32(line.Split(',')[3]), Convert.ToInt32(line.Split(',')[4])));
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    MessageBox.Show("Нажаль такого файлу не існує,\n або вказано не правильний шлях");
                    return null;
                }

                catch (FormatException)
                {
                    MessageBox.Show($"Не вдається прочитати файл{fileCount}, будь ласка заповніть його наступним чином:\n" +
                        "1. Для радіотелефонів: Назва, Фірма, Радіус дії, Наявність автовідповідача, Ціна\n" +
                        "2. Для мобільних телефонів: Назва, Фірма, Колір, Об'єм пам'яті, Ціна");
                    return null;
                }

                catch (IndexOutOfRangeException)
                {
                    MessageBox.Show($"Не вдається прочитати файл{fileCount}, будь ласка заповніть його наступним чином:\n" +
                        "1. Для радіотелефонів: Назва, Фірма, Радіус дії, Наявність автовідповідача, Ціна\n" +
                        "2. Для мобільних телефонів: Назва, Фірма, Колір, Об'єм пам'яті, Ціна");
                    return null;
                }
                catch
                {
                    MessageBox.Show("Виникла помилка! Спробуйте вказати інший файл");
                    return null;
                }

            }
            return MainList;
        }
        public string CreateStringToOutputAllPhones(List<IPhone> mainList)
        {

            //Перебор масивa для вывода(склеивает строку)
            Output1 = "All phones: \n";
            foreach (var a in mainList)
            {
                var variableType = a.GetType();//определяет тип переменной в данной итерации и формирует соответствующую типу строку
                if (variableType.Name == "MobilePhone")
                {
                    MobilePhone i = (MobilePhone)a;
                    Output1 += $"Name: {i.Name} Firm: {i.Firm} Colour: {i.Colour} Memory capacity{i.MemoryCapacity}  Price: {i.Price}\n";
                }
                else
                {
                    RadioPhone i = (RadioPhone)a;
                    Output1 += $"Name: {i.Name} Firm: {i.Firm} Reach: {i.Reach} Anserphone: {i.Answerphone}  Price: {i.Price}\n";
                }
            }
            return Output1;
        }

        public string CreateStringRadioPhonesWithAnswerphone(List<IPhone> mainList)
        {
            Output2 = "Radio phones with Answerphone: \n";

            foreach (var a in mainList)
            {
                var variableType = a.GetType();
                if (variableType.Name == "RadioPhone") //отделает только радиотелефоны
                {
                    RadioPhone i = (RadioPhone)a;
                    if (i.Answerphone) // отделяет только радиотелефоны с автоответчиком
                    {
                        Output2 += $"Name: {i.Name} Firm: {i.Firm} Reach: {i.Reach} Anserphone: {i.Answerphone}  Price: {i.Price}\n";
                    }
                }
            }
            return Output2;
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
