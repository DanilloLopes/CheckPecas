using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckPecas
{
    public partial class frmControlePecasProduzidas : Form
    {
        public frmControlePecasProduzidas()
        {
            InitializeComponent();
        }
        private void frmControlePecasProduzidas_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dbCheckPecasDataSet.tblPecas' table. You can move, or remove it, as needed.
            this.tblPecasTableAdapter.Fill(this.dbCheckPecasDataSet.tblPecas);


            /*SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tblPecas", strConexao);
            DataTable tabela = new DataTable();
            da.Fill(tabela);
            cbPecas.DataSource = tabela;
            cbPecas.DisplayMember = "nomePeca";*/


            mtxtData.Focus();
        }

        private void mtxtData_Leave(object sender, EventArgs e)
        {
            if(DateTime.Parse(mtxtData.Text) > System.DateTime.Now)
            {
                MessageBox.Show("Data inválida!");
                mtxtData.ResetText();
                mtxtData.Focus();
            }
        }

    }
}
