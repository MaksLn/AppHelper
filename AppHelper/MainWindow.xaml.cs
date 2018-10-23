using AppHelper.JsonClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
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
using System.Threading;
using System.Collections.ObjectModel;
using System.IO;

namespace AppHelper
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string DayOrHour = "Day of week";
        WeatherUse weatherUse;
        public static string ActiveCityNow = "Polatsk";

        public MainWindow()
        {
            InitializeComponent();

            SizeChanged += MainWindow_SizeChanged;

            ButtonNow.Content = "Hourly Forecast";
            ButtonDay3.Content = "On Ten Day";

            weatherUse = new WeatherUse(Dispatcher);

            CenterListView.SelectionChanged += (s, e) => CenterListView.SelectedItem = null;

            ((GridView)CenterListView1.View).Columns.First().Header = DayOrHour;

            SearchAndUseCity.ActiveCity activeCity = new SearchAndUseCity.ActiveCity();
            activeCity.AddToToolBar(TopToolBar);
            var butt = new Button()
            {
                FontSize = 14,
                Width = 25,
                VerticalContentAlignment = VerticalAlignment.Top,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                Content = "+",
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"))
            };
            butt.Click += ButtonAdd_Click;
            TopToolBar.Items.Add(butt);

            foreach (var i in TopToolBar.Items)
            {
                if ((i as Button) != null && (i as Button).Content.ToString() != "+")
                {
                    ((Button)i).Click += Butt_Click;
                }
            }
        }

        public delegate void InvokeDelegate();

        double MarginY = 0;

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Thickness thickness = new Thickness(-MainWind.ActualWidth / 4, MainWind.ActualHeight / 2 - 30, 0, 0);

            MainTool.Width = MainWind.ActualHeight - 30;
            MainTool.Padding = thickness;

            CenterGrid.Width = MainWind.ActualWidth - 54;
            TopToolBar.Width = MainWind.ActualWidth - 52;

            ButtonNow.Width = (MainWind.ActualWidth - 54) / 3;
            ButtonDay3.Width = (MainWind.ActualWidth - 54) / 3;

            MarginY = (MainWind.ActualHeight - 500);
            Thickness thickness1 = new Thickness(0, 26, 0, -MarginY);

            CenterScrol.Width = MainWind.ActualWidth - 54;
            CenterScrol.Height = (MainWind.ActualHeight) - MarginY / 1.8;
            CenterScrol.Margin = thickness1;
        }

        private void MainTool_Loaded(object sender, RoutedEventArgs e)
        {
            ToolBar toolBar = sender as ToolBar;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
            var mainPanelBorder = toolBar.Template.FindName("MainPanelBorder", toolBar) as FrameworkElement;
            if (mainPanelBorder != null)
            {
                mainPanelBorder.Margin = new Thickness();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var butt = new Button() { Content = "City", Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")) };
            TopToolBar.Items.Add(butt);
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            SearchAndUseCity.ChoseCiti choseCiti = new SearchAndUseCity.ChoseCiti(TopToolBar);
            choseCiti.Owner = this;
            try
            {
                choseCiti.ShowDialog();
                foreach (var i in TopToolBar.Items)
                {
                    if ((i as Button) != null && (i as Button).Content.ToString() != "+")
                    {
                        ((Button)i).Click += Butt_Click;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButtonNow_Click(object sender, RoutedEventArgs e)
        {
            CenterListView.ItemsSource = null;
            weatherUse.InDay(CenterListView, string.Join(" ",
                ActiveCityNow.Split(' ').TakeWhile((x, i) => i < ActiveCityNow.Split(' ').Length - 1)),
                ActiveCityNow.Split(' ').Last());

            DayOrHour = "Hour";
            ((GridView)CenterListView1.View).Columns.First().Header = DayOrHour;
        }

        private void ButtonDay3_Click(object sender, RoutedEventArgs e)
        {
            CenterListView.ItemsSource = null;
            weatherUse.InFourDay(CenterListView,
                string.Join(" ", ActiveCityNow.Split(' ').TakeWhile((x, i) => i < ActiveCityNow.Split(' ').Length - 1)),
                ActiveCityNow.Split(' ').Last());

            DayOrHour = "Day of week";
            ((GridView)CenterListView1.View).Columns.First().Header = DayOrHour;
        }

        private void CenterScrol_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta / 8);
            e.Handled = true;
        }

        public void Butt_Click(object sender, RoutedEventArgs e)
        {
            foreach (var i in TopToolBar.Items)
            {
                if(i is Button)
                (i as Button).Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#202225"));
            }

            ActiveCityNow = ((Button)sender).Content.ToString();
            (sender as Button).Background= new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2F3136"));
        }
    }
}
