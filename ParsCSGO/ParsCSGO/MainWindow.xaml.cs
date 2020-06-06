using Newtonsoft.Json;
using System.IO;
using System.Windows;

namespace ParsCSGO
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Parser p = new Parser();
        public MainWindow()
        {
            InitializeComponent();            
            logList.ItemsSource = p.db;
        }
        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            p.Parse();           
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            logList.Items.Refresh();
        }
    }
}
