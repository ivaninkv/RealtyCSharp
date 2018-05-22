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
        /// <param name="searchurl">Поисковый запрос, который нужно распарсить.</param>
        /// <param name="delay">Задержка между запросами, с.</param>
        public Avito(string searchurl, int delay = 3) : base(searchurl)
        {
            Delay = delay;
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
            dt.Columns.Add("FloorAll");
            foreach (DataRow row in dt.Rows)
            {                
                row["Area"] = row["Area"].ToString().Replace("м²", String.Empty).Replace(".", ",").Trim();
                row["Price"] = row["Price"].ToString().Replace("р. в месяц", String.Empty).Replace(".", ",").Trim();
                row["Floor"] = row["Floor"].ToString().Replace(" эт.", String.Empty).Trim();
                row["FloorAll"] = row["Floor"].ToString().Split('/')[1];
                row["Floor"] = row["Floor"].ToString().Split('/')[0];

            }
            
            dt.ChangeColumnDataType("Area", typeof(decimal));
            dt.ChangeColumnDataType("Price", typeof(decimal));
            dt.ChangeColumnDataType("Floor", typeof(int));
            dt.ChangeColumnDataType("FloorAll", typeof(int));

            return dt;
        }
        #endregion

    }
}
