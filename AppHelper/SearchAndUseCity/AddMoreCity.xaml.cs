using System;
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
using System.Windows.Shapes;

namespace AppHelper.SearchAndUseCity
{
    /// <summary>
    /// Логика взаимодействия для AddMoreCity.xaml
    /// </summary>
    public partial class AddMoreCity : Window
    {
        public List<string> ListWithCity;
        private RootObject result;

        public AddMoreCity(List<string> listCitys)
        {
            ListWithCity = listCitys;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SearchCityClass searchCityClass = new SearchCityClass(TextBoxForSearch.Text.ToString());
            result= searchCityClass.JsonParse<RootObject>();
            var result1 = searchCityClass.JsonParse<RootObject1>();

            if (result.response.results != null)
            {
                ResultSearchActiveCity.Items.Clear();
                foreach (var i in result.response.results) {
                    ResultSearchActiveCity.Items.Add(new Label() { Content = i.city+" "+i.country_name+" "+i.state });
                }
            }
            else if (result1.current_observation != null)
            {
                ListWithCity.Insert(0, result1.current_observation.display_location.city + " " + result1.current_observation.display_location.country_iso3166);
                AddMoreCityWind.Close();
            }
            else
            {
                ResultSearchActiveCity.Items.Clear();
                ResultSearchActiveCity.Items.Add(new Label() { Content = "Not Found" });
            }

            ActiveCity.ListWithCity = ListWithCity;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta / 8);
            e.Handled = true;
        }

        private void GridViewColumn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var itemselect=((sender as  ListView).SelectedItem as Label).Content.ToString();

            foreach (var i in result.response.results)
            {
                if (i.city.Contains(itemselect.Split(' ').First())&&
                    i.country_name.Contains(itemselect.Split(' ').Take(2).Last())&&
                    i.state.Contains(itemselect.Split(' ').Take(3).Last()))
                {
                    ListWithCity.Insert(0, i.city+" "+i.country_iso3166);
                }
            }
            AddMoreCityWind.Close();
        }

    }
}
