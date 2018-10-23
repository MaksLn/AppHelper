using AppHelper.JsonClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AppHelper
{
    class WeatherUse
    {
        Dispatcher Dispatcher;

        public WeatherUse(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }

        /// <summary>
        /// получение картинок 
        /// </summary>
        /// <param name="url"></param>
        private void DownloadFile(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                try
                {
                    byte[] ds = webClient.DownloadData(new Uri(url));
                    MemoryStream ms = new MemoryStream(ds);

                    File.Create(Directory.GetCurrentDirectory() + url.Split('/').Last()).Close();
                    File.WriteAllBytes(Directory.GetCurrentDirectory() + url.Split('/').Last(), ds);
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
            }
        }

        /// <summary>
        /// поллучение почасового прогноза
        /// </summary>
        /// <param name="CenterListView"></param>
        public void InDay(ListView CenterListView,string ActiveCity, string Region)
        {

            JsonClassOnDay.JsonUseOnDay json = new JsonClassOnDay.JsonUseOnDay(ActiveCity,Region);
            JsonClassOnDay.RootObject root = null;

            Task task = new Task(() =>
            {
            root = json.JsonParse();

            ObservableCollection<Weather> weathers = new ObservableCollection<Weather>();

            foreach (var i in root.hourly_forecast)
            {
                  if(!File.Exists(Directory.GetCurrentDirectory() + i.icon_url.Split('/').Last()))
                    { 
                        DownloadFile(i.icon_url.Replace("/k/","/a/"));
                    }

                    weathers.Add(new Weather()
                    {
                        weekday = i.FCTTIME.civil+"\n"+i.FCTTIME.mday_padded+" "+i.FCTTIME.month_name,
                        Conditions = i.condition,
                        temper = i.temp.metric + " C°",
                        wind = i.wspd.metric+" KpH",
                        image = Directory.GetCurrentDirectory() + i.icon_url.Split('/').Last(),
                        dir = i.wdir.dir
                    });
                }

                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    CenterListView.ItemsSource = weathers;
                }));
            });

            task.Start();
        }
        
        /// <summary>
        /// получения прогноза на 10 дней
        /// </summary>
        /// <param name="CenterListView"></param>
        public void InFourDay(ListView CenterListView, string ActiveCity, string Region)
        {

            JsonUse json = new JsonUse(ActiveCity, Region);
            RootObject root = null;

            Task task = new Task(() =>
            {
                root = json.JsonParse();
               
                ObservableCollection<Weather> weathers = new ObservableCollection<Weather>();

                foreach (var i in root.forecast.simpleforecast.forecastday)
                {
                    if (!File.Exists(Directory.GetCurrentDirectory() + i.icon_url.Split('/').Last()))
                    {
                        DownloadFile(i.icon_url);
                    }

                    weathers.Add(new Weather()
                    {
                        weekday = i.date.weekday,
                        Conditions = i.conditions,
                        temper = i.low.celsius + " - " + i.high.celsius + " C°",
                        wind = i.avewind.kph.ToString() + " - " + i.maxwind.kph + " KpH",
                        image = Directory.GetCurrentDirectory() + i.icon_url.Split('/').Last(),
                        dir = i.avewind.dir
                    });
                }

                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    CenterListView.ItemsSource = weathers;
                }));
            });

            task.Start();
        }
    }
}
