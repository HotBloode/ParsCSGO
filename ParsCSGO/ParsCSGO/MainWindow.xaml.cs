using Microsoft.Win32;
using System.Threading;
using System.Windows;

namespace ParsCSGO
{    
    public partial class MainWindow : Window
    {
        static Parser p = new Parser();
        static Thread myThread = new Thread(new ThreadStart(p.ParsePage));

        public MainWindow()
        {
            InitializeComponent();   
            //Связываем таблицу с Репозиторием кодов
            logList.ItemsSource = p.db;
          
            //Сылка на список в окне
            p.mainList = logList;
            //Поле с к-вом кодов
            p.Text = TextCount;
            
            //Цепляем ивенты к собитию выгрузки и загрузки пользователя винды
            SystemEvents.SessionSwitch += OnIn;
            SystemEvents.SessionEnding += OnOut;
        }

        static void OnIn(object sender, SessionSwitchEventArgs e)
        {
            //Запускаем поток после входа в профиль
            myThread.Resume();
        }
        static void OnOut(object sender, SessionEndingEventArgs e)
        {
            //Останавливаем поток перед выходом из профиля
            myThread.Suspend();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Проверяем к-во минут (форум банит за однотипные* запросы меньше 7 минут)
            if (System.Convert.ToInt32(TimerBox.Text) < 7)
            {
                MessageBox.Show("Значение пазуы должно быть больше 7 минут");
            }
            else
            {
                //Переводим минуты в милисекунды
                p.timer = System.Convert.ToInt32(TimerBox.Text) * 60000;
                //Делаем кнопку и поле ввода неактивными
                StartButton.IsEnabled = false;
                TimerBox.IsEnabled = false;
                //Запускаем парсер в отдельном потоке
                myThread.Start();                
            }
        }       
       

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Очистка, обновление
            p.db.Clear();
            logList.Items.Refresh();
            TextCount.Text = "Всего кодов: " + p.db.Count;
        }

        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //Копируем в буфер обмена выделенное моле с кодом
            Clipboard.SetText(TextToCopy.Text);
        }
    }
}
