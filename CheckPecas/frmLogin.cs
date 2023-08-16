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
    public partial class frmLogin : Form
    {
        string strConexao = @"Data Source=.\SQLEXPRESS;Initial Catalog=dbCheckPecas;User ID=sa;Password=sql2022";//
        SqlConnection conexao;//objeto de conecao

        int codigo;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtUsuario.Text) && !string.IsNullOrEmpty(txtSenha.Text))
            {
                string usuario = txtUsuario.Text;
                string senha = txtSenha.Text;

                conexao = new SqlConnection(strConexao); //INSTANCIA
                SqlCommand cmd = new SqlCommand();//comando de conecao
                cmd.Connection = conexao;
                cmd.CommandText = "SELECT senhaUsuario FROM tblUsuarios WHERE loginUsuario = @valor";
                cmd.Parameters.AddWithValue("@valor", usuario);

                conexao.Open();//abriu conexao

                var senhaTbl = cmd.ExecuteScalar() ?? "";
                
                if(senha == senhaTbl.ToString())
                {

                    cmd.CommandText = "SELECT codigo FROM tblUsuarios WHERE loginUsuario = @val";
                    cmd.Parameters.AddWithValue("@val", usuario);

                    codigo = Convert.ToInt32(cmd.ExecuteScalar());

                    conexao.Close();

                    frmControlePecasProduzidas fControle = new frmControlePecasProduzidas();
                    fControle.cod = codigo;
                    fControle.ShowDialog();
                }
                else
                {
                    if(senhaTbl.ToString() == "")
                    {
                        DialogResult resposta = MessageBox.Show("Usuário inexistente! Deseja realizar o cadastro?", "Cadastro", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (resposta == DialogResult.Yes) //PRECISA AJUSTAR: SENHA NULL REGISTRA
                        {
                            cmd.CommandText = "INSERT INTO tblUsuarios VALUES(@login,@senha)";
                            cmd.Parameters.AddWithValue("@login", usuario);
                            cmd.Parameters.AddWithValue("@senha", senha);
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Cadastro realizado com sucesso!");
                            conexao.Close();
                        }
                        else
                        {
                            conexao.Close();
                            txtUsuario.ResetText();
                            txtSenha.ResetText();
                        }
                    }
                    else
                    {
                        conexao.Close();
                        MessageBox.Show("A senha não confere!");
                    }
                }

            }
            else
            {
                MessageBox.Show("Usuário ou Senha não preenchidos!");
            }
           
        }

        
    }
}
