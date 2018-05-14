using System.Data;

namespace Realty
{
    /// <summary>
    /// Класс для работы с сайтом cian.ru
    /// </summary>
    public class Avito : Parser
    {
        #region Конструкторы
        /// <summary>
        /// Конструктор, в качестве параметра принимает поисковую строку
        /// </summary>
        /// <param name="searchurl">Поисковый запрос, который нужно распарсить.</param>
        public Avito(string searchurl, int delay = 4) : base(searchurl)
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

            return dt;
        }
        #endregion

    }
}
