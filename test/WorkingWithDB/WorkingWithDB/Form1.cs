using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Data;
using System.Data.SqlClient;

using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace WorkingWithDB
{
    public partial class Form1 : Form
    {
        /// Открытое подключение к БД
        SqlConnection sqlConnection;

        public Form1()
        {
            InitializeComponent();
        }

        /// Происходить при загрузке формы
        private void Form1_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=\\FSERVER-366\Public\PAVLENKO ARTEM\DB_HORIZONT\Database.mdf;Integrated Security=True";

            sqlConnection = new SqlConnection(connectionString);

            //var openDB1 = Task.Factory.StartNew(() => { sqlConnection.Open(); });
            sqlConnection.Open();

            SqlDataReader sqlReader = null;

            SqlCommand commandSELECT = new SqlCommand("SELECT *  FROM [Products]", sqlConnection);

            try
            {
                //var openDB2 = Task.Factory.StartNew(() => { sqlReader = command.ExecuteReader(); });
                sqlReader = commandSELECT.ExecuteReader();
                while (sqlReader.Read())
                {
                    listBox1.Items.Add(Convert.ToString(sqlReader["id"]) + "       " + Convert.ToString(sqlReader["name"]) + "        " + Convert.ToString(sqlReader["price"]));
                }

            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message.ToString(), es.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }

        /// Отключиться от БД
        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
            {
                sqlConnection.Close();
            }
        }

        /// Отключиться от БД при закрытии формы
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlConnection != null && sqlConnection.State != ConnectionState.Closed)
            {
                sqlConnection.Close();
            }
        }

        /// Добавление в БД
        private void button1_Click(object sender, EventArgs e)
        {
            if (label7.Visible)
                label7.Visible = false;

            if (!string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text) && !string.IsNullOrWhiteSpace(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
            {
                SqlCommand commandINSERT = new SqlCommand("INSERT INTO [Products] (name, price) VALUES (@name, @price)", sqlConnection);
                commandINSERT.Parameters.AddWithValue("name", textBox1.Text);
                commandINSERT.Parameters.AddWithValue("price", textBox2.Text);

                commandINSERT.ExecuteNonQuery();
            }
            else
            {
                label7.Visible = true;
                label7.Text = "'name' и 'price' должны быть заполнены!!!";
            }
        }

        /// Обновление БД и вывод в listBox1
        private void обновитьБДToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            SqlDataReader sqlReader = null;

            SqlCommand commandSELECT = new SqlCommand("SELECT *  FROM [Products]", sqlConnection);

            try
            {
                //var openDB2 = Task.Factory.StartNew(() => { sqlReader = command.ExecuteReader(); });
                sqlReader = commandSELECT.ExecuteReader();
                while (sqlReader.Read())
                {
                    listBox1.Items.Add(Convert.ToString(sqlReader["id"]) + "       " + Convert.ToString(sqlReader["name"]) + "        " + Convert.ToString(sqlReader["price"]));
                }
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message.ToString(), es.Source.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (sqlReader != null)
                    sqlReader.Close();
            }
        }

        /// Обновление записи в БД
        private void button2_Click(object sender, EventArgs e)
        {
            if (label8.Visible)
                label8.Visible = false;

            if (!string.IsNullOrWhiteSpace(textBox3.Text) && !string.IsNullOrEmpty(textBox3.Text) &&
                !string.IsNullOrWhiteSpace(textBox4.Text) && !string.IsNullOrEmpty(textBox4.Text) &&
                !string.IsNullOrWhiteSpace(textBox5.Text) && !string.IsNullOrEmpty(textBox5.Text))
            {
                SqlCommand commandUPDATE = new SqlCommand("UPDATE [Products] SET [name]=@name, [price]=@price WHERE [id]=@id", sqlConnection);
                commandUPDATE.Parameters.AddWithValue("name", textBox4.Text);
                commandUPDATE.Parameters.AddWithValue("price", textBox3.Text);
                commandUPDATE.Parameters.AddWithValue("id", textBox5.Text);

                commandUPDATE.ExecuteNonQuery();
            }
            else
            {
                label8.Visible = true;
                label8.Text = "'id', 'name' и 'price' должны быть заполнены!!!";
            }
        }

        /// удаление записи в БД
        private void button3_Click(object sender, EventArgs e)
        {
            if (label9.Visible)
                label9.Visible = false;

            if (!string.IsNullOrWhiteSpace(textBox6.Text) && !string.IsNullOrEmpty(textBox6.Text))
            {
                SqlCommand commandDELETE = new SqlCommand("DELETE FROM [Products] WHERE [id]=@id", sqlConnection);
                commandDELETE.Parameters.AddWithValue("id", textBox6.Text);
            
                commandDELETE.ExecuteNonQuery();
            }
            else
            {
                label9.Visible = true;
                label9.Text = "'id' должен быть заполнен!!!";
            }
        }
    }
}