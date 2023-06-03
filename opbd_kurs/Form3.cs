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
using System.Data.SqlClient;
using System.Xml.Linq;
using Microsoft.Data.SqlClient;
using BCrypt.Net;
namespace opbd_kurs
{
    public partial class Form3 : Form
    {
        DataBase db = new DataBase();

        Microsoft.Data.SqlClient.SqlConnection sqlConnection = new Microsoft.Data.SqlClient.SqlConnection("Data Source=(localdb)\\mssqllocaldb; Initial Catalog = opbd; Integrated Security=True");

        public Form3()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            textBoxPas.UseSystemPasswordChar = true;
            pictureBox1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataBase data = new DataBase();
           

            db.openConnection();

            string log = textBoxLog.Text;
            string pass = textBoxPas.Text;
            string password = Hash.hashPassword(pass);
            string role = "клиент";
            string name = textBoxN.Text;
            string surname = textBoxF.Text;
            string otch = textBoxO.Text;
            string email = textBoxE.Text;
            string phone = textBoxP.Text;

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

            if (com.ExecuteNonQuery()>0)
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

        private void label7_Click(object sender, EventArgs e)
        {

        }



        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBoxPas.UseSystemPasswordChar = false;

            pictureBox1.Visible = true;
            pictureBox2.Visible = false;

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            textBoxPas.UseSystemPasswordChar = true;

            pictureBox1.Visible = false;
            pictureBox2.Visible = true;
        }
       

    }
}

