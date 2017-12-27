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
    public partial class Form_Search : Form
    {
        MySQLConnection DBConn;
        public Form_Search()
        {
            InitializeComponent();
           
            DBConn = new MySQLConnection(new MySQLConnectionString("140.134.208.84", "parkingsystem", "root", "FCUIECS", 3306).AsString); //�s��Ʈw 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string choice ;
            choice = comboBox1.Text;
            DBConn.Open();
            MySQLCommand firstCmd = new MySQLCommand("set names big5", DBConn);
            firstCmd.ExecuteNonQuery();

            MySQLCommand DBComm;
            MySQLDataReader DBReader;

            switch (choice)
            {

                case "�����w��]��":
                    DBComm = new MySQLCommand("select * from `parkingsystem`.`reader`", DBConn);
                    DBReader = DBComm.ExecuteReaderEx();
                    Mshow.Text = ("");  
                    while (DBReader.Read())
                    {                              
                          Mshow.Text += (" �s��: " + DBReader.GetValue(0)); 
                          Mshow.Text += (" ��m: " + DBReader.GetValue(1)); 
                          Mshow.Text += (" �ʶR���: " + DBReader.GetValue(2));        
                          Mshow.Text += (" ��m: " + DBReader.GetValue(3)); 
                          Mshow.Text += (" �ʶR����: " + DBReader.GetValue(4)+"\r\n");                                   
                     }                                             
                    break;

                case "��������O��":
                    DBComm = new MySQLCommand("select * from `parkingsystem`.`record`", DBConn);
                    DBReader = DBComm.ExecuteReaderEx();                  
                    Mshow.Text = ("");
                    while (DBReader.Read())
                    {
                        Mshow.Text += (" ����: " + DBReader.GetValue(1));
                        Mshow.Text += (" �i�J���: " + DBReader.GetValue(3));
                        Mshow.Text += (" �X�h���: " + DBReader.GetValue(4));
                        Mshow.Text += (" ���O�g�B: " + DBReader.GetValue(7) + "\r\n");
                    } 
                    break;              

                case "�޲z�H�����": 
                    DBComm = new MySQLCommand("select * from `parkingsystem`.`manager`", DBConn);
                    DBReader = DBComm.ExecuteReaderEx();
                    Mshow.Text = (""); 
                    while (DBReader.Read())
                    {  
                        Mshow.Text += (" �b��: " + DBReader.GetValue(0));
                        Mshow.Text += (" �m�W: " + DBReader.GetValue(1));        
                        Mshow.Text += (" �q��: " + DBReader.GetValue(4) + "\r\n");
                    }
                    break;
            }
            DBConn.Close();
        }
    }
}