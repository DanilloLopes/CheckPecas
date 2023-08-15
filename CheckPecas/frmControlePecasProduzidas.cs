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

        public int cod = -1;

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

            cbPecas.SelectedIndex = -1;
            cbPecas.DropDownStyle = ComboBoxStyle.DropDownList;

            txtReprovadas.Enabled = false;
            txtAprovadas.Enabled = false;
            txtEmail.Enabled = false;
            txtPrejuizo.Enabled = false;

            mtxtData.Focus();
        }

        private void mtxtData_Leave(object sender, EventArgs e)
        {
            if (!mtxtData.MaskCompleted)
            {
                MessageBox.Show("Data inválida!");
                mtxtData.Focus();
                return;
            }
            if (DateTime.Parse(mtxtData.Text) > DateTime.Now )
            {
                MessageBox.Show("Data inválida!");
                mtxtData.ResetText();
                mtxtData.Focus();
            }
        }

        private void Valores_TextChanged(object sender, EventArgs e)
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
                double pecasReprovadas = Double.Parse(txtReprovadas.Text);

                if (!string.IsNullOrEmpty(txtAprovadas.Text))
                {
                    double pecasAprovadas = Convert.ToDouble(txtAprovadas.Text);

                    if (pecasReprovadas > pecasAprovadas)
                    {
                        MessageBox.Show("Erro! O valor de peças reprovadas não pode ser maior que as aprovadas.");

                        txtReprovadas.ResetText();
                        txtAprovadas.ResetText();
                        txtEmail.ResetText();
                        txtPrejuizo.ResetText();

                        return;
                    }

                    double pecasProduzidas = pecasAprovadas + pecasReprovadas;

                    txtProduzidas.Text = pecasProduzidas.ToString();
                }
                else
                {
                    txtProduzidas.ResetText();
                }

                double prejuTotal = valorPreju * pecasReprovadas;
                
                txtPrejuizo.Text = prejuTotal.ToString();

                if(pecasReprovadas < 10)
                {
                    txtEmail.Enabled = false;
                }
                else 
                {
                    txtEmail.Enabled = true;
                }
            }
            else
            {
                txtPrejuizo.ResetText();
                txtProduzidas.ResetText();
                txtEmail.ResetText();
            }
        }

        private void cbPecas_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtAprovadas.Enabled = true;
            txtReprovadas.Enabled = true;
        }

        private void _KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && !Char.IsControl(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if(cbPecas.SelectedIndex != -1 && !string.IsNullOrEmpty(txtProduzidas.Text)) 
            {
                if(int.Parse(txtReprovadas.Text) > 10 && string.IsNullOrEmpty(txtEmail.Text))
                {
                    MessageBox.Show("Erro! Digite um email!");
                    return;
                }

                conexao = new SqlConnection(strConexao);

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "INSERT INTO FROM tblPecas WHERE codigo = @cod";
                cmd.Parameters.AddWithValue("@cod", cbPecas.SelectedValue);
                
            }
            else 
            {
                MessageBox.Show("Erro! Valores não inseridos!");
            }
        }
    }
}
