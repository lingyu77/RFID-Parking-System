namespace Parking
{
    partial class login
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該公開 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(login));
            this.tB_id = new System.Windows.Forms.TextBox();
            this.tB_password = new System.Windows.Forms.TextBox();
            this.id = new System.Windows.Forms.Label();
            this.pw = new System.Windows.Forms.Label();
            this.sure = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.canel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tB_id
            // 
            this.tB_id.Location = new System.Drawing.Point(271, 81);
            this.tB_id.Name = "tB_id";
            this.tB_id.Size = new System.Drawing.Size(154, 22);
            this.tB_id.TabIndex = 0;
            // 
            // tB_password
            // 
            this.tB_password.Location = new System.Drawing.Point(271, 142);
            this.tB_password.Name = "tB_password";
            this.tB_password.Size = new System.Drawing.Size(154, 22);
            this.tB_password.TabIndex = 1;
            // 
            // id
            // 
            this.id.AutoSize = true;
            this.id.Location = new System.Drawing.Point(142, 102);
            this.id.Name = "id";
            this.id.Size = new System.Drawing.Size(0, 12);
            this.id.TabIndex = 2;
            // 
            // pw
            // 
            this.pw.AutoSize = true;
            this.pw.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.pw.ForeColor = System.Drawing.Color.Black;
            this.pw.Location = new System.Drawing.Point(213, 145);
            this.pw.Name = "pw";
            this.pw.Size = new System.Drawing.Size(35, 13);
            this.pw.TabIndex = 3;
            this.pw.Text = "密碼";
            // 
            // sure
            // 
            this.sure.Location = new System.Drawing.Point(271, 206);
            this.sure.Name = "sure";
            this.sure.Size = new System.Drawing.Size(75, 23);
            this.sure.TabIndex = 4;
            this.sure.Text = "確定";
            this.sure.UseVisualStyleBackColor = true;
            this.sure.Click += new System.EventHandler(this.sure_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(213, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "帳號";
            // 
            // canel
            // 
            this.canel.Location = new System.Drawing.Point(362, 206);
            this.canel.Name = "canel";
            this.canel.Size = new System.Drawing.Size(75, 23);
            this.canel.TabIndex = 6;
            this.canel.Text = "取消";
            this.canel.UseVisualStyleBackColor = true;
            this.canel.Click += new System.EventHandler(this.canel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(280, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(135, 24);
            this.label2.TabIndex = 7;
            this.label2.Text = "使用者登入";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(48, 52);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(143, 165);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 256);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.canel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sure);
            this.Controls.Add(this.pw);
            this.Controls.Add(this.id);
            this.Controls.Add(this.tB_password);
            this.Controls.Add(this.tB_id);
            this.Name = "login";
            this.Text = "login";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tB_id;
        private System.Windows.Forms.TextBox tB_password;
        private System.Windows.Forms.Label id;
        private System.Windows.Forms.Label pw;
        private System.Windows.Forms.Button sure;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button canel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
    }
}