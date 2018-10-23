using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace AppHelper.SearchAndUseCity
{
    /// <summary>
    /// Логика взаимодействия для ChoseCiti.xaml
    /// </summary>
    public partial class ChoseCiti : Window
    {
        public string content = "fsdafas";
        public List<string> ListWithCity;
        SearchAndUseCity.ActiveCity city;
        ObservableCollection<Label> ActiveCitys;
        ToolBar ToolBar;

        public ChoseCiti(ToolBar toolBar)
        {
            InitializeComponent();

            ActiveCitys = new ObservableCollection<Label>();
            city = new ActiveCity();

            foreach (var i in city.GetNameCity())
                ActiveCitys.Add(new Label() { Content = i });

            ActiveCitys.Add(new Label() {Content="+ Add More" });
            ActiveCity.ItemsSource=(ActiveCitys);

            ListWithCity = new List<string>();
            ListWithCity.AddRange(city.GetNameCity());

            ToolBar = toolBar;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ChoseCityWind.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            city.UpdateCityInFile(ListWithCity);
            city.UpdateToolBar(ToolBar);
            ChoseCityWind.Close();
        }

        private void GridViewColumn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListView listView = sender as ListView;
                var Item = (Label)listView.SelectedItem;
                if (Item != null && Item.Content.ToString() != "+ Add More")
                {
                 MessageBoxResult boxResult = MessageBox.Show("Do you want to delete this city?", "Delete City", MessageBoxButton.OKCancel, MessageBoxImage.Information,
                        MessageBoxResult.Cancel, MessageBoxOptions.ServiceNotification);
                    if (boxResult == MessageBoxResult.OK)
                    {
                        ListWithCity.Remove(Item.Content.ToString());

                        ActiveCitys.Clear();

                        foreach (var i in ListWithCity)
                            ActiveCitys.Add(new Label() { Content = i });
                        ActiveCitys.Add(new Label() { Content = "+ Add More" });
                    }
                }
                else if(Item != null && Item.Content.ToString() == "+ Add More")
                {
                    AddMoreCity addMoreCity = new AddMoreCity(ListWithCity);
                    addMoreCity.Owner = this;
                    addMoreCity.ShowDialog();

                    ListWithCity.Concat(SearchAndUseCity.ActiveCity.ListWithCity);
                    ActiveCitys.Clear();

                    foreach (var i in ListWithCity)
                        ActiveCitys.Add(new Label() { Content = i });
                    ActiveCitys.Add(new Label() { Content = "+ Add More" });
                }
            }
            catch { }
            ActiveCity.SelectedItem = null;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta / 8);
            e.Handled = true;
        }
    }
}
