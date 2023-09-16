using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        const int n = 3;
        public string[,] hide = new string[n * n, n * n];
        public int[,] solve = new int[n * n, n * n];
        public int[,] map = new int[n*n,n*n];
        public Button[,] btns = new Button[n*n,n*n];

        #region button event
        private void Create_Matrix_Click(object sender, EventArgs e)
        {
            GenerateMap();
            Button create = (Button)sender;
            create.Enabled = false;
            button_check.Enabled = true;
            Reset.Enabled = true;
            Solve.Enabled = true;
            timer1.Start();
        }
        int gtri;
        private void O1_Click(object sender, MouseEventArgs e)
        {
            
            Button btn = sender as Button;
            if (MouseButtons.Left == e.Button)
            {
                if (gtri == 9)
                {
                    gtri = 0;
                }
                gtri++;
            }
            else if (MouseButtons.Right == e.Button)
            {
                if (gtri == 1)
                {
                    gtri = 10;
                }
                gtri--;
            }
            btn.ForeColor = Color.Black;
            btn.Text = gtri.ToString();
        }
        private void button_check_Click(object sender, EventArgs e)
        {
            int flag = 0;
            for (int i =0; i < n*n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    var btnText = btns[i, j].Text;
                    if (btnText != map[i, j].ToString())
                    {
                        flag = 1;
                        btns[i,j].ForeColor = Color.Red;
                    }else
                    {
                        btns[i, j].ForeColor = Color.Blue;
                        
                    }  
                }
            }
            if (flag == 0)
                MessageBox.Show("Dung roi. Chuc mung ban!!!");
            
        }
        private void button_exit_Click(object sender, EventArgs e) => Close();
        private void button_Solve(object sender, EventArgs e)
        {
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    if (btns[i, j].Text.Equals(hide[i, j]))
                        btns[i, j].BackColor = Color.Gray;
                    else if (btns[i,j].Text == map[i, j].ToString())
                    {
                        btns[i, j].BackColor = Color.Yellow;
                    }
                    
                }
            }
            for (int i = 0; i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    btns[i, j].Text = solve[i, j].ToString();
                    btns[i, j].Enabled = false;
                }
            }
            timer1.Stop();
            Solve.Enabled = false;
            button_check.Enabled = false;
        }

        private void button_Reset(object sender, EventArgs e)
        {
            GenerateMap();
            button_check.Enabled = true;
            Solve.Enabled = true;
            value_timer = 300;
            label1.Text = "5:00";
            timer1.Start();
        }
        
        #endregion
        #region Tao bang sudoku
        public void GenerateMap()
        {
            for (int i = 0;i < n * n; i++)
            {
                for (int j = 0; j < n * n; j++)
                {
                    map[i,j] = (i*n+i/n+j) % (n*n)+1;
                    btns[i, j] = new Button();
                }
            }
            Random rd = new Random();
            for (int i = 0; i < 40; i++)
            {
                SuffleMap(rd.Next(0, 5));
            }
            CreateMap();
            HideCell();
        }
        public void CreateMap()
        {
            groupBox1.Controls.Clear();
            for (int j = 0; j < 9; j++)
            {
                for (int i = 0; i < 9; i++)
                {
                    Button btn = new System.Windows.Forms.Button();
                    btns[i, j] =btn;
                    btn.BackColor = System.Drawing.Color.White;
                    btn.FlatStyle = FlatStyle.Popup;

                    btn.Name = (j * 9 + i + 1).ToString();
                    btn.Size = new System.Drawing.Size(39, 34);
                    btn.TabIndex = 0;
                    btn.Text = map[i,j].ToString();

                    btn.UseVisualStyleBackColor = false;
                    btn.MouseUp += O1_Click;

                    btn.Location = new System.Drawing.Point(17 + i * 47, 22 + j * 40);
                    groupBox1.Controls.Add(btn);
                    solve[i, j] = map[i, j];
                }
            }
        }
        public void SuffleMap(int i)
        {
            switch (i)
            {
                case 0:
                    MatrixTransposition();
                    break;
                case 1:
                    SwapRowInBlock();
                    break;
                case 2:
                    SwapColInBlock();
                    break;
                case 3:
                    SwapBlocksInRow();
                    break;
                case 4:
                    SwapBlocksInCol();
                    break;
                default:
                    MatrixTransposition();
                    break;

            }
        }
        #region Doi cho lm cho cac o khong trung gia tri
        public void MatrixTransposition()
        {
            int[,] tmap = new int[n * n, n * n];
            for (int i = 0; i < n*n; i++) 
            {
                for (int j = 0; j < n*n; j++)
                {
                    tmap[i,j] = map[j,i];
                }
            }
            map = tmap;
        }
        public void SwapRowInBlock()
        {
            Random rd = new Random();
            var Block = rd.Next(0, n);
            var row1 = rd.Next(0,n);
            var line1 = Block * n + row1;
            var row2 = rd.Next(0, n);
            while (row1 == row2)
            {
                row2 = rd.Next(0, n);
            }
            var line2 = Block * n + row2;
            for (int i = 0; i < n*n;i++)
            {
                var temp = map[line1, i];
                map[line1,i] = map[line2,i];
                map[line2,i] = temp;
            }
        }

        public void SwapColInBlock()
        {
            Random rd = new Random();
            var Block = rd.Next(0, n);
            var row1 = rd.Next(0, n);
            var line1 = Block * n + row1;
            var row2 = rd.Next(0, n);
            while (row1 == row2)
            {
                row2 = rd.Next(0, n);
            }
            var line2 = Block * n + row2;
            for (int i = 0; i < n * n; i++)
            {
                var temp = map[i, line1];
                map[ i, line1] = map[i,line2];
                map[i,line2] = temp;
            }
        }

        public void SwapBlocksInRow()
        {
            Random rd = new Random();
            var Block1 = rd.Next(0, n);
            var Block2 = rd.Next(0, n);
            while(Block1 == Block2)
            {
                Block2 = rd.Next(0, n);
            }
            Block1 *= n;
            Block2 *= n;
            for (int i = 0; i < n*n; i++)
            {
                var k = Block2;
                for (int j = Block1; j < Block1 + n; j++)
                {
                    var temp = map[j, i];
                    map[j,i] = map[k, i];
                    map[k,i] = temp;
                    k++;
                }
            }
        }

        public void SwapBlocksInCol()
        {
            Random rd = new Random();
            var Block1 = rd.Next(0, n);
            var Block2 = rd.Next(0, n);
            while (Block1 == Block2)
            {
                Block2 = rd.Next(0, n);
            }
            Block1 *= n;
            Block2 *= n;
            for (int i = 0; i < n * n; i++)
            {
                var k = Block2;
                for (int j = Block1; j < Block1 + n; j++)
                {
                    var temp = map[i,j];
                    map[i,j] = map[i,k];
                    map[i,k] = temp;
                    k++;
                }
            }
        }
        #endregion
        public void HideCell()
        {
            int N = 41;
            Random rd = new Random();
            while (N > 0)
            {
                for (int i = 0; i < n * n; i++)
                {
                    for (int j = 0;j < n * n; j++)
                    {
                        if (!string.IsNullOrEmpty(btns[i,j].Text))
                        {
                            int a = rd.Next(0, 5);
                            btns[i, j].Text = a == 0 ? "": btns[i, j].Text; 
                            btns[i,j].Enabled = a == 0 ? true : false;
                            if (a == 0)
                            {
                                N--;
                            }
                            if (N <=0)
                            {
                                break;
                            }
                        }
                        if (btns[i, j].Text != "")
                        {
                            hide[i,j] = btns[i,j].Text;
                        }
                            
                        else
                        {
                            hide[i, j] = "-1";
                        }

                        if (N <= 0)
                        {
                            break;
                        }
                    }
                }
            }

            
        } //lam lung lo mot vai vtri trong bang 
        #endregion
        #region Tính Giờ
        int value_timer = 300;
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            DateTime dt = new DateTime();
            try
            {
                value_timer--;
                label1.Text = dt.AddSeconds(value_timer).ToString("mm:ss");
            }
            catch
            {
                timer1.Stop();
                MessageBox.Show("Hết giờ !!!", "Thông báo", MessageBoxButtons.OK);
                for (int i = 0; i < n*n; i++)
                {
                    for (int j = 0; j < n*n; j++)
                    {
                        btns[i,j].Enabled = false;
                        if (btns[i,j].Text == hide[i,j].ToString())
                            btns[i,j].BackColor = Color.LightGray;
                        else
                            btns[i, j].BackColor = Color.White;
                    }
                }
                
            }
            
        }
        #endregion
    }
}
