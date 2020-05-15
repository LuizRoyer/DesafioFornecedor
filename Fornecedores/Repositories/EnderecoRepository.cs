using Fornecedores.Entities;
using Fornecedores.Entities.ViewModels;
using Fornecedores.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Fornecedores.Repositories
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly SqlConnection _conn;
        private readonly SqlTransaction _trans;

        public EnderecoRepository(SqlConnection conn, SqlTransaction trans)
        {
            this._conn = conn;
            this._trans = trans;
        }

        public void Add(Endereco obj)
        {
            string sqlInsert = @"INSERT INTO dbo.Endereco
                                    (Logradouro, Complemento, IdBairro ,IdEstado, IdCidade, Cep)
                                 values
                                 (@logradouro ,@complemento,@idBairro ,@idEstado,@idCidade,@cep)";

            SqlCommand cmd = new SqlCommand(sqlInsert.ToString(), _conn);
            cmd.Transaction = _trans;

            cmd.Parameters.Add(new SqlParameter("@logradouro", obj.Logradouro));
            cmd.Parameters.Add(new SqlParameter("@complemento", obj.Complemento));
            cmd.Parameters.Add(new SqlParameter("@idBairro", obj.IdBairro));
            cmd.Parameters.Add(new SqlParameter("@idEstado", obj.IdEstado));
            cmd.Parameters.Add(new SqlParameter("@idCidade", obj.IdCidade));
            cmd.Parameters.Add(new SqlParameter("@cep", obj.Cep));

            cmd.ExecuteNonQuery();
        }
        public EnderecoView Get(int id)
        {
            StringBuilder sqlSelect = new StringBuilder();

            sqlSelect.Append(@"SELECT E.Id as IdEndereco,
                                      E.Cep ,
                                      E.IdEstado ,
                                      ES.Uf ,
                                      ES.Nome AS NomeEstado ,
                                      E.IdCidade ,
                                      C.Nome AS NomeCidade ,
                                      E.IdBairro ,
                                      B.Nome AS NomeBairro ,
                                      E.Logradouro ,
                                      E.Complemento
                                FROM dbo.Endereco E,
                                     dbo.Estado ES,
                                     dbo.Cidade C,
                                     dbo.Bairro B
                                WHERE E.Id >0								
								    AND E.IdEstado = ES.Id
                                    AND E.IdEstado = C.IdEstado
                                    AND E.idCidade = C.Id
                                    AND E.IdBairro = B.Id
                                    AND E.idCidade = B.idCidade");

            sqlSelect.Append(" AND  E.Id = @endereco");

            SqlCommand cmd = new SqlCommand(sqlSelect.ToString(), _conn);
            cmd.Transaction = _trans;

            cmd.Parameters.Add(new SqlParameter("@endereco", id));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    return PopularObjetoEnderecoView(reader);

            }
            return new EnderecoView();
        }
        public List<EnderecoView> GetAll()
        {
            List<EnderecoView> enderecos = new List<EnderecoView>();

            string sqlSelect = @"SELECT E.Id as IdEndereco,
                                      E.Cep ,
                                      E.IdEstado ,
                                      ES.Uf ,
                                      ES.Nome AS NomeEstado ,
                                      E.IdCidade ,
                                      C.Nome AS NomeCidade ,
                                      E.IdBairro ,
                                      B.Nome AS NomeBairro ,
                                      E.Logradouro ,
                                      E.Complemento
                                FROM dbo.Endereco E,
                                     dbo.Estado ES,
                                     dbo.Cidade C,
                                     dbo.Bairro B
                                WHERE E.Id >0								
								    AND E.IdEstado = ES.Id
                                    AND E.IdEstado = C.IdEstado
                                    AND E.idCidade = C.Id
                                    AND E.IdBairro = B.Id
                                    AND E.idCidade = B.idCidade

                                ORDER BY E.Id ";

            SqlCommand cmd = new SqlCommand(sqlSelect, _conn);
            cmd.Transaction = _trans;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    enderecos.Add(PopularObjetoEnderecoView(reader));

            }
            return enderecos;
        }

        public void Update(Endereco obj)
        {
            string sqlUpdate = @"UPDATE dbo.Endereco
                                SET  
                                    Logradouro=@logradouro                
                                   ,Complemento=@complemento
                                   ,IdBairro=@idBairro
                                   ,IdEstado=@idEstado
                                   ,idCidade=@idCidade
                                   ,Cep=@cep
                             WHERE Id =@id ";

            SqlCommand cmd = new SqlCommand(sqlUpdate.ToString(), _conn);

            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@id", obj.Id));
            cmd.Parameters.Add(new SqlParameter("@logradouro", obj.Logradouro));
            cmd.Parameters.Add(new SqlParameter("@complemento", obj.Complemento));
            cmd.Parameters.Add(new SqlParameter("@idBairro", obj.IdBairro));
            cmd.Parameters.Add(new SqlParameter("@idEstado", obj.IdEstado));
            cmd.Parameters.Add(new SqlParameter("@idCidade", obj.IdCidade));
            cmd.Parameters.Add(new SqlParameter("@cep", obj.Cep));

            cmd.ExecuteNonQuery();
        }

        public void Remove(int id)
        {
            string sqlDelete = @"Delete from dbo.Endereco
                                    where id= @id ";

            SqlCommand cmd = new SqlCommand(sqlDelete, _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@id", id));

            cmd.ExecuteNonQuery();
        }
        public EnderecoView PopularObjetoEnderecoView(SqlDataReader reader)
        {
            return new EnderecoView
            {
                IdEndereco = Convert.ToInt32(reader["IdEndereco"].ToString()),
                Cep = reader["CEP"].ToString().Trim(),
                IdEstado = Convert.ToInt32(reader["IdEstado"].ToString()),
                Uf = reader["UF"].ToString(),
                NomeEstado = reader["NomeEstado"].ToString().Trim(),
                IdCidade = Convert.ToInt32(reader["IdCidade"].ToString()),
                Localidade = reader["NomeCidade"].ToString().Trim(),
                IdBairro = Convert.ToInt32(reader["IdBairro"].ToString()),
                Bairro = reader["NomeBairro"].ToString().Trim(),
                Logradouro = reader["Logradouro"].ToString().Trim(),
                Complemento = reader["Complemento"].ToString().Trim()
            };
        }
    }
}
