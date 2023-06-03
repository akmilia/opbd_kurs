using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows;

namespace opbd_kurs
{
   
    enum RowsStates
    {
        Existed, 
        New, 
        Modified, 
        ModifiedNew,
        Deleted
    }
    public partial class Form2 : Form
    {   
        DataBase db = new DataBase();
        Microsoft.Data.SqlClient.SqlConnection sqlConnection = new Microsoft.Data.SqlClient.SqlConnection("Data Source=(localdb)\\mssqllocaldb; Initial Catalog = opbd; Integrated Security=True");

        int SelectedRow;
        public Form2()
        {
            InitializeComponent();
        } 

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id", "ID");  
            dataGridView1.Columns.Add("id_тренера", "Фамилия");
            dataGridView1.Columns.Add("id_тренера", "Имя");
            dataGridView1.Columns.Add("id_направления", "Направление");
            dataGridView1.Columns.Add("место_проведения", "Место");
            dataGridView1.Columns.Add("дата_время", "Время");
            dataGridView1.Columns.Add("IsNew", String.Empty);

        }
       
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            //dgw.Rows.Add( record.GetString(1), record.GetString(2), record.GetString(3), record.GetDateTime(4), RowsStates.ModifiedNew );
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetString(4), record.GetDateTime(5), RowsStates.ModifiedNew);

        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            string query = "SELECT r.id as 'ID', имя,  фамилия AS 'Тренер', n.описание AS 'Направление', r.место_проведения, r.дата_время  " +
                "FROM расписание r " +
                "INNER JOIN тренеры t ON t.id = r.id_тренера " +
                " INNER JOIN тренер_направление tn ON tn.id_napr = r.id_направления " +
                
                "INNER JOIN направление n ON n.id = tn.id_napr ";
            sqlConnection.Open();
           // string query = $"select*from zapis_registration where applicant_id='{DataStorage.idClient.ToString()}'";
            SqlCommand command = new SqlCommand(query, db.getConnection());
            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close(); 
            sqlConnection.Close();
            db.closeConnection();

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedRow = e.RowIndex;

            if(e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[SelectedRow];

                textBox2.Text = row.Cells[0].Value.ToString();
                textBox3.Text = row.Cells[1].Value.ToString();
                textBox4.Text = row.Cells[2].Value.ToString();
                textBox1.Text = row.Cells[3].Value.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Info info = new Info();
            info.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            ////добавить   
            Form2_Add form = new Form2_Add(); 
            form.ShowDialog();

        }


        private void button4_Click(object sender, EventArgs e)
        {
            Update(); 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //удалить 
            deleteRow();
            newUpdate();
            RefreshDataGrid(dataGridView1);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            deleteRow();
        }

        private void newUpdate()
        {
            db.openConnection();
            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowsStates)dataGridView1.Rows[index].Cells[4].Value;
                if (rowState == RowsStates.Existed)
                    continue;
                if (rowState == RowsStates.Deleted)
                {
                    var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
                    var deleteQuery = $"delete from расписание where id={id}";
                    var command = new SqlCommand(deleteQuery, db.getConnection());
                    command.ExecuteNonQuery();
                }

            } 

            db.closeConnection();
        }


        private void deleteRow()
        {

            int index = dataGridView1.CurrentCell.RowIndex;
            dataGridView1.Rows[index].Visible = false;

            if (dataGridView1.Rows[index].Cells[0].Value.ToString() == string.Empty)
            {
                dataGridView1.Rows[index].Cells[4].Value = RowsStates.Deleted;
                return;
            }

        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {   
            
            RefreshDataGrid(dataGridView1);
            newUpdate();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
