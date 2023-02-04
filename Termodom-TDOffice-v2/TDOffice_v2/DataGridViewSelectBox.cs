using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TDOffice_v2
{
    /// <summary>
    /// Predefinisana forma za prikazivanje DataTable-a
    /// </summary>
    public partial class DataGridViewSelectBox : Form
    {
        public delegate void RowSelectEventHanlder(RowSelectEventArgs args);
        public RowSelectEventHanlder OnRowSelected { get; set; }
        private DataTable _dt { get; set; } = new DataTable();

        public DataGridViewSelectionMode SelectionMode
        {
            get
            {
                return dataGridView1.SelectionMode;
            }
            set
            {
                dataGridView1.SelectionMode = value;
            }
        }
        public bool RowHeaderVisible
        {
            get
            {
                return dataGridView1.RowHeadersVisible;
            }
            set
            {
                dataGridView1.RowHeadersVisible = value;
            }
        }
        public bool CloseOnSelect { get; set; } = false;

        public class RowSelectEventArgs
        {
            public DataRow SelectedRow { get; set; }
        }

        private DataTable _dataTable { get; set; }
        private DataTable dataGridViewDataTable { get; set; }

        public DataGridViewSelectBox(DataTable source)
        {
            InitializeComponent();

            _dataTable = source;

            dataGridView1.Visible = false;
            dataGridView1.DataSource = source;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            _dt = source;

            if (_dataTable.Rows.Count == 0)
                return;

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                cmb_PoljePretrage.Items.Add(col.Name);
                if (col.ValueType == typeof(double) || col.ValueType == typeof(decimal) || col.ValueType == typeof(float))
                {
                    col.DefaultCellStyle.Format = "#,##0.00";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (col.ValueType == typeof(Int64) || col.ValueType == typeof(Int32) || col.ValueType == typeof(Int16))
                {
                    col.DefaultCellStyle.Format = "#,##0";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
            dataGridView1.Visible = true;

            cmb_PoljePretrage.SelectedIndex = 0;
            slogova_lbl.Text = "Slogova: " + _dt.Rows.Count.ToString();
        }

        private void InvokeOnRowSelect()
        {
            if (OnRowSelected == null)
                return;

            DataRow selectedRow = (dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].DataBoundItem as DataRowView).Row;
            OnRowSelected(new RowSelectEventArgs()
            {
                SelectedRow = selectedRow
            });

            if (CloseOnSelect)
                this.Close();
        }
        private void PretragaEnter()
        {
            dataGridView1.ClearSelection();
            string kolona = cmb_PoljePretrage.SelectedItem.ToString();
            string input = txt_Pretraga.Text;

            if (string.IsNullOrWhiteSpace(input))
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = 0;
                dataGridView1.Rows[0].Selected = true;
                dataGridView1.Focus();
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[kolona];
                return;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string vrednostCelije = row.Cells[kolona].Value.ToString();
                if (vrednostCelije.ToLower().IndexOf(input.ToLower()) == 0)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = row.Index > 0 ? row.Index - 1 : 0;
                    dataGridView1.Rows[row.Index].Selected = true;
                    dataGridView1.Focus();
                    dataGridView1.CurrentCell = dataGridView1.Rows[row.Index].Cells[kolona];
                    return;
                }
            }
        }
        private void PretragaCtrlA()
        {
            string selectString = "";
            string input = txt_Pretraga.Text;
            string[] inputElemets = input.Split('+');

            foreach (object o in cmb_PoljePretrage.Items)
            {
                for (int i = 0; i < inputElemets.Length; i++)
                    selectString += "CONVERT(" + o.ToString() + ", System.String) LIKE '%" + inputElemets[i] + "%' AND ";

                selectString = selectString.Remove(selectString.Length - 4);
                selectString += " OR ";
            }

            selectString = selectString.Remove(selectString.Length - 4);
            DataTable dataTable = _dt;
            DataRow[] rows = dataTable.Copy().Select(selectString);
            dataTable = rows == null || rows.Count() == 0 ? null : rows.CopyToDataTable();

            dataGridView1.DataSource = dataTable;
            slogova_lbl.Text = "Slogova: " + rows.Count().ToString();
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            InvokeOnRowSelect();
        }
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                InvokeOnRowSelect();
        }

        private void txt_Pretraga_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                PretragaEnter();
                dataGridView1.Focus();
            }
            else if (e.Control && e.KeyCode == Keys.A)
            {
                PretragaCtrlA();
                dataGridView1.Focus();
            }
        }
    }
}