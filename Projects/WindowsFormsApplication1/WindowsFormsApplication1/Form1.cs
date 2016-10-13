using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO:  这行代码将数据加载到表“dataSet1.ITCC_Contract_List”中。您可以根据需要移动或删除它。
            this.iTCC_Contract_ListTableAdapter.Fill(this.dataSet1.ITCC_Contract_List);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.iTCC_Contract_ListTableAdapter.Update(this.dataSet1.ITCC_Contract_List);
        }
    }
}
