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
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text;
            string senha = txtSenha.Text;

            conexao = new SqlConnection(strConexao); //INSTANCIA
            SqlCommand cmd = new SqlCommand();//comando de conecao
            cmd.Connection = conexao;
            cmd.CommandText = "SELECT loginUsuario FROM tblUsuarios WHERE loginUsuario = @valor";
            cmd.Parameters.AddWithValue("@valor", usuario);

            conexao.Open();//abriu conexao

            if(cmd.ExecuteScalar() == null)//executa o comando de conecao
            {
                DialogResult resposta = MessageBox.Show("Usuário inexistente! Deseja realizar o cadastro?", "Cadastro", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if(resposta == DialogResult.Yes) //PRECISA AJUSTAR: SENHA NULL REGISTRA
                {
                    cmd.CommandText = "INSERT INTO tblUsuarios VALUES(@login,@senha)";
                    cmd.Parameters.AddWithValue("@login", usuario);
                    cmd.Parameters.AddWithValue("@senha", senha);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Cadastro realizado com sucesso!");
                }
                else
                {
                    txtUsuario.ResetText();
                    txtSenha.ResetText();
                }
            }
            else
            {
                cmd.CommandText = "SELECT senhaUsuario FROM tblUsuarios WHERE loginUsuario = @user";
                cmd.Parameters.AddWithValue("@user", usuario);
                if(senha != cmd.ExecuteScalar().ToString())
                {
                    MessageBox.Show("A senha não confere!");
                }
                else
                {
                    frmControlePecasProduzidas fControle = new frmControlePecasProduzidas();
                    fControle.ShowDialog();

                }
            }
        }
    }
}
