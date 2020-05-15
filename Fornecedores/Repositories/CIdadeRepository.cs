using Fornecedores.Entities;
using Fornecedores.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Fornecedores.Repositories
{
    public class CidadeRepository : ICidadeRepository
    {
        private readonly SqlConnection _conn;
        private readonly SqlTransaction _trans;

        public CidadeRepository(SqlConnection conn, SqlTransaction trans)
        {
            this._conn = conn;
            this._trans = trans;
        }
        public void Add(Cidade obj)
        {
            string sqlInsert = @"INSERT INTO dbo.Cidade
                                    (NOME, IDESTADO)
                                values
                                    (@nome, @estado)";

            SqlCommand cmd = new SqlCommand(sqlInsert, _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@nome", obj.Nome));
            cmd.Parameters.Add(new SqlParameter("@estado", obj.IdEstado));

            cmd.ExecuteNonQuery();
        }
        public Cidade Get(Cidade obj)
        {
            StringBuilder sqlSelect = new StringBuilder();

            sqlSelect.Append(@"SELECT Id,  Nome , IdEstado
                                    FROM dbo.Cidade
                                        WHERE 1=1");

            if (obj.Id > 0)
                sqlSelect.Append(" AND Id= @id");
            else
            {
                if (!string.IsNullOrWhiteSpace(obj.Nome))
                    sqlSelect.Append(" AND NOME = @nome");

                if (obj.IdEstado > 0)
                    sqlSelect.Append(" AND IdEstado = @estado");
            }

            SqlCommand cmd = new SqlCommand(sqlSelect.ToString(), _conn);
            if (obj.Id > 0)
                cmd.Parameters.Add(new SqlParameter("@id", obj.Id));
            else
            {
                if (!string.IsNullOrWhiteSpace(obj.Nome))
                    cmd.Parameters.Add(new SqlParameter("@nome", obj.Nome));
                if (obj.IdEstado > 0)
                    cmd.Parameters.Add(new SqlParameter("@estado", obj.IdEstado));
            }

            cmd.Transaction = _trans;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return new Cidade
                    {
                        Id = Convert.ToInt32(reader["ID"].ToString()),
                        Nome = reader["NOME"].ToString().Trim(),
                        IdEstado = Convert.ToInt32(reader["IDESTADO"].ToString()),
                    };
                }
            }

            return new Cidade();
        }
        public List<Cidade> GetAll()
        {
            string sqlSelect = @"SELECT Id, Nome , IdEstado
                                    FROM dbo.Cidade
                                        WHERE 1=1
                                    ORDER BY Id";

            SqlCommand cmd = new SqlCommand(sqlSelect, _conn);
            cmd.Transaction = _trans;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                List<Cidade> cidades = new List<Cidade>();
                while (reader.Read())
                {
                    cidades.Add(new Cidade
                    {
                        Id = Convert.ToInt32(reader["ID"].ToString()),
                        Nome = reader["NOME"].ToString().Trim(),
                        IdEstado = Convert.ToInt32(reader["IDESTADO"].ToString()),
                    });
                }
                return cidades;
            }
        }

        public void Remove(int id)
        {
            string sqlDelete = @"Delete from dbo.Cidade
                                    where id= @id";

            SqlCommand cmd = new SqlCommand(sqlDelete, _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@id", id));

            cmd.ExecuteNonQuery();
        }

        public void Update(Cidade obj)
        {
            string sqlUpdate = @"UPDATE dbo.Cidade
                                     SET NOME =@nome
                                        ,IDESTADO=@estado
                                     WHERE ID =@id ";

            SqlCommand cmd = new SqlCommand(sqlUpdate, _conn);

            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@nome", obj.Nome));
            cmd.Parameters.Add(new SqlParameter("@estado", obj.IdEstado));
            cmd.Parameters.Add(new SqlParameter("@id", obj.Id));

            cmd.ExecuteNonQuery();
        }
    }
}
