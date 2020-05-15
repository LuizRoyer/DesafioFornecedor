using Fornecedores.Entities;
using Fornecedores.IRepositories;
using System;
using System.Data.SqlClient;
using System.Text;

namespace Fornecedores.Repositories
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly SqlConnection _conn;
        private readonly SqlTransaction _trans;

        public PessoaRepository(SqlConnection conn, SqlTransaction trans)
        {
            this._conn = conn;
            this._trans = trans;
        }
        public void Add(Pessoa obj)
        {
            string sqlInsert = @"INSERT INTO dbo.Pessoa
                                     (NOME, SOBRENOME,TIPO)
                                values
                                     (@nome, @sobrenome,@tipo)";

            SqlCommand cmd = new SqlCommand(sqlInsert.ToString(), _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@nome", obj.Nome));
            cmd.Parameters.Add(new SqlParameter("@sobrenome", obj.Sobrenome));
            cmd.Parameters.Add(new SqlParameter("@tipo", obj.Tipo));

            cmd.ExecuteNonQuery();
        }

        public Pessoa Get(int id, string nome)
        {
            StringBuilder sqlSelect = new StringBuilder();

            sqlSelect.Append(@"SELECT ID ,
                                       NOME ,
                                       SOBRENOME                                      
                                FROM dbo.Pessoa                                  
                                     WHERE 1=1");
            if (!string.IsNullOrWhiteSpace(nome))
                sqlSelect.Append(" AND Nome =@nome ");
            else
                sqlSelect.Append(" AND Id =@id ");

            SqlCommand cmd = new SqlCommand(sqlSelect.ToString(), _conn);
            cmd.Transaction = _trans;
            if (!string.IsNullOrWhiteSpace(nome))
                cmd.Parameters.Add(new SqlParameter("@nome", nome));
            else
                cmd.Parameters.Add(new SqlParameter("@id", id));
            
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return new Pessoa
                    {
                        Id = Convert.ToInt32(reader["ID"].ToString()),
                        Nome = reader["NOME"].ToString(),
                        Sobrenome = reader["SOBRENOME"].ToString()
                    };
                }
            }

            return new Pessoa();
        }

        public void Remove(int id)
        {
            string sqlDelete = @"DELETE FROM dbo.Pessoa
                                     WHERE ID =@id ";

            SqlCommand cmd = new SqlCommand(sqlDelete, _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@id", id));
            cmd.ExecuteNonQuery();
        }

        public void Update(Pessoa obj)
        {
            string sqlUpdate = @"UPDATE dbo.Pessoa
                                    SET NOME =@nome
                                        ,SOBRENOME=@sobrenome                               
                                        ,TIPO=@tipo
                             WHERE ID =@id ";

            SqlCommand cmd = new SqlCommand(sqlUpdate.ToString(), _conn);

            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@nome", obj.Nome));
            cmd.Parameters.Add(new SqlParameter("@sobrenome", obj.Sobrenome));
            cmd.Parameters.Add(new SqlParameter("@tipo", obj.Tipo));
            cmd.Parameters.Add(new SqlParameter("@id", obj.Id));

            cmd.ExecuteNonQuery();
        }
    }
}
