using Fornecedores.Entities;
using Fornecedores.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Fornecedores.Repositories
{
    public class EstadoRepository : IEstadoRepository
    {
        private readonly SqlConnection _conn;
        private readonly SqlTransaction _trans;

        public EstadoRepository(SqlConnection conn, SqlTransaction trans)
        {
            this._conn = conn;
            this._trans = trans;
        }

        public void Add(Estado obj)
        {
            string sqlInsert = @"INSERT INTO dbo.Estado
                                    (NOME, UF)
                                 values
                                    (@nome, @uf)";

            SqlCommand cmd = new SqlCommand(sqlInsert, _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@nome", obj.Nome));
            cmd.Parameters.Add(new SqlParameter("@uf", obj.Uf.ToUpper()));

            cmd.ExecuteNonQuery();
        }
              
        public Estado Get(Estado obj)
        {
            Estado Estado = new Estado();
            StringBuilder sb = new StringBuilder();

            sb.Append(@"SELECT Id , Nome , UF
                          FROM dbo.Estado");

            sb.Append(" where ID >0 ");

            if (obj.Id > 0)
                sb.Append(" AND ID =@id");
            else
                sb.Append(" AND UF =@uf");

            SqlCommand cmd = new SqlCommand(sb.ToString(), _conn);
            cmd.Transaction = _trans;
            if (obj.Id > 0)
                cmd.Parameters.Add(new SqlParameter("@id", obj.Id));
            else
                cmd.Parameters.Add(new SqlParameter("@uf", obj.Uf));
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return new Estado
                    {
                        Id = Convert.ToInt32(reader["ID"].ToString()),
                        Nome = reader["NOME"].ToString().Trim(),
                        Uf = reader["UF"].ToString().Trim()
                    };
                }
            }

            return Estado;
        }

        public List<Estado> GetAll()
        {

            string sqlSelect =@"SELECT Id , Nome , UF
                                     FROM dbo.Estado
                                        where ID > 0 
                                    ORDER BY ID";
            
            SqlCommand cmd = new SqlCommand(sqlSelect, _conn);
            cmd.Transaction = _trans;
         
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                List<Estado> Estados = new List<Estado>();

                while (reader.Read())
                {
                    Estado estado = new Estado
                    {
                        Id = Convert.ToInt32(reader["ID"].ToString()),
                        Nome = reader["NOME"].ToString().Trim(),
                        Uf = reader["UF"].ToString().Trim()
                    };

                    Estados.Add(estado);
                }
                return Estados;
            }
        }
        public void Update(Estado obj)
        {
            string sqlUpdate = @"UPDATE dbo.Estado
                                    SET NOME =@nome
                                        ,UF=@uf
                                     WHERE ID=@id ";

            SqlCommand cmd = new SqlCommand(sqlUpdate, _conn);

            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@nome", obj.Nome));
            cmd.Parameters.Add(new SqlParameter("@uf", obj.Uf.ToUpper()));
            cmd.Parameters.Add(new SqlParameter("@id", obj.Id));

            cmd.ExecuteNonQuery();
        }
        public void Remove(int id)
        {
            string sqlDelete = @"Delete from dbo.Estado
                                    where id= @id ";

            SqlCommand cmd = new SqlCommand(sqlDelete, _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@id", id));

            cmd.ExecuteNonQuery();
        }
    }
}
