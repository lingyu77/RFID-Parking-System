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

    public partial class Main : Form
    {
        MySQLConnection DBConn;

        public Int32 port, port1, port2;
        public TcpClient client, client1, client2;
        public NetworkStream stream, stream1, stream2;

        int Ncar = 0, SNcar = 50;

        public Main()
        {
            InitializeComponent();
            DBConn = new MySQLConnection(new MySQLConnectionString("140.134.208.84", "parkingsystem", "root", "FCUIECS", 3306).AsString); //連資料庫 

            connect();
            request("alien" + (char)13);
            delaytime(200);
            request("password" + (char)13);
            tSB_Exit.Enabled = false;
            tSB_search.Enabled = false;
            TSMI_help.Enabled = false;
            tabControl1.Enabled = false;
        }

        #region core

        public void connect()
        {
            port = 20000;  //門口的reader
            client = new TcpClient("127.0.0.1", port);
            stream = client.GetStream();

            port1 = 20001;   //1F的reader
            client1 = new TcpClient("127.0.0.1", port1);
            stream1 = client1.GetStream();

            port2 = 20002;   //2F的reader
            client2 = new TcpClient("127.0.0.1", port2);
            stream2 = client2.GetStream();
        }

        public void disconnect()
        {
            stream.Close();
            client.Close();
            stream1.Close();
            client1.Close();
            stream2.Close();
            client2.Close();
        }

        public void request(String message)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            stream1.Write(data, 0, data.Length);
            stream2.Write(data, 0, data.Length);
        }

        public String response()
        {
            Byte[] data = new Byte[256];
            Int32 bytes = stream.Read(data, 0, data.Length);
            String responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            return responseData;
        }

        public String response1()
        {
            Byte[] data = new Byte[4096];
            Int32 bytes = stream1.Read(data, 0, data.Length);
            String responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            return responseData;
        }

        public String response2()
        {
            Byte[] data = new Byte[4096];
            Int32 bytes = stream2.Read(data, 0, data.Length);
            String responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            return responseData;
        }

        public void delaytime(int time)
        {
            System.Threading.Thread.Sleep(time);
        }

        public void Save_recoder()
        {
            DBConn.Open();
            MySQLCommand DBComm = new MySQLCommand("select * from `parkingsystem`.`record`", DBConn);
            MySQLCommand firstCmd = new MySQLCommand("set names big5", DBConn);
            firstCmd.ExecuteNonQuery();
            MySQLDataReader DBReader = DBComm.ExecuteReaderEx();

            DBReader.Read();
            bool s = true;
            do
            {
                string compareID = ("" + DBReader.GetValue(0));  //Tad_ID  
                string state = ("" + DBReader.GetValue(2));    //目前狀態                             

                if (compareID == textBox1.Text)
                {
                    if (state == "P")//要出去
                    {
                        string Etime = ("" + DBReader.GetValue(3));  //進入時間 

                        UPdata(compareID, Etime);
                        s = false;
                        break;
                    }
                    else if (state == "E")//正在進入
                    {
                        s = false;
                        break;
                    }
                    else if (state == "")//正在出去
                    {
                        s = false;
                        break;
                    }
                }
            } while (DBReader.Read());

            if (s) save();  //要進來
            DBConn.Close();
        }

        public void UPdata(string id, string EnterTime)
        {
            DBConn.Open();
            MySQLCommand firstCmd = new MySQLCommand("set names big5", DBConn);
            firstCmd.ExecuteNonQuery();
            string Out = tSSL_Now.Text = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");

            DateTime ft = DateTime.Parse(Out);
            DateTime ot = DateTime.Parse(EnterTime);
            string result = ((TimeSpan)(ft - ot)).TotalSeconds.ToString();  //算時間(單位:秒)

            int money, temp, totaltime;  //算錢

            totaltime = int.Parse(result);
            temp = totaltime % 3600;  //轉小時
            if (temp == 0) money = temp * 20; //剛好停整數小時

            else
            {
                totaltime = (totaltime / 3600) + 1;
                money = totaltime * 20;
            }
            
            MySQLCommand DBCo = new MySQLCommand("select * from `parkingsystem`.`member`", DBConn);
            MySQLDataReader DBReader1 = DBCo.ExecuteReaderEx();
            
            while (DBReader1.Read())
            {
                string s1 = DBReader1.GetString(1);

                if (id == s1)
                {
                    string s2 = DBReader1.GetString(10);
                    int temp1 = int.Parse(s2);
                    int ss = temp1 - money;

                    s2 = (""+ss);

                    MySQLCommand DBCom = new MySQLCommand("UPDATE `parkingsystem`.`member` SET `Money` = '" + s2 + "' WHERE CONVERT( `member`.`Tag_ID` USING utf8 ) = '" + id + "' LIMIT 1 ;", DBConn);
                    MySQLDataReader DBReader2 = DBCom.ExecuteReaderEx();
                    break;
                }
            }

            string hresult = ("" + totaltime);
            string smoney = ("" + money);

            MySQLCommand DBComm = new MySQLCommand("UPDATE `parkingsystem`.`record` SET  `Ttime` =  '" + hresult + "',`state` = '',`Otime` = '" + Out + "',`Smoney` = '" + smoney + "' WHERE CONVERT( `record`.`Etime` USING utf8 ) = '" + EnterTime + "' LIMIT 1 ;", DBConn);
            MySQLDataReader DBReader = DBComm.ExecuteReaderEx();
            DBConn.Close();

            if (Ncar != 0)
            {
                textBox4.Text = ("" + --Ncar);
                textBox5.Text = ("" + ++SNcar);
                tB_event.Text += (Out + "    有車輛出去!      " + textBox3.Text + "        " + smoney + "\r\n"); //message
            }
            timer3.Enabled = true;
        }

        public void save()
        {
            DBConn.Open();
            MySQLCommand firstCmd = new MySQLCommand("set names big5", DBConn);
            firstCmd.ExecuteNonQuery();

            string enter = System.DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
            MySQLCommand DBComm = new MySQLCommand("INSERT INTO `parkingsystem`.`record` (`Tag_ID`,`carID`,`state`,`Etime`)VALUES ('" + textBox1.Text + "','" + textBox3.Text + "','" + "E" + "','" + enter + "');", DBConn);
            MySQLDataReader DBReader = DBComm.ExecuteReaderEx();
            DBConn.Close();

            if (SNcar != 0)
            {
                textBox4.Text = ("" + ++Ncar);
                textBox5.Text = ("" + --SNcar);
                tB_event.Text += (enter + "    有車輛進入!      " + textBox3.Text + "\r\n"); //message               
            }
        }

        public void Save_state()
        {
            DBConn.Open();
            MySQLCommand DBComm = new MySQLCommand("select * from `parkingsystem`.`record`", DBConn);
            MySQLCommand firstCmd = new MySQLCommand("set names big5", DBConn);
            firstCmd.ExecuteNonQuery();
            MySQLDataReader DBReader = DBComm.ExecuteReaderEx();
            DBReader.Read();
            do
            {
                string time = ("" + DBReader.GetValue(3));  //key 
                string state = ("" + DBReader.GetValue(2));  //目前狀態                             

                if (state == "E")
                {
                    MySQLCommand DBCom = new MySQLCommand("UPDATE `parkingsystem`.`record` SET `state` = 'P' WHERE CONVERT( `record`.`Etime` USING utf8 ) = '" + time + "' LIMIT 1 ;", DBConn);
                    MySQLDataReader DBReader1 = DBCom.ExecuteReaderEx();
                }
            } while (DBReader.Read());
            DBConn.Close();
        }


        #endregion

        private void tSB_About_Click(object sender, EventArgs e)
        {
            About AB = new About();
            AB.StartPosition = FormStartPosition.CenterScreen;
            AB.Show();
        }

        private void tSB_Exit_Click(object sender, EventArgs e)
        {
            tSB_Exit.Enabled = false;
            tSB_search.Enabled = false;
            TSMI_help.Enabled = false;
            tabControl1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)  //顯示目前時間
        {
            tSSL_Now.Text = DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss");
        }

        private void timer2_Tick(object sender, EventArgs e)   //持續偵測
        {
            string Tag = "";
            DBConn.Open();
            MySQLCommand DBComm = new MySQLCommand("select * from `parkingsystem`.`member`", DBConn);
            MySQLCommand firstCmd = new MySQLCommand("set names big5", DBConn);
            firstCmd.ExecuteNonQuery();

            request("T" + (char)13);
            delaytime(200);

            String result = response();
            String result1 = response1();
            String result2 = response2();
            string sPattern = "[\\da-fA-F]{4} [\\da-fA-F]{4} [\\da-fA-F]{4} [\\da-fA-F]{4} [\\da-fA-F]{4} [\\da-fA-F]{4}";
            MatchCollection matchs = Regex.Matches(result, sPattern, RegexOptions.IgnoreCase);  //抓一大段其中的一段字串
            MatchCollection matchs1 = Regex.Matches(result1, sPattern, RegexOptions.IgnoreCase);
            MatchCollection matchs2 = Regex.Matches(result2, sPattern, RegexOptions.IgnoreCase);
            MySQLDataReader DBReader = DBComm.ExecuteReaderEx();

            foreach (Match m in matchs)
            {
                Tag = m.Value;
            }

            int F = 0;

            foreach (Match m in matchs1)  //1F
            {
                if (m.Value != "") F = F + 1;//掃到                        
                DBReader.Read();
                do
                {
                    Save_state();
                } while (DBReader.Read());
            }
            tB_1F.Text = ("" + F); //message 
            if (F >= 25) state_1F.Text = "車位已滿";
            F = 0;

            foreach (Match m in matchs2)  //2F
            {
                if (m.Value != "") F = F + 1;//掃到    
                DBReader.Read();
                do
                {
                    Save_state();
                } while (DBReader.Read());
            }
            tB_2F.Text = ("" + F);
            if (F >= 25) state_2F.Text = "車位已滿";

            tSSL_state.Text = "偵測中...";

            if (Tag != "")
            {       //掃到                     

                textBox1.Text = Tag;
                try
                {

                    while (DBReader.Read())
                    {
                        string tagstr = DBReader.GetString(1);

                        if (textBox1.Text == tagstr)
                        {
                            textBox2.Text = ("" + DBReader.GetValue(2)); //MID
                            textBox3.Text = ("" + DBReader.GetValue(3)); //carID                               
                            Save_recoder();
                            break;
                        }
                    }
                }
                   finally  //釋放資源
                {
                    DBReader.Close();
                }
                DBConn.Close();
            } //if 
        }

        private void timer3_Tick(object sender, EventArgs e) //存入歷史記錄
        {
            DBConn.Open();
            MySQLCommand DBComm = new MySQLCommand("select * from `parkingsystem`.`record`", DBConn);
            MySQLCommand firstCmd = new MySQLCommand("set names big5", DBConn);
            firstCmd.ExecuteNonQuery();
            MySQLDataReader DBReader = DBComm.ExecuteReaderEx();

            DBReader.Read();
            do
            {
                string time = ("" + DBReader.GetValue(3));  //key 
                string state = ("" + DBReader.GetValue(2));  //目前狀態                             

                if (state == "")
                {
                    MySQLCommand DBCom = new MySQLCommand("UPDATE `parkingsystem`.`record` SET `state` = 'S' WHERE CONVERT( `record`.`Etime` USING utf8 ) = '" + time + "' LIMIT 1 ;", DBConn);
                    MySQLDataReader DBReader1 = DBCom.ExecuteReaderEx();
                }
            } while (DBReader.Read());
            timer3.Enabled = false;
        }

        private void tSB_login_Click(object sender, EventArgs e)
        {

            login F = new login();
            F.StartPosition = FormStartPosition.CenterScreen;
            F.Show();
            timer4.Enabled = true;

        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            DBConn.Open();
            MySQLCommand DBComm = new MySQLCommand("select * from `parkingsystem`.`login_record`", DBConn);
            MySQLCommand firstCmd = new MySQLCommand("set names big5", DBConn);
            firstCmd.ExecuteNonQuery();
            MySQLDataReader DBReader = DBComm.ExecuteReaderEx();

            DBReader.Read();
            do
            {
                if (DBReader.GetString(2) == "帳密正確")
                {

                    tSB_Exit.Enabled = true;
                    tSB_search.Enabled = true;
                    TSMI_help.Enabled = true;
                    tabControl1.Enabled = true;
                    MySQLCommand DBCom = new MySQLCommand("UPDATE `parkingsystem`.`login_record` SET `result` = '成功登入' WHERE CONVERT( `login_record`.`date` USING utf8 ) = '" + DBReader.GetString(0) + "' LIMIT 1 ;", DBConn);
                    MySQLDataReader DBReader1 = DBCom.ExecuteReaderEx();
                    timer4.Enabled = false;
                    break;
                }
            } while (DBReader.Read());
            DBConn.Close();
        }

        private void tSB_search_Click(object sender, EventArgs e)
        {
            Form_Search S = new Form_Search();
            S.StartPosition = FormStartPosition.CenterScreen;
            S.Show();
        }

        private void TSMI_use_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("請參考書面附件! 電子檔尚在建置中....");
        }


        private void TStMI_About_Click(object sender, EventArgs e)
        {
            About AB = new About();
            AB.StartPosition = FormStartPosition.CenterScreen;
            AB.Show();

        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            DBConn.Open();
            MySQLCommand DBComm = new MySQLCommand("select * from `parkingsystem`.`record`", DBConn);
            MySQLCommand firstCmd = new MySQLCommand("set names big5", DBConn);
            firstCmd.ExecuteNonQuery();
            MySQLDataReader DBReader = DBComm.ExecuteReaderEx();
            bool s = true;

            while (DBReader.Read())
            {
                if (DBReader.GetString(2) == "E" || DBReader.GetString(2) == "")
                {
                    tSSL_C.Text = "開啟";
                    s = false;
                    break;
                }
            }
            if (s) tSSL_C.Text = "關閉";
            DBConn.Close();
        }

       

    }   
        
}
