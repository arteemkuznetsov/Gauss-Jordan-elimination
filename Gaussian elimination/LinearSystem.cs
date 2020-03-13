using System;
using System.Data;
using System.IO;

namespace GaussJordan
{
    public class LinearSystem // класс создает пользовательское представление СЛАУ в виде таблицы
    {
        public int M; // поле - размер матрицы
        DataSet data; // класс, в котором хранятся таблицы (аналогично базе данных)
        public static DataTable matrixTable; // табличное отображение матрицы
        public static DataTable resultTable; // вектор корней
        DataRow row; 
        DataColumn column;

        public LinearSystem(int M) // конструктор, позволяющий создать матрицу с заданным параметром
        {
            data = new DataSet();

            matrixTable = new DataTable("Matrix"); // создание таблицы-матрицы
            data.Tables.Add(matrixTable);

            resultTable = new DataTable("Result");
            data.Tables.Add(resultTable);

            for (int j = 0; j < M; j++) // в цикле создаем столбцы
            {
                column = new DataColumn("x[" + j + "]"); // создаем новый столбец с именем x[j]
                column.DataType = Type.GetType("System.Decimal"); // тип данных - вещественные числа
                column.DefaultValue = 0;
                matrixTable.Columns.Add(column); // добавляем его в матрицу

                column = new DataColumn("x[" + j + "]"); // создаем новый столбец с именем x[j]
                resultTable.Columns.Add(column);
            }

            // далее создаем колонку свободных членов

            column = new DataColumn("b[i]");
            column.DataType = Type.GetType("System.Decimal");
            column.DefaultValue = 0;
            matrixTable.Columns.Add(column);

            for (int i = 0; i < M; i++) // в цикле создаем строки
            {
                row = matrixTable.NewRow(); // создаем новую строку в матрице
                matrixTable.Rows.Add(row); // добавляем строку в матрицу
            }

            row = resultTable.NewRow();
            resultTable.Rows.Add(row);

            this.M = M; // инициализируем значение поля, обозначенного ключевым словом this, переданным аргументом
        }

        public void InitResultView()
        {

            for (int i = 0; i < M; i++)
            {
                row[i] = GaussJordanSolver.resultVector[i];
            }
        }

        public void WriteLog()
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(
                    "Gauss-Jordan_log.txt", true, System.Text.Encoding.Default))
                {
                    streamWriter.Write(DateTime.Now + "     ");
                    for (int i = 0; i < M; i++)
                        streamWriter.Write("x[" + i + "] : " + row[i] + "   ");
                    streamWriter.WriteLine("");
                }
            }
            catch (Exception) { }
        }
    }
}
