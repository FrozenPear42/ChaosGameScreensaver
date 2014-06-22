using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;

namespace ChaosGameScreensaver
{
    public partial class ConfigurationForm : Form
    {
        public ConfigurationForm()
        {
            InitializeComponent();
            updateList(RegistryStorage.GetData());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            saveData();
            this.Close();
        }

        private void ConfigurationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveData();
        }


        
        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

            String data = e.FormattedValue.ToString();

            if (String.IsNullOrEmpty(data))
                return;

            if (e.ColumnIndex == 0)
            {
                int value;
                if (Int32.TryParse(data, out value))
                    if (value > 0 && value <= 1000)
                    {
                        dataGridView1.Rows[e.RowIndex].ErrorText = "";
                        e.Cancel = false;
                        return;
                    }
                dataGridView1.Rows[e.RowIndex].ErrorText = "Number of verticles must be greatet than 0 and lower than 1000";
                e.Cancel = true;
            }
            else if (e.ColumnIndex == 1)
            {
                double value;
                if (Double.TryParse(data, out value))
                    if (value > 0.0 && value <= 1.0)
                    {
                        dataGridView1.Rows[e.RowIndex].ErrorText = "";
                        e.Cancel = false;
                        return;
                    }
                dataGridView1.Rows[e.RowIndex].ErrorText = "Factors must be greater than 0 and lower than 1.0";
                e.Cancel = true;
            }
            else
                e.Cancel = true;
        }


        private void saveData()
        {
            List<RenderData> obj = new List<RenderData>();
            
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                int n;
                double f;
                
                if(String.IsNullOrEmpty(r.ErrorText))
                    continue;

                if(!Int32.TryParse((string)r.Cells[0].Value, out n))
                    continue;
                if(!Double.TryParse((string)r.Cells[1].Value, out f))
                    continue;

                obj.Add(new RenderData(n, f));
            }
            RegistryStorage.SaveData(obj);
            
        }


        private void updateList(List<RenderData> list)
        {
            foreach(RenderData r in list)
            {
                dataGridView1.Rows.Add(r.N.ToString(), r.F.ToString());
            }
        }



    }
}
