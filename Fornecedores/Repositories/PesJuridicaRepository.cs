using Fornecedores.Entities;
using Fornecedores.Entities.ViewEntities;
using Fornecedores.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Fornecedores.Repositories
{
    public class PesJuridicaRepository : IPesJuridicaRepository
    {
        private readonly SqlConnection _conn;
        private readonly SqlTransaction _trans;

        public PesJuridicaRepository(SqlConnection conn, SqlTransaction trans)
        {
            this._conn = conn;
            this._trans = trans;
        }
        public void Add(PesJuridica obj)
        {
            string sqlInsert = @"INSERT INTO dbo.PesJuridica
                                    (CNPJ, IDPESSOA)
                                values
                                    (@cnpj , @idPessoa)";

            SqlCommand cmd = new SqlCommand(sqlInsert, _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@idPessoa", obj.IdPessoa));
            cmd.Parameters.Add(new SqlParameter("@cnpj", obj.Cnpj));

            cmd.ExecuteNonQuery();
        }
        public PesJuridicaView Get(int id, string cnpj)
        {
            StringBuilder sqlSelect = new StringBuilder();

            sqlSelect.Append(@"SELECT
                               J.Id
                              ,J.IdPessoa
	                          ,P.Nome
                              ,P.Sobrenome                              
                              ,P.tipo
                              ,J.Cnpj                             
                          FROM dbo.PesJuridica J,
		                        dbo.Pessoa P								
	                                WHERE J.IdPessoa = p.Id");

            if (!string.IsNullOrWhiteSpace(cnpj))
                sqlSelect.Append(" AND J.CNPJ = @cnpj");
            else
                sqlSelect.Append(" AND J.Id = @id");
            SqlCommand cmd = new SqlCommand(sqlSelect.ToString(), _conn);
            cmd.Transaction = _trans;

            if (!string.IsNullOrWhiteSpace(cnpj))
                cmd.Parameters.Add(new SqlParameter("@cnpj", cnpj));
            else
                cmd.Parameters.Add(new SqlParameter("@id", id));
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    return PopularObjeto(reader);
            }

            return new PesJuridicaView();
        }
        public List<PesJuridicaView> GetAll()
        {
            List<PesJuridicaView> listaPesJuridica = new List<PesJuridicaView>();

            string sqlSelect = @"SELECT
                               J.Id
                              ,J.IdPessoa
	                          ,P.Nome
                              ,P.Sobrenome                              
                              ,P.tipo
                              ,J.Cnpj                             
                          FROM dbo.PesJuridica J,
		                        dbo.Pessoa P								
	                                WHERE J.IdPessoa = p.Id";

            SqlCommand cmd = new SqlCommand(sqlSelect.ToString(), _conn);
            cmd.Transaction = _trans;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    listaPesJuridica.Add(PopularObjeto(reader));
                }
            }

            return listaPesJuridica;
        }

        public void Remove(int id)
        {
            string sqlDelete = @"DELETE FROM dbo.PesJuridica
                                         WHERE ID =@id ";

            SqlCommand cmd = new SqlCommand(sqlDelete.ToString(), _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@id", id));
            cmd.ExecuteNonQuery();
        }

        public void Update(PesJuridica obj)
        {
            string sqlUpdate = @"UPDATE dbo.PesJuridica
                                     SET CNPJ =@cnpj                                
                                         WHERE ID =@id ";

            SqlCommand cmd = new SqlCommand(sqlUpdate.ToString(), _conn);

            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@cnpj", obj.Cnpj));
            cmd.Parameters.Add(new SqlParameter("@id", obj.Id));

            cmd.ExecuteNonQuery();
        }
        private PesJuridicaView PopularObjeto(SqlDataReader reader)
        {
            return new PesJuridicaView
            {
                Id = Convert.ToInt32(reader["ID"].ToString()),
                IdPessoa = Convert.ToInt32(reader["IDPESSOA"].ToString()),
                Nome = reader["NOME"].ToString().Trim(),
                Sobrenome = reader["SOBRENOME"].ToString().Trim(),
                Cnpj = reader["CNPJ"].ToString().Trim()
            };
        }
    }
}
