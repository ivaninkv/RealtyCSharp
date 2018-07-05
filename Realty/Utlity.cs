using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Realty
{
    /// <summary>
    /// Различные вспомогательные утилиты
    /// </summary>
    public static class Utlity
    {
        #region Методы
        /// <summary>
        /// Экспортирует DataTable в *.csv файл
        /// </summary>
        /// <param name="dtDataTable">DataTable для экспорта</param>
        /// <param name="strFilePath">Путь для сохранения файла</param>
        /// <param name="delimiter">Разделитель полей</param>
        public static void ToCSV(this DataTable dtDataTable, string strFilePath, string delimiter = ";")
        {
            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers  
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(delimiter);
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(delimiter);
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }

        /// <summary>
        /// Меняет тип столбца в DataTable
        /// </summary>
        /// <param name="table">DataTable, в котором нужно изменить тип столбца</param>
        /// <param name="columnName">Название столбца</param>
        /// <param name="newType">Новый тип столбца</param>        
        public static void ChangeColumnDataType(this DataTable table, string columnName, Type newType)
        {
            if (table.Columns.Contains(columnName) == false)
                return;

            DataColumn column = table.Columns[columnName];
            if (column.DataType == newType)
                return;

            try
            {
                DataColumn newcolumn = new DataColumn("temporary", newType);
                table.Columns.Add(newcolumn);
                foreach (DataRow row in table.Rows)
                {
                    try
                    {
                        row["temporary"] = Convert.ChangeType(row[columnName], newType);
                    }
                    catch
                    {
                    }
                }
                table.Columns.Remove(columnName);
                newcolumn.ColumnName = columnName;
            }
            catch (Exception)
            {
                return;
            }

            return;
        }

        /// <summary>
        /// Удаляет множественные пробельные символы.
        /// </summary>
        /// <param name="str">Исходная строка</param>
        /// <returns>Корректная строка</returns>
        public static string RemoveIncorrectChars(this string str)
        {
            return Regex.Replace(str, @"\s+", " ");
        }
        
        /// <summary>
        /// Группирует DataTable по указанному столбцу, применяя агрегирующую функцию для другого столбца
        /// </summary>
        /// <param name="table">Исходный DataTable</param>
        /// <param name="groupColumn">Название столбца для группировки</param>
        /// <param name="calcColumn">Название столбца для вычисления</param>
        /// <param name="func">Агрегатная функция</param>
        /// <returns>Сгруппированный DataTable</returns>
        public static DataTable GropupBy(this DataTable table, string groupColumn, string calcColumn, string func = "AVG")
        {
            DataView dv = table.DefaultView;
            dv.Sort = groupColumn;            
            DataTable result_dt = dv.ToTable(true, groupColumn);
            result_dt.Columns.Add("CalcValue", typeof(decimal));

            foreach (DataRow dr in result_dt.Rows)
            {
                var avg = table.Compute($"{func}({calcColumn})", $"{groupColumn}={dr[groupColumn]}");                
                dr["CalcValue"] = (decimal)avg;                
            }
            
            return result_dt;
        }
        #endregion
    }
}
