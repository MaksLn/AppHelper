using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AppHelper.SearchAndUseCity
{
    class ActiveCity
    {
        private List<Button> buttons;
        public static List<string> ListWithCity;

        public ActiveCity()
        {
            buttons = new List<Button>();

            var CityInFile= File.ReadAllText(@"ActiveCityFile.txt");
            foreach(var i in CityInFile.Split('|'))
            {
                if (i != "")
                {
                    var butt = new Button() { FontSize = 14, Content = i, Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")) };
                    buttons.Add(butt);
                }
            }
        }

        private void Butt1_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MainWindow.ActiveCityNow = ((Button)sender).Content.ToString();
        }

        public void AddToToolBar(ToolBar toolBar)
        {
            foreach (var i in buttons)
            {
                toolBar.Items.Insert(0,i);
                toolBar.Items.Insert(1,new Separator());
            }
        }

        public List<string> GetNameCity()
        {
            var list = new List<string>();

            foreach (var i in buttons)
            {
                list.Add(i.Content.ToString());
            }

            return list;
        }

        public void UpdateCityInFile(List<string> Citys)
        {
            File.CreateText(@"ActiveCityFile.txt").Close();

            foreach (var i in Citys)
            {
                File.AppendAllText(@"ActiveCityFile.txt", i + "|");
            }
        }

        public void UpdateToolBar(ToolBar toolBar)
        {
          
                var CityInFile = File.ReadAllText(@"ActiveCityFile.txt");
                buttons.Clear();

                foreach (var i in CityInFile.Split('|'))
                {
                    if (i != "")
                    {
                        var butt = new Button() { FontSize = 14, Content = i, Foreground
                            = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")) };
                        buttons.Add(butt);
                    }
                }

                for(int i=0;i<toolBar.Items.Count;i++)
                {
                    if ((toolBar.Items[i] as Button) != null && (toolBar.Items[i] as Button).Content.ToString() != "+")
                    {
                        toolBar.Items.Remove(toolBar.Items[i]);
                        i--;
                    }
                    else if ((toolBar.Items[i] is Separator))
                    {
                        toolBar.Items.Remove(toolBar.Items[i]);
                        i--;
                    }
                }

                foreach (var i in buttons)
                {
                        toolBar.Items.Insert(0, i);
                        toolBar.Items.Insert(1, new Separator());
                }
        }
    }
}
