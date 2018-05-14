using System;
using System.Data;
using System.IO;
using System.Linq;

namespace Realty
{
    /// <summary>
    /// Класс для работы с *.csv
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
        /// <param name="columnname">Название столбца</param>
        /// <param name="newtype">Новый тип столбца</param>
        /// <returns></returns>
        public static bool ChangeColumnDataType(DataTable table, string columnname, Type newtype)
        {
            if (table.Columns.Contains(columnname) == false)
                return false;

            DataColumn column = table.Columns[columnname];
            if (column.DataType == newtype)
                return true;

            try
            {
                DataColumn newcolumn = new DataColumn("temporary", newtype);
                table.Columns.Add(newcolumn);
                foreach (DataRow row in table.Rows)
                {
                    try
                    {
                        row["temporary"] = Convert.ChangeType(row[columnname], newtype);
                    }
                    catch
                    {
                    }
                }
                table.Columns.Remove(columnname);
                newcolumn.ColumnName = columnname;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
