using Fornecedores.Entities;
using Fornecedores.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Fornecedores.Repositories
{
    public class BairroRepository : IBairroRepository
    {
        private readonly SqlConnection _conn;
        private readonly SqlTransaction _trans;

        public BairroRepository(SqlConnection conn, SqlTransaction trans)
        {
            this._conn = conn;
            this._trans = trans;
        }
        public void Add(Bairro obj)
        {
            string sqlInsert = @"INSERT INTO dbo.Bairro
                                    (NOME, IdCidade)
                                values
                                    (@nome, @cidade)";

            SqlCommand cmd = new SqlCommand(sqlInsert.ToString(), _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@nome", obj.Nome));
            cmd.Parameters.Add(new SqlParameter("@cidade", obj.IdCidade));

            cmd.ExecuteNonQuery();
        }      
        public Bairro Get(int id , string nome, int idCidade)
        {
            StringBuilder sqlSelect = new StringBuilder();

            sqlSelect.Append(@"SELECT Id,  Nome , IdCidade
                                    FROM dbo.Bairro 
                                       WHERE 1=1");

            if (string.IsNullOrWhiteSpace(nome) && idCidade == 0)
                sqlSelect.Append(" AND Id = @id");
            if (!string.IsNullOrWhiteSpace(nome))
                sqlSelect.Append(" AND Nome = @nome");
            if (idCidade > 0)
                sqlSelect.Append(" AND IdCidade = @cidade");

            SqlCommand cmd = new SqlCommand(sqlSelect.ToString(), _conn);

            if (string.IsNullOrWhiteSpace(nome) && idCidade == 0)
                cmd.Parameters.Add(new SqlParameter("@id", id));
            if (!string.IsNullOrWhiteSpace(nome))
                cmd.Parameters.Add(new SqlParameter("@nome", nome));
            if (idCidade > 0)
                cmd.Parameters.Add(new SqlParameter("@cidade", idCidade));
         

            cmd.Transaction = _trans;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return new Bairro
                    {
                        Id = Convert.ToInt32(reader["ID"].ToString()),
                        Nome = reader["NOME"].ToString().Trim(),
                        IdCidade = Convert.ToInt32(reader["IDCIDADE"].ToString())
                    };
                }
            }

            return new Bairro();
        }

        public Bairro Get(int id)
        {
            return this.Get(id, string.Empty, default(int));
        }

        public List<Bairro> GetAll()
        {
            string sqlSelect = @"SELECT Id,  Nome , IdCidade
                                    FROM dbo.Bairro 
                                       WHERE 1=1
                                    ORDER BY ID ";

            SqlCommand cmd = new SqlCommand(sqlSelect.ToString(), _conn);

            cmd.Transaction = _trans;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                List<Bairro> Bairros = new List<Bairro>();
                while (reader.Read())
                {
                    Bairros.Add(new Bairro
                    {
                        Id = Convert.ToInt32(reader["ID"].ToString()),
                        Nome = reader["NOME"].ToString().Trim(),
                        IdCidade = Convert.ToInt32(reader["IDCIDADE"].ToString())
                    });
                }
                return Bairros;
            }
        }
              
        public void Remove(int id)
        {
            string sqlDelete = @"Delete from dbo.Bairro
                                    where id= @id";

            SqlCommand cmd = new SqlCommand(sqlDelete, _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@id", id));

            cmd.ExecuteNonQuery();
        }

        public void Update(Bairro obj)
        {
            string sqlUpdate = @"UPDATE dbo.Bairro
                                     SET NOME =@nome
                                        ,IdCidade=@cidade
                                     WHERE Id =@id ";

            SqlCommand cmd = new SqlCommand(sqlUpdate, _conn);

            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@nome", obj.Nome));
            cmd.Parameters.Add(new SqlParameter("@cidade", obj.IdCidade));
            cmd.Parameters.Add(new SqlParameter("@id", obj.Id));

            cmd.ExecuteNonQuery();
        }
    }
}
