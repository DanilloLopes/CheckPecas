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
        string strConexao = @"Data Source=.\SQLEXPRESS;Initial Catalog=dbCheckPecas;User ID=sa;Password=sql2022";
        SqlConnection conexao;
        public frmControlePecasProduzidas()
        {
            InitializeComponent();
        }
        private void frmControlePecasProduzidas_Load(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM tblPecas", strConexao);
            DataTable tabela = new DataTable();
            da.Fill(tabela);
            cbPecas.DataSource = tabela;
            cbPecas.DisplayMember = "nomePeca";
            cbPecas.ValueMember = "codigo";

            mtxtData.Focus();
        }

        private void mtxtData_Leave(object sender, EventArgs e)
        {
            if (DateTime.Parse(mtxtData.Text) > System.DateTime.Now)
            {
                MessageBox.Show("Data inválida!");
                mtxtData.ResetText();
                mtxtData.Focus();
            }
        }

        private void txtReprovadas_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtReprovadas.Text))
            {
                conexao = new SqlConnection(strConexao);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT prejuizo FROM tblPecas WHERE codigo = @cod";
                cmd.Parameters.AddWithValue("@cod", cbPecas.SelectedValue);
                conexao.Open();
                double valorPreju = Convert.ToDouble(cmd.ExecuteScalar());
                double prejuTotal = valorPreju * Double.Parse(txtReprovadas.Text);
                txtPrejuizo.Text = prejuTotal.ToString();
            }
            else
            {
                txtPrejuizo.ResetText();
            }
        }
    }
}
