using AdminPanel.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserPanel.Services;

namespace AdminPanel.Services
{
    public class CreateStatisticService
    {

        public static void SetStatistic(float price)
        {
            string[] dir = Directory.GetCurrentDirectory().Split('\\');
            string path = "";
            foreach (var item in dir)
            {
                if (item.ToLower() == "eyetaxi")
                    break;
                path += item + "\\";
            }


            Statistic statistic = JsonSaveService<Statistic>.Load(path + @"\EyeTaxi\AdminPanel\bin\Debug\statistic");

            if(statistic == null)
                statistic = new Statistic();


            if (statistic.Month == DateTime.Now.Month)
                statistic.MonthlyIncome += price;
            else
            {
                statistic.Month = DateTime.Now.Month;
                statistic.MonthlyIncome = price;
            }

            if (statistic.Day == DateTime.Now.Day)
                statistic.DailyIncome += price;
            else
            {
                statistic.DailyIncome = price;
                statistic.Day = DateTime.Now.Day;
            }

            statistic.IncomeAfterLWD += price;
            statistic.Interest = (statistic.IncomeAfterLWD * 0.15f).ToString("0.##");

            JsonSaveService<Statistic>.Save(statistic, path + @"\EyeTaxi\AdminPanel\bin\Debug\statistic");

        }
        
    }
}
