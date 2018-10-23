using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AppHelper.JsonClassOnDay
{
    class JsonUseOnDay
    {
        private string ActiveC;
        private string Region;

        public JsonUseOnDay(string ActiveCity, string region)
        {
            ActiveC = ActiveCity;
            Region = region;
        }
            public string HttpCl()
            {
                string Content = null;

                try
                {
                    using (var wc = new WebClient())
                    {
                        Content = wc.DownloadString("http://api.wunderground.com/api/54a567efd367fe0a/hourly/q/"+Region+"/" + ActiveC+".json");
                        return Content;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
                }

                return Content;
            }

            public RootObject JsonParse()
            {
                try
                {
                    var jobject = JsonConvert.DeserializeObject<RootObject>(HttpCl());
                    return jobject;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
                }

                return new RootObject();
            }
        }
    }



