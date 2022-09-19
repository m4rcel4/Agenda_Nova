using MySql.Data.MySqlClient;
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

namespace Agenda_Nova
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            mostrar();
            btnInserir.Visible = true;
            btnDeletar.Visible = true;
            btnAlterar.Visible = true;
        }



        string continua = "yes";// quando iniciado o programa, os campos podem receb


        private void btnInserir_Click(object sender, EventArgs e)
        {
            verificaVazio();

            if (btnInserir.Text == "INSERIR" && continua == "yes")// mudou o texto do  botão. Caso "inserir", insere novo contato.
                                                                  // caso "novo", o MESMO botão faz outra coisa.
            {

                try
                {
                    using (MySqlConnection cnn = new MySqlConnection())
                    {
                        cnn.ConnectionString = "server=localhost;database=agenda;uid=root;pwd=;port=3306";
                        cnn.Open();
                        MessageBox.Show("Inserido com sucesso!");
                        string sql = "insert into contatos (nome, email) values ('" + txtNome.Text + "', '" + txtEmail.Text + "')";
                        MySqlCommand cmd = new MySqlCommand(sql, cnn);
                        cmd.ExecuteNonQuery();

                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            mostrar();
            limpar();
        }







        private void dgwTabela_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtID.Text = dgwTabela.CurrentRow.Cells[0].Value.ToString(); // o txtID vai receber a informaação do 1º campo da tabela
                                                                         // Cells[0] quer dizer a primeira posição
            txtNome.Text = dgwTabela.CurrentRow.Cells[1].Value.ToString();// o txt nome vai receber informações do segundo campo da tabela
                                                                          // Cell[1] é a segunda posição

            txtEmail.Text = dgwTabela.CurrentRow.Cells[2].Value.ToString();// cells[2] é a terceira posição


            //antes selecionar alguma coisa na tabela, o único botão a aparecer é o de inserir

            btnDeletar.Visible = false;
            btnAlterar.Visible = false;
            // lembrando que já defini, no início do código, que todos os 3 botões aparecem
            // mas aqui eu faço 2 sumirem, somente.

            btnInserir.Text = "NOVO"; // o botão inserir não pode funcionar quando já selecionei algo na tabela

            txtPesquisar.Text = ""; // faz sumir algo digitado anteriormente, limpa o txt


        }


        void limpar()
        {

            txtID.Text = "";
            txtNome.Text = "";
            txtEmail.Text = "";
            btnInserir.Text = "INSERIR";
            btnDeletar.Visible = false;
            btnAlterar.Visible = false;
        }




        void mostrar() // void mostrar é mostrar todas as informações do banco
        {
            try
            {
                using (MySqlConnection cnn = new MySqlConnection())
                {
                    cnn.ConnectionString = "server=localhost;database=agenda;uid=root;pwd=;port=3306";
                    cnn.Open();
                    string sql = "Select * from contatos";
                    DataTable table = new DataTable();
                    MySqlDataAdapter adpter = new MySqlDataAdapter(sql, cnn);
                    adpter.Fill(table);
                    dgwTabela.DataSource = table;

                    dgwTabela.AutoGenerateColumns = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }




        void verificaVazio() // void criado para impedir que campos vazios sejam inseridos no banco
        {
            if (txtNome.Text == "" || txtEmail.Text == "") // se o nome OU o email estiverem vazios, ele não insere
            {
                continua = "no"; // essa é uma variável criada no ínicio do código, usada no botão de inserir.
                MessageBox.Show("Preencha todos os campos");
            }
            else
            {
                continua = "yes";
            }
        }







        private void btnDeletar_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("Deseja excluir contato?", "Confirmação", MessageBoxButtons.YesNo))
            // criei uma messageBox, caso confirme que deseja excluir, o comando abaixo é realizado 

            {
                try
                {
                    using (MySqlConnection cnn = new MySqlConnection())
                    {
                        cnn.ConnectionString = "server=localhost;database=agenda;uid=root;pwd=;port=3306";
                        cnn.Open();
                        string sql = "Delete from contatos where idContatos = '" + txtID.Text + "'";
                        MySqlCommand cmd = new MySqlCommand(sql, cnn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show(" Deletado com sucesso! ");

                    }
                    limpar();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            mostrar();

        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection cnn = new MySqlConnection())
                {
                    cnn.ConnectionString = "server=localhost;database=agenda;uid=root;pwd=;port=3306";
                    cnn.Open();
                    string sql = "Update contatos set nome='" + txtNome.Text + "', email='" + txtEmail.Text + "' where idContatos='" + txtID.Text + "'";
                    MySqlCommand cmd = new MySqlCommand(sql, cnn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Atualizado com sucesso!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            mostrar();
        }

        private void txtPesquisar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection cnn = new MySqlConnection())
                {
                    cnn.ConnectionString = "server=localhost;database=agenda;uid=root;pwd=;port=3306";
                    cnn.Open();
                    string sql;

                    if (rbEmail.Checked)
                    {
                        sql = "Select * from contatos where email like'" + txtPesquisar.Text + "%'";
                    }
                    else // não defini o rbNome no if porque ele já vai estar selecionado automaticamente kkkkk
                    {
                        sql = "Select * from contatos where nome Like'" + txtPesquisar.Text + "%'";
                    }

                    MySqlCommand cmd = new MySqlCommand(sql, cnn);
                    cmd.ExecuteNonQuery();
                    DataTable table = new DataTable();
                    MySqlDataAdapter adpter = new MySqlDataAdapter(sql, cnn);
                    adpter.Fill(table);
                    dgwTabela.DataSource = table;

                    dgwTabela.AutoGenerateColumns = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

    }
}
