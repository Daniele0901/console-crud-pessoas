using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace appCrudPessoas
{
    internal class program
    {
        private static string connectionString = "Server=sql10.freesqldatabase.com;Database=sql10727350;Uid=sql10727350;Pwd=3y6BlDigUL;";
       static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("1 - Adicionar Pessoa ");
                Console.WriteLine("2 - Listar Pessoa ");
                Console.WriteLine("3 - Excluir Pessoa ");
                Console.WriteLine("4 - Editar Pessoa ");
                Console.WriteLine("5 - Sair ");
                Console.WriteLine("Escolha uma opção acima ");

                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        AdicionarPessoa();
                        break;

                    case "2":
                        ListarPessoa();
                        break;

                    case "3":
                        EditarPessoa();
                        break;

                    case "4":
                        ExcluirPessoa();
                        break;

                    case "5":
                        return;
                    default:
                        Console.WriteLine("Opção inválida");
                        break;

                }

            }
        }
        static void AdicionarPessoa()
        {
            Console.WriteLine("Informe o nome:");
            string nome = Console.ReadLine();

            Console.WriteLine("Informe o email:");
            string email = Console.ReadLine();

            Console.WriteLine("Informe a idade:");
            int idade = int.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO  pessoa(nome,email,idade) VALUES( @Nome , @Email, @Idade)";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@Nome", nome);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Idade", idade);
                cmd.ExecuteNonQuery();

                Console.WriteLine("Pessoa cadastrada com sucesso!");

            }
        }

        static void ListarPessoa()
        {
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Id, Nome, Idade, Email FROM pessoa";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Console.WriteLine($"Id: {reader["Id"]}, Nome: {reader["Nome"]}, Idade: {reader["Idade"]}, Email: {reader["Email"]}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Não existe pessoa cadastrada!");
                        }
                    }
                }

            }

        }

        static void ExcluirPessoa()
        {
            Console.WriteLine("Informe o Id da pessoa que deseja excluir:");
            int idExclusao = int.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM pessoa WHERE Id = @Id";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@Id", idExclusao);
                int linhaAfetada = cmd.ExecuteNonQuery();
                if (linhaAfetada > 0)
                {
                    Console.WriteLine("Pessoa excluida com sucesso!");
                }
                else
                {
                    Console.WriteLine("Pessoa não encontrada!");
                }
            }

        }

        static void EditarPessoa()
        {


            Console.WriteLine("Informe o Id da pessoa que deseja excluir:");
            int idEditado = int.Parse(Console.ReadLine());

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT* FROM pessoa WHERE Id = @Id";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                cmd.Parameters.AddWithValue("@Id", idEditado);


                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Console.WriteLine("Informe o novo nome: (* Deixe o capo em branco, para não alterar): ");
                        string novonome = Console.ReadLine();

                        Console.WriteLine("Informe o novo email: (* Deixe o capo em branco, para não alterar): ");
                        string emailnovo = Console.ReadLine();

                        Console.WriteLine("Informe a nova idade: (* Deixe o capo em branco, para não alterar): ");
                        string novaidade = Console.ReadLine();

                        reader.Close();

                        string queryUpdate = "UPDATE pessoa SET Nome= @Nome, Email=@Email, Idade=@idade WHERE Id = @Id";
                        cmd = new MySqlCommand(queryUpdate, connection);
                        cmd.Parameters.AddWithValue("@Nome", string.IsNullOrWhiteSpace(novonome) ? reader["Nome"] : novonome);
                        cmd.Parameters.AddWithValue("@Email", string.IsNullOrWhiteSpace(emailnovo) ? reader["Email"] : emailnovo);
                        cmd.Parameters.AddWithValue("@Idade", string.IsNullOrWhiteSpace(novaidade) ? reader["Idade"] : int.Parse(novaidade));
                        cmd.Parameters.AddWithValue("#Id", idEditado);

                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Pessoa editada com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("O ID da pessoa não exiate!");
                    }
                }
            }
        }
    }

}

    


