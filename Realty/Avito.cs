using System;
using System.Data;

namespace Realty
{
    /// <summary>
    /// Класс для работы с сайтом avito.ru
    /// </summary>
    public class Avito : Parser
    {
        #region Конструкторы
        /// <summary>
        /// Конструктор, в качестве параметра принимает поисковую строку
        /// </summary>
        /// <param name="searchUrl">Поисковый запрос, который нужно распарсить.</param>
        /// <param name="delay_s">Задержка между запросами, с.</param>
        /// <param name="useProxy">Испмользовать ли прокси.</param>
        public Avito(string searchUrl, int delay_s = 3, bool useProxy = false) : base(searchUrl, useProxy)
        {
            if (SearchUrl.IndexOf('?') == -1)
                SearchUrl += "?view=list";
            else
                SearchUrl += "&view=list";

            Delay_s = delay_s;
            Floor = ".floor";
            Link = ".description-title-link";
            Area = ".area";
            Address = ".address";
            Price = ".price";
        }
        #endregion

        #region Методы
        /// <summary>
        /// Парсит результаты запроса
        /// </summary>
        /// <returns>Дататейбл с результатами запроса</returns>
        public override DataTable Parse()
        {
            var dt = new DataTable();
            dt = base.Parse();
            dt.Columns.Add("Floor_All");
            
            foreach (DataRow row in dt.Rows)
            {
                row["Area"] = row["Area"].ToString().Replace("м²", String.Empty).Replace(".", ",").Trim();
                row["Price"] = row["Price"].ToString().Replace("р. в месяц", String.Empty).Replace(".", ",").Trim();
                row["Floor"] = row["Floor"].ToString().Replace(" эт.", String.Empty).Trim();
                row["Floor_All"] = row["Floor"].ToString().Split('/')[1];
                row["Floor"] = row["Floor"].ToString().Split('/')[0];

            }

            dt.ChangeColumnDataType("Area", typeof(decimal));
            dt.ChangeColumnDataType("Price", typeof(decimal));
            dt.ChangeColumnDataType("Floor", typeof(int));
            dt.ChangeColumnDataType("Floor_All", typeof(int));

            dt.Columns.Add("Price_by_meter", typeof(decimal));
            foreach (DataRow row in dt.Rows)
            {
                row["Price_by_meter"] = (decimal)row["Price"] / (decimal)row["Area"];
            }

            return dt;
        }
        #endregion
    }
}
