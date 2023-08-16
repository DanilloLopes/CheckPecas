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

        public int cod = 11;

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

            txtReprovadas.ResetText();
            txtReprovadas.Enabled = false;
            txtAprovadas.Enabled = false;
            txtAprovadas.ResetText();
            txtEmail.Enabled = false;
            txtEmail.ResetText();
            txtPrejuizo.Enabled = false;
            txtPrejuizo.ResetText();

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
                        conexao.Close();

                        return;
                    }

                    double pecasProduzidas = pecasAprovadas + pecasReprovadas;

                    txtProduzidas.Text = pecasProduzidas.ToString();
                }
                else
                {
                    conexao.Close();
                    txtProduzidas.ResetText();
                }

                double prejuTotal = valorPreju * pecasReprovadas;
                conexao.Close();


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
        private void reiniciar()
        {
            txtReprovadas.ResetText();
            txtReprovadas.Enabled = false;
            
            txtAprovadas.ResetText();
            txtAprovadas.Enabled = false;

            txtEmail.ResetText();
            txtEmail.Enabled = false;

            txtPrejuizo.ResetText();
            txtPrejuizo.Enabled = false;

            cbPecas.SelectedIndex = -1;
            cbPecas.DropDownStyle = ComboBoxStyle.DropDownList;

            mtxtData.Clear();
            mtxtData.Focus();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {

   
            if (cbPecas.SelectedIndex != -1 && !string.IsNullOrEmpty(txtProduzidas.Text)) 
            {
                if(int.Parse(txtReprovadas.Text) > 10 && string.IsNullOrEmpty(txtEmail.Text))
                {
                    MessageBox.Show("Erro! Digite um email!");
                    return;
                }

                if(int.Parse(txtReprovadas.Text) > 10)
                {
                    if (txtEmail.Text.Contains("@"))
                    {
                        string email = txtEmail.Text;
                        string local = "";
                        string dominio = "";

                        for (int i = 0; i < email.Length; i++)
                        {
                            if (email.Substring(i, 1) != "@")
                            {
                                local = local + email.Substring(i + 1);
                            }
                            else
                            {
                                dominio = email.Substring(i + 1, email.Length - (i + 1));
                                i = email.Length;
                            }
                        }

                        if(local.Length <= 3)
                        {
                            MessageBox.Show("Erro! Digite um email válido!");
                            return;
                        }
                        if(!dominio.Contains("."))
                        {
                            MessageBox.Show("Erro! Digite um email válido!");
                            return;
                        }
                    }
                }

                conexao = new SqlConnection(strConexao);

                conexao.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conexao;
                cmd.CommandText = "INSERT INTO tblRegistros VALUES(@codUser, @codPeca, @dtReg, @aprov, @reprov,@prod, @preju, @email)";
                cmd.Parameters.AddWithValue("@codUSer", cod);
                cmd.Parameters.AddWithValue("@codPeca", cbPecas.SelectedValue);
                cmd.Parameters.AddWithValue("@dtReg", DateTime.Parse(mtxtData.Text));
                cmd.Parameters.AddWithValue("@aprov", txtAprovadas.Text);
                cmd.Parameters.AddWithValue("@reprov", txtReprovadas.Text);
                cmd.Parameters.AddWithValue("@prod", txtProduzidas.Text);
                cmd.Parameters.AddWithValue("@preju", txtPrejuizo.Text);
                cmd.Parameters.AddWithValue("@email", txtEmail.Text);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Registro foi efetuado com sucesso!");

                reiniciar();

            }
            else 
            {
                MessageBox.Show("Erro! Valores não inseridos!");
            }
        }
    }
}
