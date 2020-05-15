using Fornecedores.Entities;
using Fornecedores.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Fornecedores.Repositories
{
    public class ContatoRepository : IContatoRepository
    {
        private readonly SqlConnection _conn;
        private readonly SqlTransaction _trans;

        public ContatoRepository(SqlConnection conn, SqlTransaction trans)
        {
            this._conn = conn;
            this._trans = trans;
        }
        public void Add(Contato obj)
        {
            StringBuilder sqlInsert = new StringBuilder();

            sqlInsert.Append("INSERT INTO dbo.Contato ");
            if (!string.IsNullOrWhiteSpace(obj.Telefone) && !string.IsNullOrWhiteSpace(obj.Celular))
            {
                sqlInsert.Append(@"(TELEFONE,CELULAR, IDPessoa)
                                values
                             (@telefone, @celular,@idPessoa)");
            }
            else if (!string.IsNullOrWhiteSpace(obj.Telefone))
            {
                sqlInsert.Append(@"(TELEFONE, IDPessoa)
                                values
                             (@telefone,@idPessoa)");
            }
            else
            {
                sqlInsert.Append(@"(CELULAR, IDPessoa)
                                values
                             (@celular,@idPessoa)");
            }

            SqlCommand cmd = new SqlCommand(sqlInsert.ToString(), _conn);
            cmd.Transaction = _trans;
            if (!string.IsNullOrWhiteSpace(obj.Telefone))
                cmd.Parameters.Add(new SqlParameter("@telefone", obj.Telefone));
            if (!string.IsNullOrWhiteSpace(obj.Celular))
                cmd.Parameters.Add(new SqlParameter("@celular", obj.Celular));
            cmd.Parameters.Add(new SqlParameter("@idPessoa", obj.IdPessoa));

            cmd.ExecuteNonQuery();
        }
        public Contato Get(int id, string telefone, string celular, int idPessoa)
        {
            StringBuilder sqlSelect = new StringBuilder();

            sqlSelect.Append(@"SELECT ID, TELEFONE , CELULAR , IDPESSOA
                                    FROM dbo.Contato
                                         WHERE 1=1");

            if (idPessoa == 0)
                sqlSelect.Append(" AND ID = @id");
            else
            {
                if (!string.IsNullOrWhiteSpace(telefone) && !string.IsNullOrWhiteSpace(celular))
                {
                    sqlSelect.Append(" AND IDPESSOA = @idPessoa");
                    sqlSelect.Append(" AND TELEFONE = @telefone");
                    sqlSelect.Append(" AND CELULAR = @celular");
                }
                else if (!string.IsNullOrWhiteSpace(telefone))
                {
                    sqlSelect.Append(" AND IDPESSOA = @idPessoa");
                    sqlSelect.Append(" AND TELEFONE = @telefone");
                }
                else
                {
                    sqlSelect.Append(" AND IDPESSOA = @idPessoa");
                    sqlSelect.Append(" AND CELULAR = @celular");
                }
            }


            SqlCommand cmd = new SqlCommand(sqlSelect.ToString(), _conn);
            cmd.Transaction = _trans;

            if (idPessoa == 0)
                cmd.Parameters.Add(new SqlParameter("@id", id));
            else
            {
                if (!string.IsNullOrWhiteSpace(telefone) && !string.IsNullOrWhiteSpace(celular))
                {
                    cmd.Parameters.Add(new SqlParameter("@idPessoa", idPessoa));
                    cmd.Parameters.Add(new SqlParameter("@telefone", telefone));
                    cmd.Parameters.Add(new SqlParameter("@celular", celular));
                }
                else if (!string.IsNullOrWhiteSpace(telefone))
                {
                    cmd.Parameters.Add(new SqlParameter("@idPessoa", idPessoa));
                    cmd.Parameters.Add(new SqlParameter("@telefone", telefone));
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@idPessoa", idPessoa));
                    cmd.Parameters.Add(new SqlParameter("@celular", celular));
                }

            }

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    return PopularObjeto(reader);
            }

            return new Contato();
        }

        public Contato Get(int id)
        {
           return this.Get(id, string.Empty, string.Empty, default(int));
        }

        public List<Contato> GetAll(int idPessoa)
        {
            List<Contato> listaContato = new List<Contato>();
            StringBuilder sqlSelect = new StringBuilder();
            sqlSelect.Append(@"SELECT ID, TELEFONE , CELULAR , IDPESSOA
                                    FROM dbo.Contato
                                        WHERE 1=1 ");

            if (idPessoa > 0)
                sqlSelect.Append(" AND IDPESSOA = @idPessoa");

            sqlSelect.Append(" ORDER BY Id ");

            SqlCommand cmd = new SqlCommand(sqlSelect.ToString(), _conn);
            cmd.Transaction = _trans;
            if (idPessoa > 0)
                cmd.Parameters.Add(new SqlParameter("@idPessoa", idPessoa));
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {                   
                    listaContato.Add(PopularObjeto(reader));
                }
            }

            return listaContato;
        }

        public void Remove(int id)
        {
            string sqlDelete = @"DELETE FROM dbo.Contato                 
                                      WHERE ID = @id ";

            SqlCommand cmd = new SqlCommand(sqlDelete.ToString(), _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@id", id));

            cmd.ExecuteNonQuery();
        }

        public void Update(Contato obj)
        {
            string sqlUpdate = @"UPDATE dbo.Contato
                                    SET Telefone =@telefone
                                        ,CELULAR=@celular
                                     WHERE ID =@id ";

            SqlCommand cmd = new SqlCommand(sqlUpdate.ToString(), _conn);

            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@telefone", obj.Telefone));
            cmd.Parameters.Add(new SqlParameter("@celular", obj.Celular));
            cmd.Parameters.Add(new SqlParameter("@id", obj.Id));

            cmd.ExecuteNonQuery();
        }
        private Contato PopularObjeto(SqlDataReader reader)
        {
            return new Contato
            {
                Id = Convert.ToInt32(reader["ID"].ToString()),
                Telefone = reader["TELEFONE"].ToString(),
                Celular = reader["CELULAR"].ToString(),
                IdPessoa = Convert.ToInt32(reader["IDPESSOA"].ToString()),
            };
        }
    }
}
