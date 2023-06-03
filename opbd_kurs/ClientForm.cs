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
namespace opbd_kurs
{

    public partial class ClientForm : Form
    {
   
        DataBase db = new DataBase();
        SqlConnection sqlConnection = new SqlConnection("Data Source=(localdb)\\mssqllocaldb; Initial Catalog = opbd; Integrated Security=True");

        int SelectedRow;
        public ClientForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Info info = new Info();
            info.ShowDialog();
        }

        private void label2_Click(object sender, EventArgs e)
        {

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
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetString(4), record.GetDateTime(5), RowsStates.ModifiedNew);

        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            string query = "SELECT r.id as 'ID', имя, фамилия AS 'Тренер', n.описание AS 'Направление', r.место_проведения, r.дата_время  " +
                "FROM расписание r " +
                "INNER JOIN тренеры t ON t.id = r.id_тренера " +
                " INNER JOIN тренер_направление tn ON tn.id_napr = r.id_направления " +

                "INNER JOIN направление n ON n.id = tn.id_napr ";

            SqlCommand command = new SqlCommand(query, db.getConnection());
            db.openConnection();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[SelectedRow];

                textBox2.Text = row.Cells[0].Value.ToString();
                textBox3.Text = row.Cells[1].Value.ToString();
                textBox4.Text = row.Cells[2].Value.ToString();
                textBox1.Text = row.Cells[3].Value.ToString();
            }
        }

        private void ClientForm_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            try
            {
             
                int id_клиента = DataStorage.idClient;

                int id_расписания = (int)dataGridView1.CurrentRow.Cells[0].Value;
 
                //MessageBox.Show("id расписания " + id_расписания);
                //MessageBox.Show("id клиента " + id_клиента);
                string query = $"INSERT INTO занятие (id_клиента, id_расписания) VALUES (@id_клиента, @id_расписания)"; 

                SqlCommand com = new SqlCommand(query, db.getConnection());
                //MessageBox.Show("сформировал запрос");
                //com.CommandType = CommandType.Text;
                com.Parameters.AddWithValue("@id_клиента", id_клиента);
                com.Parameters.AddWithValue("@id_расписания", id_расписания);
         
                sqlConnection.Open();
                //MessageBox.Show("открыл sql");

                //com.ExecuteNonQuery(); 
                if (com.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Вы записались на занятие! \n Дополнительную информацию вы можете просмотреть на своей странице.");

                }
                else
                {
                    MessageBox.Show("Произошла ошибка! Попробуйте еще раз.");
                }
            }
            catch 
            {
                MessageBox.Show("Произошла ошибка!");
            }

            
        }





    }
}
