using System;
using System.Data;
using System.IO;
using System.Linq;

namespace Realty
{
    /// <summary>
    /// Класс для работы с *.csv
    /// </summary>
    public static class CSVUtlity
    {
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
    }
}
