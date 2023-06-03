using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace opbd_kurs
{
    public partial class Form1 : Form
    {   
        DataBase db= new DataBase();

        Microsoft.Data.SqlClient.SqlConnection sqlConnection = new Microsoft.Data.SqlClient.SqlConnection("Data Source=(localdb)\\mssqllocaldb; Initial Catalog = opbd; Integrated Security=True");

        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen; 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
            pictureBox1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
            {
                string login = textBox1.Text;
                string password = textBox2.Text;
                string pas = Hash.hashPassword(password);

                Microsoft.Data.SqlClient.SqlDataAdapter adapter = new Microsoft.Data.SqlClient.SqlDataAdapter();
                DataTable dt = new DataTable();

                db.openConnection();

                if (radioButton1.Checked)
                {
                    try
                    {
                        //Microsoft.Data.SqlClient.SqlCommand com = new Microsoft.Data.SqlClient.SqlCommand("SELECT COUNT(*) FROM пользователь where  'логин' = @login and 'пароль' = @pas and 'роль' = 'клиент' ", sqlConnection);

                        string query = "SELECT COUNT(*) FROM пользователь where логин = @login and пароль = @pas and роль = 'клиент' ";
                        Microsoft.Data.SqlClient.SqlCommand com = new Microsoft.Data.SqlClient.SqlCommand(query, sqlConnection);
                        com.CommandType = System.Data.CommandType.Text;
                        com.Parameters.AddWithValue("@login", login);
                        com.Parameters.AddWithValue("@pas", pas);

                        sqlConnection.Open();

                        if ((int)com.ExecuteScalar() > 0)
                        {
                            MessageBox.Show("Вы успешно вошли!");
                            DataStorage.idClient = ID();
                           
                            ClientForm form = new ClientForm();
                            this.Hide();
                            form.ShowDialog();


                            sqlConnection.Close(); 
                            textBox1.Clear();
                            textBox2.Clear();
                            radioButton1.Checked = false; 
                            db.closeConnection();
                        }
                        else
                        {
                            MessageBox.Show("Произошла ошибка! Попробуйте еще раз.");
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка: " + ex.Message);
                    }
                }
                else if (radioButton2.Checked)
                {
                    try
                    {
                        string query = "SELECT COUNT(*) FROM пользователь where логин = @login and пароль = @pas and роль = 'администратор' ";
                        Microsoft.Data.SqlClient.SqlCommand com = new Microsoft.Data.SqlClient.SqlCommand(query, sqlConnection);
                        com.CommandType = System.Data.CommandType.Text;
                        com.Parameters.AddWithValue("@login", login);
                        com.Parameters.AddWithValue("@pas", pas);

                        sqlConnection.Open();

                        if ((int)com.ExecuteScalar() == 1)
                        {
                            MessageBox.Show("Успешно!");
                            DataStorage.idClient = ID();
                            Form2 form2 = new Form2();
                            this.Hide();
                            form2.ShowDialog();

                           

                            sqlConnection.Close();
                            textBox1.Clear();
                            textBox2.Clear();
                            radioButton2.Checked = false;
                            db.closeConnection();
                        }
                        else
                        {
                            MessageBox.Show("Произошла ошибка! Попробуйте еще раз.");
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста введите логин пользователя и пароль!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form3 form3 = new Form3();
            this.Hide();
            form3.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
            textBox2.UseSystemPasswordChar = true;
         
            pictureBox1.Visible = false;
            pictureBox2.Visible = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = false;

            pictureBox1.Visible = true;
            pictureBox2.Visible = false;

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        public int ID()
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            string pas = Hash.hashPassword(password);
            int  id_cliet = 0;  

            string query = "SELECT id FROM пользователь where логин = @login and пароль = @pas";
            Microsoft.Data.SqlClient.SqlCommand com = new Microsoft.Data.SqlClient.SqlCommand(query, sqlConnection);
            com.CommandType = System.Data.CommandType.Text;
            com.Parameters.AddWithValue("@login", login);
            com.Parameters.AddWithValue("@pas", pas);
          
           // sqlConnection.Open();

            id_cliet = (int)com.ExecuteScalar();   
            //MessageBox.Show("id from int fuction "  + id_cliet);
            //DataStorage.idClient = id_cliet; 
            sqlConnection.Close();
            return DataStorage.idClient;
        }
    }
}
