using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WindowsFormsApp6 //git change
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        OleDbDataAdapter adapter;
        DataSet dataset;
        string[,] MAIN;
        int p;
       
        private void Form1_Load(object sender, EventArgs e)
        {

            data.Columns.Add("Code", "Код");
            data.Columns.Add("Surname", "Фамилия");
            data.Columns.Add("Name", "Имя");
            data.Columns.Add("Secondname", "Отчество");
            data.Columns.Add("Street", "Код улицы");
            data.Columns.Add("House", "Номер дома");
            data.Columns.Add("Fraction", "Дробная часть");
            data.Columns.Add("Phone", "Телефон");

            string conn_param = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + @"БД.mdb";
            OleDbConnection connection = new OleDbConnection(conn_param);
            OleDbCommand command = connection.CreateCommand();
            //посчитать количество записей и потом только создать массив стринг
            command.CommandText = "select count(*) from Владельцы";
            connection.Open();
            int v =Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
            //
            command.CommandText = "select * from Владельцы";
            connection.Open();
            OleDbDataReader reader = command.ExecuteReader();
            int i = 0;
            string[,] A = new string[8,v];
            try
            {
                while (reader.Read())
                {
                    data.Rows.Add();
                    data[0, i].Value = reader["Код"];
                    A[0, i] = Convert.ToString(reader["Код"]);
                    data[1, i].Value = reader["Фамилия"];
                    A[1, i] = Convert.ToString(reader["Фамилия"]);
                    data[2, i].Value = reader["Имя"];
                    A[2, i] = Convert.ToString(reader["Имя"]);
                    data[3, i].Value = reader["Отчество"];
                    A[3, i] = Convert.ToString(reader["Отчество"]);
                    data[4, i].Value = reader["Код_улицы"];
                    A[4, i] = Convert.ToString(reader["Код_улицы"]);
                    data[5, i].Value = reader["Номер_дома"];
                    A[5, i] = Convert.ToString(reader["Номер_дома"]);
                    data[6, i].Value = reader["Дробная_часть_номера"];
                    A[6, i] = Convert.ToString(reader["Дробная_часть_номера"]);
                    data[7, i].Value = reader["Телефон"];
                    A[7, i] = Convert.ToString(reader["Телефон"]);
                    ++i;                
                }
            }
            finally
            {                            
                reader.Close();
                connection.Close();
            }
            MAIN = A;
            p = v;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = data.RowCount - 1;
            int k = 0, l = 0, m = 0;
            string[,] B = new string[8, i];
            for (k = 0; k < 8; k++)
                for (l = 0; l < i; l++)
                {
                    B[k, l] = Convert.ToString(data[k, l].Value);
                }
            string conn_param = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + @"БД.mdb";
            OleDbConnection connection = new OleDbConnection(conn_param);
            OleDbCommand command = connection.CreateCommand();
            command.Parameters.Add("@kod", OleDbType.Integer);
            command.Parameters.Add("@familia", OleDbType.VarChar);
            command.Parameters.Add("@imya", OleDbType.VarChar);
            command.Parameters.Add("@otchestvo", OleDbType.VarChar);
            command.Parameters.Add("@kodUlici", OleDbType.Integer);
            command.Parameters.Add("@nomerDoma", OleDbType.Integer);
            command.Parameters.Add("@drobnayaChast", OleDbType.VarChar);
            command.Parameters.Add("@telefon", OleDbType.VarChar);
            //сравниваем все элементы по строкам

            //если добавлено
            if (i > p)
            {
                for (m = p ; m < i; m++)
                {
                    command.CommandText = "insert into Владельцы (Код,Фамилия, Имя, Отчество, Код_улицы, Номер_дома, Дробная_часть_номера, Телефон) values (@kod,@familia,@imya,@otchestvo,@kodUlici,@nomerDoma,@drobnayaChast,@telefon)";
                    command.Parameters["@kod"].Value = m + 1;
                    command.Parameters["@familia"].Value = B[1, m];
                    command.Parameters["@imya"].Value = B[2, m];
                    command.Parameters["@otchestvo"].Value = B[3, m];
                    command.Parameters["@kodUlici"].Value = B[4, m];
                    command.Parameters["@nomerDoma"].Value = B[5, m];
                    command.Parameters["@drobnayaChast"].Value = B[6, m];
                    command.Parameters["@telefon"].Value = B[7, m];
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            else
            {
                for (m = 0; m < i; m++)
                {
                    //если запись удалена
                    if (B[0, m] == "" && B[1, m] == "" && B[1, m] == "" && B[3, m] == "" && B[4, m] == "" && B[5, m] == "" && B[6, m] == "" && B[7, m] == "")
                    {
                        command.CommandText = "delete from Владельцы where Код=@kod";
                        command.Parameters["@kod"].Value = m + 1;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    //если изменено
                    else if (B[1, m] != MAIN[1, m] || B[2, m] != MAIN[2, m] || B[3, m] != MAIN[3, m] || B[4, m] != MAIN[4, m] || B[5, m] != MAIN[5, m] || B[6, m] != MAIN[6, m] || B[7, m] != MAIN[7, m])
                    {
                        command.CommandText = "update Владельцы set Фамилия=\'" + B[1, m] + "\', Имя=\'" + B[2, m] + "\', Отчество=\'" + B[3, m] + "\', Код_улицы=\'" + B[4, m] + "\', Номер_дома=\'" + B[5, m] + "\', Дробная_часть_номера=\'" + B[6, m] + "\', Телефон=\'" + B[7, m] + "\' where Код=@kod";                        
                        command.Parameters["@kod"].Value = m + 1;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
        }
    }
}
