using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppHelper.SearchAndUseCity
{
    class SearchCityClass
    {
        private string ActiveC;

        public SearchCityClass(string ActiveCity)
        {
            ActiveC = ActiveCity;
        }

        public string HttpCl()
        {
            string Content = null;

            try
            {
                using (var wc = new WebClient())
                {
                    Content = wc.DownloadString("http://api.wunderground.com/api/54a567efd367fe0a/conditions/q/CA/" + ActiveC + ".json");
                    return Content;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
            }

            return Content;
        }

        public T JsonParse<T>() where T : class
        {
            try
            {
                var jobject = JsonConvert.DeserializeObject<T>(HttpCl());
                return jobject;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
            }

            return null;
        }
    }
}
