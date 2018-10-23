using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AppHelper
{
    class Weather
    {
        private string conditions;

        public string temper { get; set; }
        public string image { get; set; }
        public string wind { get; set; }
        public string Conditions
        {
            get
            {
                return conditions;
            }
            set
            {
                if (value.Length < 15)
                {
                    conditions = value;
                }
                else
                {
                    conditions = value.TrimEnd(value.Split(' ').Last().ToCharArray());
                    conditions+=value.Split(' ').Select(e=>e).Last().Insert(0, "\n");
                }
            }
        }
        public string weekday { get; set; }
        public string dir { get; set; }
        public string DayOrHour = "Day of week";
    }
}
