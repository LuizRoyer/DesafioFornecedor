using Fornecedores.Entities;
using Fornecedores.Entities.ViewEntities;
using Fornecedores.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Fornecedores.Repositories
{
    public class PesFisicaRepository : IPesFisicaRepository
    {
        private readonly SqlConnection _conn;
        private readonly SqlTransaction _trans;

        public PesFisicaRepository(SqlConnection conn, SqlTransaction trans)
        {
            this._conn = conn;
            this._trans = trans;
        }
        public void Add(PesFisica obj)
        {
            string sqlInsert = @"INSERT INTO dbo.PesFisica
                                    ( CPF,DATANASCIMENTO,RG,IDPessoa)
                                values
                                    (@cpf,@datanascimento,@rg,@idPessoa )";

            SqlCommand cmd = new SqlCommand(sqlInsert, _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@idPessoa", obj.IdPessoa));
            cmd.Parameters.Add(new SqlParameter("@cpf", obj.Cpf));
            cmd.Parameters.Add(new SqlParameter("@datanascimento", obj.DataNascimento));
            cmd.Parameters.Add(new SqlParameter("@rg", obj.Rg));

            cmd.ExecuteNonQuery();
        }

        public PesFisicaView Get(int id, string cpf)
        {
            StringBuilder sqlSelect = new StringBuilder();

            sqlSelect.Append(@"SELECT F.Id
                              ,F.IdPessoa
	                          ,P.Nome
                              ,P.Sobrenome                                   
                              ,F.Cpf
                              ,F.DataNascimento
                              ,F.Rg
                          FROM dbo.PesFisica F,
		                        dbo.Pessoa P
								
	                       WHERE F.IdPessoa =P.Id");

            if (!string.IsNullOrWhiteSpace(cpf))
                sqlSelect.Append("     AND F.CPF =@cpf");
            else
                sqlSelect.Append("     AND F.Id =@id");
            SqlCommand cmd = new SqlCommand(sqlSelect.ToString(), _conn);

            if (!string.IsNullOrWhiteSpace(cpf))
                cmd.Parameters.Add(new SqlParameter("@cpf", cpf));
            else
                cmd.Parameters.Add(new SqlParameter("@id", id));
            cmd.Transaction = _trans;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    return PopularObjeto(reader);
            }

            return new PesFisicaView();
        }

        public List<PesFisicaView> GetAll()
        {
            List<PesFisicaView> listaPesFisica = new List<PesFisicaView>();

            string sqlSelect = @"SELECT F.Id
                              ,F.IdPessoa
	                          ,P.Nome
                              ,P.Sobrenome                                   
                              ,F.Cpf
                              ,F.DataNascimento
                              ,F.Rg
                          FROM dbo.PesFisica F,
		                        dbo.Pessoa P
								
	                          WHERE F.IdPessoa =P.Id";

            SqlCommand cmd = new SqlCommand(sqlSelect, _conn);

            cmd.Transaction = _trans;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    listaPesFisica.Add(PopularObjeto(reader));
                }
            }

            return listaPesFisica;
        }

        public DateTime GetDataNascimento(int idPessoa)
        {
            string sqlSelect = @"SELECT DataNascimento                         
                                      FROM dbo.PesFisica 		                       							
	                                       WHERE 1=1
                                             AND IDPESSOA = @pessoa";

            SqlCommand cmd = new SqlCommand(sqlSelect, _conn);
            cmd.Parameters.Add(new SqlParameter("@pessoa", idPessoa));
            cmd.Transaction = _trans;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                    return reader["DATANASCIMENTO"] == DBNull.Value ? new DateTime(9999, 01, 01) : Convert.ToDateTime(reader["DATANASCIMENTO"]);
            }

            return new DateTime(9999, 01, 01);
        }

        public void Remove(int id)
        {
            string sqlDelete = @"DELETE FROM dbo.PesFisica
                                      WHERE ID =@id ";

            SqlCommand cmd = new SqlCommand(sqlDelete.ToString(), _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@id", id));

            cmd.ExecuteNonQuery();
        }

        public void Update(PesFisica obj)
        {
            string sqlUpdate = @"UPDATE dbo.PesFisica
                                     SET CPF =@cpf
                                        ,DATANASCIMENTO=@datanascimento
                                        ,RG =@rg
                                     WHERE ID =@id ";

            SqlCommand cmd = new SqlCommand(sqlUpdate.ToString(), _conn);

            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@cpf", obj.Cpf));
            cmd.Parameters.Add(new SqlParameter("@datanascimento", obj.DataNascimento));
            cmd.Parameters.Add(new SqlParameter("@rg", obj.Rg));
            cmd.Parameters.Add(new SqlParameter("@id", obj.Id));

            cmd.ExecuteNonQuery();
        }
        private PesFisicaView PopularObjeto(SqlDataReader reader)
        {
            return new PesFisicaView
            {
                Id = Convert.ToInt32(reader["ID"].ToString()),
                IdPessoa = Convert.ToInt32(reader["IDPESSOA"].ToString()),
                Nome = reader["NOME"].ToString().Trim(),
                Sobrenome = reader["SOBRENOME"].ToString().Trim(),
                Cpf = reader["CPF"].ToString().Trim(),
                DataNascimento = reader["DATANASCIMENTO"] == DBNull.Value ? new DateTime(9999, 01, 01) : Convert.ToDateTime(reader["DATANASCIMENTO"]),
                Rg = reader["RG"].ToString().Trim(),
            };
        }
    }
}

