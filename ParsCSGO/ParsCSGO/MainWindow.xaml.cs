using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ParsCSGO
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Parser p = new Parser();
        Thread myThread;

        public MainWindow()
        {
            InitializeComponent();            
            logList.ItemsSource = p.db;
            myThread = new Thread(new ThreadStart(p.ParsePage));
            p.mainList = logList;              
                        
        }        

        private void Button_Click(object sender, RoutedEventArgs e)
        {            
            myThread.Start();
        }       
       

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var x = myThread.IsAlive;
            int a = 0;
            logList.Items.Refresh();
        }
    }
}
