namespace PM_THI_TN
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.hệThốngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnUserManager = new System.Windows.Forms.ToolStripMenuItem();
            this.thoátToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.danhMụcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnBoardManager = new System.Windows.Forms.ToolStripMenuItem();
            this.btnTaskManager = new System.Windows.Forms.ToolStripMenuItem();
            this.btnListManager = new System.Windows.Forms.ToolStripMenuItem();
            this.thốngKêBáoCáoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hệThốngToolStripMenuItem
            // 
            this.hệThốngToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnUserManager,
            this.thoátToolStripMenuItem});
            this.hệThốngToolStripMenuItem.Name = "hệThốngToolStripMenuItem";
            this.hệThốngToolStripMenuItem.Size = new System.Drawing.Size(108, 32);
            this.hệThốngToolStripMenuItem.Text = "Hệ thống";
            // 
            // btnUserManager
            // 
            this.btnUserManager.Image = ((System.Drawing.Image)(resources.GetObject("btnUserManager.Image")));
            this.btnUserManager.Name = "btnUserManager";
            this.btnUserManager.Size = new System.Drawing.Size(251, 32);
            this.btnUserManager.Text = "Quản lý tài khoản";
            this.btnUserManager.Click += new System.EventHandler(this.quảnLýTàiKhoảnToolStripMenuItem_Click);
            // 
            // thoátToolStripMenuItem
            // 
            this.thoátToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("thoátToolStripMenuItem.Image")));
            this.thoátToolStripMenuItem.Name = "thoátToolStripMenuItem";
            this.thoátToolStripMenuItem.Size = new System.Drawing.Size(251, 32);
            this.thoátToolStripMenuItem.Text = "Thoát";
            this.thoátToolStripMenuItem.Click += new System.EventHandler(this.thoátToolStripMenuItem_Click);
            // 
            // danhMụcToolStripMenuItem
            // 
            this.danhMụcToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBoardManager,
            this.btnTaskManager,
            this.btnListManager});
            this.danhMụcToolStripMenuItem.Name = "danhMụcToolStripMenuItem";
            this.danhMụcToolStripMenuItem.Size = new System.Drawing.Size(55, 32);
            this.danhMụcToolStripMenuItem.Text = "List";
            // 
            // btnBoardManager
            // 
            this.btnBoardManager.Image = ((System.Drawing.Image)(resources.GetObject("btnBoardManager.Image")));
            this.btnBoardManager.Name = "btnBoardManager";
            this.btnBoardManager.Size = new System.Drawing.Size(224, 32);
            this.btnBoardManager.Text = "Quản lý Board";
            this.btnBoardManager.Click += new System.EventHandler(this.btnBoardManager_Click);
            // 
            // btnTaskManager
            // 
            this.btnTaskManager.Image = ((System.Drawing.Image)(resources.GetObject("btnTaskManager.Image")));
            this.btnTaskManager.Name = "btnTaskManager";
            this.btnTaskManager.Size = new System.Drawing.Size(224, 32);
            this.btnTaskManager.Text = "Quản lý Task";
            this.btnTaskManager.Click += new System.EventHandler(this.btnTaskManager_Click);
            // 
            // btnListManager
            // 
            this.btnListManager.Image = ((System.Drawing.Image)(resources.GetObject("btnListManager.Image")));
            this.btnListManager.Name = "btnListManager";
            this.btnListManager.Size = new System.Drawing.Size(224, 32);
            this.btnListManager.Text = "Quản lý List";
            this.btnListManager.Click += new System.EventHandler(this.quảnLýCâuHỏiToolStripMenuItem_Click);
            // 
            // thốngKêBáoCáoToolStripMenuItem
            // 
            this.thốngKêBáoCáoToolStripMenuItem.Name = "thốngKêBáoCáoToolStripMenuItem";
            this.thốngKêBáoCáoToolStripMenuItem.Size = new System.Drawing.Size(92, 32);
            this.thốngKêBáoCáoToolStripMenuItem.Text = "Kanban";
            this.thốngKêBáoCáoToolStripMenuItem.Click += new System.EventHandler(this.thốngKêBáoCáoToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hệThốngToolStripMenuItem,
            this.danhMụcToolStripMenuItem,
            this.thốngKêBáoCáoToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1600, 38);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Silver;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1600, 866);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MinimumSize = new System.Drawing.Size(1614, 897);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Phần mềm quản lý công việc";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem hệThốngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnUserManager;
        private System.Windows.Forms.ToolStripMenuItem thoátToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem danhMụcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnBoardManager;
        private System.Windows.Forms.ToolStripMenuItem btnListManager;
        private System.Windows.Forms.ToolStripMenuItem btnTaskManager;
        private System.Windows.Forms.ToolStripMenuItem thốngKêBáoCáoToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
    }
}

