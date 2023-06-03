using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Data.SqlClient;
namespace opbd_kurs
{
    public partial class Form2_Add : Form
    {
        DataBase db = new DataBase();

       SqlConnection sqlConnection = new SqlConnection("Data Source=(localdb)\\mssqllocaldb; Initial Catalog = opbd; Integrated Security=True");

        public Form2_Add()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }


        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Add_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataBase data = new DataBase();


            db.openConnection();

            int id_trener = Convert.ToInt32(textBox1.Text);
            int id_napr = Convert.ToInt32(textBox2.Text);
            var date = DateTime.Parse(textBox3.Text);
            string place = textBox4.Text; 

            try
            {
                string query = "insert into расписание (id_тренера, id_направления, место_проведения, дата_время) values " +
                          "(@id_trener, @id_napr, @place, @date )";

                SqlCommand com = new SqlCommand(query, sqlConnection);
                com.CommandType = CommandType.Text;
                com.Parameters.AddWithValue("@id_trener", id_trener);
                com.Parameters.AddWithValue("@id_napr", id_napr);
                com.Parameters.AddWithValue("@place", place);
                com.Parameters.AddWithValue("@date", date);

                sqlConnection.Open();

                if (com.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show(" Запись успешно добавлена !");
                    db.closeConnection(); 
                    sqlConnection.Close();

                }
                else
                {
                    MessageBox.Show("Произошла ошибка! Попробуйте еще раз.");
                    db.closeConnection();
                    sqlConnection.Close();

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ошибка. Проверьте корректность ввода даты ");
            }
           

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CreateColumns()
        {
            dataGridView1.Columns.Add("id_тренера", "id_тренера");
            dataGridView1.Columns.Add("Фамилия", "Фамилия");
            dataGridView1.Columns.Add("Имя", "Имя");
            dataGridView1.Columns.Add("id_направления", "id_направления");
            dataGridView1.Columns.Add("Направление", "Направление");
            dataGridView1.Columns.Add("IsNew", String.Empty); 
        }
       
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetInt32(3), record.GetString(4), RowsStates.ModifiedNew);

        }

        private void RefreshDataGrid(DataGridView dgw)
        {
           SqlCommand cmd = new SqlCommand 
               (" SELECT t.id, t.фамилия , t.имя AS ФИО_тренера, " +
                " n.id, n.описание FROM тренеры t" +
                "  JOIN тренер_направление tn ON t.id = tn.id_trener " +
                " JOIN направление n ON tn.id_napr = n.id;", sqlConnection);

            sqlConnection.Open();
            db.openConnection();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();

            db.closeConnection();
            sqlConnection.Close();
        }
    }
}
