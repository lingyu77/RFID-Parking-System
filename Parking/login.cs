using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using MySQLDriverCS;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Parking
{
    public partial class login : Form
    {
        MySQLConnection DBConn;
        
        public login()
        {
            InitializeComponent();
            tB_password.PasswordChar = '*';
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.ControlBox = false;
            DBConn = new MySQLConnection(new MySQLConnectionString("140.134.208.84", "parkingsystem", "root", "FCUIECS", 3306).AsString); //�s��Ʈw 
 
        }
   
        private void sure_Click(object sender, EventArgs e)
        {
            DBConn.Open();
            MySQLCommand DBComm = new MySQLCommand("select * from `parkingsystem`.`manager`", DBConn);
            MySQLCommand firstCmd = new MySQLCommand("set names big5", DBConn);
            firstCmd.ExecuteNonQuery();
            MySQLDataReader DBReader = DBComm.ExecuteReaderEx();
            bool error = true;

            DBReader.Read();
            do
            {
                string id = ("" + DBReader.GetValue(0));  //id 
                string passoord = ("" + DBReader.GetValue(2));  //password                             

                string enter = System.DateTime.Now.ToString("yyyy�~MM��dd�� HH:mm:ss");
                if (tB_id.Text == id && tB_password.Text == passoord)
                {
                    error = false;
                    MySQLCommand DBCom = new MySQLCommand("INSERT INTO `parkingsystem`.`login_record` (`date`,`id`,`result`)VALUES ('" + enter + "','" + tB_id.Text + "','" + "�b�K���T" + "');", DBConn);
                    MySQLDataReader DBReader1 = DBCom.ExecuteReaderEx();
                    this.Close();

                }
            } while (DBReader.Read());
            if (error) MessageBox.Show("�n���ɧ@�~����!! =" + "\r\n" + "�b���αK�X����!�ЦA�T�{");
        }

        private void canel_Click(object sender, EventArgs e)
        {         
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DBConn.Open();
            MySQLCommand DBComm = new MySQLCommand("select * from `parkingsystem`.`manager`", DBConn);
            MySQLCommand firstCmd = new MySQLCommand("set names big5", DBConn);
            firstCmd.ExecuteNonQuery();
            MySQLDataReader DBReader = DBComm.ExecuteReaderEx();

            while (DBReader.Read())
            {
                string tagstr = DBReader.GetString(0);
                if (tB_id.Text == tagstr) pictureBox1.ImageLocation = DBReader.GetString(5); //�Ϥ�
                                   
            }
        }
    }
}