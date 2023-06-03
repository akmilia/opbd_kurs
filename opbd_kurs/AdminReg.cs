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

namespace opbd_kurs
{
    public partial class AdminReg : Form
    {
        DataBase db = new DataBase();

        Microsoft.Data.SqlClient.SqlConnection sqlConnection = new Microsoft.Data.SqlClient.SqlConnection("Data Source=(localdb)\\mssqllocaldb; Initial Catalog = opbd; Integrated Security=True");

        public AdminReg()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

        }

        private void AdminReg_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string role = "";
            DataBase data = new DataBase();

            if (radioButton1.Checked) { role = "администратор"; } 
            else if ( radioButton2.Checked ) { role = "тренер";  }

            db.openConnection();

            string log = textBox1.Text;
            string pass = textBox2.Text;
            string password = Hash.hashPassword(pass);
           
            string name = textBox3.Text;
            string surname = textBox4.Text;
            string otch = textBox5.Text;
            string email = textBox6.Text;
            string phone = " ";

            Microsoft.Data.SqlClient.SqlCommand com = new Microsoft.Data.SqlClient.SqlCommand("добавл", sqlConnection);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@userName", log);
            com.Parameters.AddWithValue("@userPassword", password);
            com.Parameters.AddWithValue("@userRole", role);
            com.Parameters.AddWithValue("@Name", name);
            com.Parameters.AddWithValue("@Surname", surname);
            com.Parameters.AddWithValue("@Otch", otch);
            com.Parameters.AddWithValue("@email", email);
            com.Parameters.AddWithValue("@phone", phone);
            sqlConnection.Open();
            //com.ExecuteNonQuery();

            if (com.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("Вы зарегестрировались!!");
                ClientForm form = new ClientForm();
                this.Hide();
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Произошла ошибка! Попробуйте еще раз.");
            }
        }
    }
}
