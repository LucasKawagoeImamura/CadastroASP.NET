using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using CadastroASP.NET.Models;
using System.Data;


namespace CadastroASP.NET.Repositorio
{
    // Define a classe responsável por interagir com os dados de produtos no banco de dados
    public class ProdutoRepositorio(IConfiguration configuration)
    {
        // Declara uma variável privada somente leitura para armazenar a string de conexão com o MySQL
        private readonly string _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");


        // Método para cadastrar um novo produto no banco de dados
        public void Cadastrar(Produto produto)
        {
            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();
                // Cria um novo comando SQL para inserir dados na tabela 'produto'
                MySqlCommand cmd = new MySqlCommand("insert into produto (Nome, Descricao, Preco, Quantidade) values (@nome, @descricao, @preco, @quantidade)", conexao); // @: PARAMETRO
                                                                                                                                                 // Adiciona um parâmetro para o nome, definindo seu tipo e valor
                cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.Nome;
                // Adiciona um parâmetro para o telefone, definindo seu tipo e valor
                cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                // Adiciona um parâmetro para o email, definindo seu tipo e valor
                cmd.Parameters.Add("@preco", MySqlDbType.VarChar).Value = produto.Preco;
                cmd.Parameters.Add("@quantidade", MySqlDbType.VarChar).Value = produto.Quantidade;
                // Executa o comando SQL de inserção e retorna o número de linhas afetadas
                cmd.ExecuteNonQuery();
                // Fecha explicitamente a conexão com o banco de dados (embora o 'using' já faça isso)
                conexao.Close();
            }
        }

        // Método para Editar (atualizar) os dados de um produto existente no banco de dados
        public bool Atualizar(Produto produto)
        {
            try
            {
                // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
                using (var conexao = new MySqlConnection(_conexaoMySQL))
                {
                    // Abre a conexão com o banco de dados MySQL
                    conexao.Open();
                    // Cria um novo comando SQL para atualizar dados na tabela 'produto' com base no código
                    MySqlCommand cmd = new MySqlCommand("Update produto set Nome=@nome, Descricao=@descricao, Preco=@preco, Quantidade=@quantidade " + " where Id=@id ", conexao);
                    // Adiciona um parâmetro para o código do produto a ser atualizado, definindo seu tipo e valor
                    cmd.Parameters.Add("@id", MySqlDbType.Int32).Value = produto.Id;
                    // Adiciona um parâmetro para o novo nome, definindo seu tipo e valor
                    cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = produto.Nome;
                    // Adiciona um parâmetro para o novo telefone, definindo seu tipo e valor
                    cmd.Parameters.Add("@descricao", MySqlDbType.VarChar).Value = produto.Descricao;
                    // Adiciona um parâmetro para o novo email, definindo seu tipo e valor
                    cmd.Parameters.Add("@preco", MySqlDbType.VarChar).Value = produto.Preco;
                    cmd.Parameters.Add("@quantidade", MySqlDbType.VarChar).Value = produto.Quantidade;
                    // Executa o comando SQL de atualização e retorna o número de linhas afetadas
                    //executa e verifica se a alteração foi realizada
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas > 0; // Retorna true se ao menos uma linha foi atualizada

                }
            }
            catch (MySqlException ex)
            {
                // Logar a exceção (usar um framework de logging como NLog ou Serilog)
                Console.WriteLine($"Erro ao atualizar produto: {ex.Message}");
                return false; // Retorna false em caso de erro

            }
        }

        // Método para listar todos os produtos do banco de dados
        public IEnumerable<Produto> TodosProdutos()
        {
            // Cria uma nova lista para armazenar os objetos Produto
            List<Produto> Produtolist = new List<Produto>();

            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();
                // Cria um novo comando SQL para selecionar todos os registros da tabela 'produto'
                MySqlCommand cmd = new MySqlCommand("SELECT * from produto", conexao);

                // Cria um adaptador de dados para preencher um DataTable com os resultados da consulta
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                // Cria um novo DataTable
                DataTable dt = new DataTable();
                // metodo fill- Preenche o DataTable com os dados retornados pela consulta
                da.Fill(dt);
                // Fecha explicitamente a conexão com o banco de dados 
                conexao.Close();

                // interage sobre cada linha (DataRow) do DataTable
                foreach (DataRow dr in dt.Rows)
                {
                    // Cria um novo objeto Produto e preenche suas propriedades com os valores da linha atual
                    Produtolist.Add(
                                new Produto
                                {
                                    Id = Convert.ToInt32(dr["Id"]), // Converte o valor da coluna "codigo" para inteiro
                                    Nome = ((string)dr["Nome"]), // Converte o valor da coluna "nome" para string
                                    Descricao = ((string)dr["Descricao"]), // Converte o valor da coluna "telefone" para string
                                    Preco = Convert.ToDouble(dr["Preco"]), // Converte o valor da coluna "email" para string
                                    Quantidade = Convert.ToInt32(dr["Quantidade"])
                                });
                }
                // Retorna a lista de todos os produtos
                return Produtolist;
            }
        }

        // Método para buscar um produto específico pelo seu código (Codigo)
        public Produto ObterProduto(int Codigo)
        {
            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();
                // Cria um novo comando SQL para selecionar um registro da tabela 'produto' com base no código
                MySqlCommand cmd = new MySqlCommand("SELECT * from produto where Id=@codigo ", conexao);

                // Adiciona um parâmetro para o código a ser buscado, definindo seu tipo e valor
                cmd.Parameters.AddWithValue("@codigo", Codigo);

                // Cria um adaptador de dados (não utilizado diretamente para ExecuteReader)
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                // Declara um leitor de dados do MySQL
                MySqlDataReader dr;
                // Cria um novo objeto Produto para armazenar os resultados
                Produto produto = new Produto();

                /* Executa o comando SQL e retorna um objeto MySqlDataReader para ler os resultados
                CommandBehavior.CloseConnection garante que a conexão seja fechada quando o DataReader for fechado*/

                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                // Lê os resultados linha por linha
                while (dr.Read())
                {
                    // Preenche as propriedades do objeto Produto com os valores da linha atual
                    produto.Id = Convert.ToInt32(dr["Id"]);//propriedade Codigo e convertendo para int
                    produto.Nome = (string)(dr["Nome"]); // propriedade Nome e passando string
                    produto.Descricao = (string)(dr["Descricao"]); //propriedade telefone e passando string
                    produto.Preco = Convert.ToDouble(dr["Preco"]); //propriedade email e passando string
                    produto.Quantidade = Convert.ToInt32(dr["Quantidade"]);
                }
                // Retorna o objeto Produto encontrado (ou um objeto com valores padrão se não encontrado)
                return produto;
            }
        }


        // Método para excluir um produto do banco de dados pelo seu código (ID)
        public void Excluir(int Id)
        {
            // Bloco using para garantir que a conexão seja fechada e os recursos liberados após o uso
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                // Abre a conexão com o banco de dados MySQL
                conexao.Open();

                // Cria um novo comando SQL para deletar um registro da tabela 'produto' com base no código
                MySqlCommand cmd = new MySqlCommand("delete from produto where Id=@codigo", conexao);

                // Adiciona um parâmetro para o código a ser excluído, definindo seu tipo e valor
                cmd.Parameters.AddWithValue("@codigo", Id);

                // Executa o comando SQL de exclusão e retorna o número de linhas afetadas
                int i = cmd.ExecuteNonQuery();

                conexao.Close(); // Fecha  a conexão com o banco de dados
            }
        }
    }
}
