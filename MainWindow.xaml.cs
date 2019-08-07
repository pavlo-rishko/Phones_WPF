using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Phones_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

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

    class MobilePhone : ArrayHandler, IPhone
    {
        string name, firm, colour;
        int price, memoryCapacity;

        public MobilePhone() { }
        public MobilePhone(string Name, string Firm, int Price, string Colour, int MemoryCapacity)
        {
            name = Name;
            firm = Firm;
            price = Price;
            colour = Colour;
            memoryCapacity = MemoryCapacity;
        }

        public string Name { get; set; }
        public string Firm { get; set; }
        public int Price { get; set; }

        public string Colour { get; set; }
        public int MemoryCapacity { get; set; }
    }

    class RadioPhone : ArrayHandler, IPhone
    {
        string name, firm;
        int price, reach;
        bool answerPhone;

        public RadioPhone() { }
        public RadioPhone(string Name, string Firm, int Price, int Reach, bool AnswerPhone)
        {
            name = Name;
            firm = Firm;
            price = Price;
            reach = Reach;
            answerPhone = AnswerPhone;
        }

        

        //ArrayHandler i = new ArrayHandler();
        //public void op ()
        //{
        //    i.Phones[0]= 
        //}

        public string Name { get; set; }
        public string Firm { get; set; }
        public int Price { get; set; }

        public int Reach { get; set; }
        public bool AnswerPhone { get; set; }
    }

    //class ArrayHandler
    //{
    //    ArrayList phones = new ArrayList();

        

    //    public ArrayList Phones(object i)
    //    {
            
    //        set { phones.Add(i) }
    //    }


    //}

}
