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
           
            DBConn = new MySQLConnection(new MySQLConnectionString("140.134.208.84", "parkingsystem", "root", "FCUIECS", 3306).AsString); //連資料庫 
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

                case "車場硬體設備":
                    DBComm = new MySQLCommand("select * from `parkingsystem`.`reader`", DBConn);
                    DBReader = DBComm.ExecuteReaderEx();
                    Mshow.Text = ("");  
                    while (DBReader.Read())
                    {                              
                          Mshow.Text += (" 編號: " + DBReader.GetValue(0)); 
                          Mshow.Text += (" 位置: " + DBReader.GetValue(1)); 
                          Mshow.Text += (" 購買日期: " + DBReader.GetValue(2));        
                          Mshow.Text += (" 位置: " + DBReader.GetValue(3)); 
                          Mshow.Text += (" 購買價錢: " + DBReader.GetValue(4)+"\r\n");                                   
                     }                                             
                    break;

                case "車輛停放記錄":
                    DBComm = new MySQLCommand("select * from `parkingsystem`.`record`", DBConn);
                    DBReader = DBComm.ExecuteReaderEx();                  
                    Mshow.Text = ("");
                    while (DBReader.Read())
                    {
                        Mshow.Text += (" 車號: " + DBReader.GetValue(1));
                        Mshow.Text += (" 進入日期: " + DBReader.GetValue(3));
                        Mshow.Text += (" 出去日期: " + DBReader.GetValue(4));
                        Mshow.Text += (" 消費經額: " + DBReader.GetValue(7) + "\r\n");
                    } 
                    break;              

                case "管理人員資料": 
                    DBComm = new MySQLCommand("select * from `parkingsystem`.`manager`", DBConn);
                    DBReader = DBComm.ExecuteReaderEx();
                    Mshow.Text = (""); 
                    while (DBReader.Read())
                    {  
                        Mshow.Text += (" 帳號: " + DBReader.GetValue(0));
                        Mshow.Text += (" 姓名: " + DBReader.GetValue(1));        
                        Mshow.Text += (" 電話: " + DBReader.GetValue(4) + "\r\n");
                    }
                    break;
            }
            DBConn.Close();
        }
    }
}