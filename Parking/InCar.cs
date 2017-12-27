using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Parking
{
    public partial class InCar : Form
    {
        public InCar(int message)
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
            if (message == 1) show();
        }
       
     #region core

     public void show()
     {
        
     }
     #endregion

       

    }
}