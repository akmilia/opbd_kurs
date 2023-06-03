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
    public partial class Info : Form
    {

        DataBase db = new DataBase();
        Microsoft.Data.SqlClient.SqlConnection sqlConnection = new Microsoft.Data.SqlClient.SqlConnection("Data Source=(localdb)\\mssqllocaldb; Initial Catalog = opbd; Integrated Security=True");

        int SelectedRow;
        public Info()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void CreateColumns()
        {
            dataGridView1.Columns.Add("Фамилия", "Фамилия");
            dataGridView1.Columns.Add("Имя", "Имя");
            dataGridView1.Columns.Add("Направление", "Направление");
            dataGridView1.Columns.Add("дата_время", "Время");
            dataGridView1.Columns.Add("место_проведения", "Место");
            

            dataGridView1.Columns.Add("IsNew", String.Empty);

        }

        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
           
            dgw.Rows.Add(record.GetString(0), record.GetString(1), record.GetString(2), record.GetDateTime(3), record.GetString(4), RowsStates.ModifiedNew);

        }
 

        private void Login(IDataRecord reader)
        {

            label2.Text = reader.GetString(1).ToString();
            label3.Text = reader.GetString(2).ToString();
            label4.Text = reader.GetString(3).ToString();
            label5.Text = reader.GetString(4).ToString();
            label6.Text = reader.GetString(5).ToString();
            label7.Text = reader.GetString(6).ToString();
        }

        private void Info_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void RefreshDataGrid(DataGridView dgw)
        {
            int idd = DataStorage.idClient;
            string query = "SELECT t.фамилия, t.имя AS 'Тренер', n.описание AS 'Направление', r.дата_время, r.место_проведения " +
                           "FROM занятие z " +
                           "INNER JOIN расписание r ON r.id = z.id_расписания " +
                           "INNER JOIN тренеры t ON t.id = r.id_тренера " +
                           "INNER JOIN направление n ON n.id = r.id_направления " +
                           "WHERE z.id_клиента = @idd"; 
            sqlConnection.Open(); 
            SqlCommand command = new SqlCommand(query, db.getConnection());
            command.Parameters.AddWithValue("@idd", idd);
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

    }

}