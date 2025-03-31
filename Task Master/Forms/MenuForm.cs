using System;
using System.Windows.Forms;
using Task_Master;
using Task_Master.Forms;

namespace PM_THI_TN
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
        }
        private void quảnLýTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void làmBàiThiToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void quảnLýCâuHỏiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormList formList = new FormList();
            formList.Show();
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void thốngKêBáoCáoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainForm frmMain = new MainForm();
            frmMain.Show();
        }

        private void btnDoiMatKhau_Click(object sender, EventArgs e)
        {

        }

        private void btnBoardManager_Click(object sender, EventArgs e)
        {
            FormBoard formBoard = new FormBoard();
            formBoard.Show();
        }

        private void btnTaskManager_Click(object sender, EventArgs e)
        {
            FormTask formTask = new FormTask();
            formTask.Show();
        }
    }
}
