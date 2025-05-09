using CadastroASP.NET.Models;
using MySql.Data.MySqlClient;
using CadastroASP.NET.Models;
using System.Data;


// Define o nome do projeto onde a classe UsuarioRepositorio está localizada.
namespace CadastroASP.NET.Repositorio
{
    // Define a classe UsuarioRepositorio, responsável por operações de acesso a dados para a entidade Usuario.
    public class LoginRepositorio(IConfiguration configuration)
    {
        // Declara um campo privado somente leitura para armazenar a string de conexão com o MySQL.
        private readonly string _conexaoMySQL = configuration.GetConnectionString("conexaoMySQL");


        /* Define um método público para obter um usuário do banco de dados com base no seu email.
                Recebe o email como parâmetro e retorna um objeto 'Usuario' ou null se não encontrado.*/
        public Usuario ObterUsuario(string email)
        {
            // Cria uma nova instância da conexão MySQL dentro de um bloco 'using'.
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL.
                conexao.Open();
                // Cria um novo comando SQL para selecionar todos os campos da tabela 'Usuario' onde o campo 'Email' corresponde ao parâmetro fornecido.
                MySqlCommand cmd = new("SELECT * FROM Usuario WHERE Email = @email", conexao);
                // Adiciona um parâmetro ao comando SQL para o campo 'Email', especificando o tipo como VarChar e utilizando o valor do parâmetro 'email'.
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;

                // Executa o comando SQL SELECT e obtém um leitor de dados (MySqlDataReader). O CommandBehavior.CloseConnection garante que a conexão
                // será fechada automaticamente quando o leitor for fechado.
                using (MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    // Inicializa uma variável 'usuario' como null. Ela será preenchida se um usuário for encontrado.
                    Usuario usuario = null;
                    // Lê a próxima linha do resultado da consulta. Retorna true se houver uma linha e false caso contrário.
                    if (dr.Read())
                    {
                        // Cria uma nova instância do objeto 'Usuario'.
                        usuario = new Usuario
                        {
                            // Lê o valor da coluna "Id" da linha atual do resultado, converte para inteiro e atribui à propriedade 'Id' do objeto 'usuario'.
                            Id = Convert.ToInt32(dr["Id"]),
                            // Lê o valor da coluna "Nome" da linha atual do resultado, converte para string e atribui à propriedade 'Nome' do objeto 'usuario'.
                            Nome = dr["Nome"].ToString(),
                            // Lê o valor da coluna "Email" da linha atual do resultado, converte para string e atribui à propriedade 'Email' do objeto 'usuario'.
                            Email = dr["Email"].ToString(),
                            // Lê o valor da coluna "Senha" da linha atual do resultado, converte para string e atribui à propriedade 'Senha' do objeto 'usuario'.
                            Senha = dr["Senha"].ToString()
                        };
                    }
                    /* Retorna o objeto 'usuario'. Se nenhum usuário foi encontrado com o email fornecido, a variável 'usuario'
                     permanecerá null e será retornado.*/
                    return usuario;
                }
            }
        }
    }
}