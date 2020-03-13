using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace GaussJordan
{
    public partial class MainForm : Form
    {
        private int M = 0;

        private DataView matrixView;
        private DataView resultView;
        private LinearSystem matrix;
        private GaussJordanSolver solver;

        public MainForm()
        {
            InitializeComponent();
        }

        private void initButton_Click(object sender, EventArgs e)
        {
            try
            {
                M = Int32.Parse(sizeTextBox.Text);
                if (M < 1) throw new Exception("Нельзя инициализировать безразмерную матрицу.");

                dataGridView1.Columns.Clear();
                dataGridView1.Refresh();

                matrix = new LinearSystem(M);
                matrixView = new DataView(LinearSystem.matrixTable);
                dataGridView1.DataSource = matrixView;

                resultView = new DataView(LinearSystem.resultTable);
                dataGridView2.DataSource = resultView;

                if (!solveButton.Enabled) solveButton.Enabled = true;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                    ); 
            }

        }
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show(
                "Введите целое или вещественное число, разделяя дробную и целую часть запятой.",
                "Ошибка ввода данных",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
                );
        }
        private void dataGridView1_ColumnPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            dataGridView1.Columns["b[i]"].DefaultCellStyle.BackColor = Color.Beige;          
        }
        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            dataGridView1.Columns[e.Column.Index].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        private void solveButton_Click(object sender, EventArgs e)
        {
            double[,] array = new double[dataGridView1.RowCount, dataGridView1.ColumnCount];
            Debug.WriteLine("rows: " + dataGridView1.RowCount + " cols: " + dataGridView1.ColumnCount);
            foreach (DataGridViewRow i in dataGridView1.Rows)
            {
                if (i.IsNewRow) continue;
                foreach (DataGridViewCell j in i.Cells)
                {
                    array[j.RowIndex, j.ColumnIndex] = Convert.ToDouble(j.Value);
                }
            }

            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    Debug.Write(array[i, j] + " ");
                }
                Debug.WriteLine("");
            }

            try
            {
                solver = new GaussJordanSolver(M, array);
                matrix.InitResultView();
                matrix.WriteLog();
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Введенная система линейных алгебраических уравнений " +
                    "имеет бесконечное количество решений или не имеет их вовсе.",
                    "Исключение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                    );
            }

        }
    }
}
