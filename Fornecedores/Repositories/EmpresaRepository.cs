using Fornecedores.Entities;
using Fornecedores.Entities.ViewModels;
using Fornecedores.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Fornecedores.Repositories
{
    public class EmpresaRepository : IEmpresaRepository
    {
        private readonly SqlConnection _conn;
        private readonly SqlTransaction _trans;

        public EmpresaRepository(SqlConnection conn, SqlTransaction trans)
        {
            this._conn = conn;
            this._trans = trans;
        }
        public void Add(Empresa obj)
        {
            string sqlInsert = @"INSERT INTO dbo.Empresa
                                    (NOMEFANTASIA, CNPJ,IDENDERECO)
                                values
                                    (@nome, @cnpj , @endereco)";

            SqlCommand cmd = new SqlCommand(sqlInsert, _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@nome", obj.NomeFantasia));
            cmd.Parameters.Add(new SqlParameter("@cnpj", obj.CNPJ));
            cmd.Parameters.Add(new SqlParameter("@endereco", obj.IdEndereco));

            cmd.ExecuteNonQuery();
        }
                public EmpresaView Get(int id)
        {
            StringBuilder sqlSelect = new StringBuilder();

            sqlSelect.Append(@"SELECT EN.Id
                                      ,EN.NomeFantasia
                                      ,EN.IdEndereco
                                      ,EN.CNPJ
                                      ,E.Id
                                      ,E.Cep ,
                                      E.IdEstado ,
                                      ES.Uf ,
                                      ES.Nome AS NomeEstado ,
                                      E.IdCidade ,
                                      C.Nome AS NomeCidade ,
                                      E.IdBairro ,
                                      B.Nome AS NomeBairro ,
                                      E.Logradouro ,
                                      E.Complemento
                                FROM dbo.Empresa EN,
                                     dbo.Endereco E,
                                     dbo.Estado ES,
                                     dbo.Cidade C,
                                     dbo.Bairro B
                                WHERE EN.idEndereco = E.Id
                                 AND E.IdEstado = ES.Id
                                    AND E.IdEstado = C.IdEstado
                                    AND E.idCidade = C.Id
                                    AND E.IdBairro = B.Id
                                    AND E.idCidade = B.idCidade");

            sqlSelect.Append(" AND EN.Id = @id ");

            SqlCommand cmd = new SqlCommand(sqlSelect.ToString(), _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@id", id));

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return new EmpresaView
                    {
                        IdEmpresa = Convert.ToInt32(reader["Id"].ToString()),
                        NomeFantasia = reader["NOMEFANTASIA"].ToString(),
                        CNPJ = reader["CNPJ"].ToString(),
                        Endereco = new EnderecoRepository(_conn, _trans).PopularObjetoEnderecoView(reader)
                    };
                }
            }

            return new EmpresaView();
        }

        public List<EmpresaView> GetAll()
        {
            List<EmpresaView> empresas = new List<EmpresaView>();

            string sqlSelect = @"SELECT EN.Id
                                      ,EN.NomeFantasia
                                      ,EN.IdEndereco
                                      ,EN.CNPJ
                                      ,E.Id
                                      ,E.Cep ,
                                      E.IdEstado ,
                                      ES.Uf ,
                                      ES.Nome AS NomeEstado ,
                                      E.IdCidade ,
                                      C.Nome AS NomeCidade ,
                                      E.IdBairro ,
                                      B.Nome AS NomeBairro ,
                                      E.Logradouro ,
                                      E.Complemento
                                FROM dbo.Empresa EN,
                                     dbo.Endereco E,
                                     dbo.Estado ES,
                                     dbo.Cidade C,
                                     dbo.Bairro B
                                WHERE EN.idEndereco = E.Id
                                 AND E.IdEstado = ES.Id
                                    AND E.IdEstado = C.IdEstado
                                    AND E.idCidade = C.Id
                                    AND E.IdBairro = B.Id
                                    AND E.idCidade = B.idCidade

                                ORDER BY EN.Id";

            SqlCommand cmd = new SqlCommand(sqlSelect, _conn);
            cmd.Transaction = _trans;

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    empresas.Add(new EmpresaView
                    {
                        IdEmpresa = Convert.ToInt32(reader["Id"].ToString()),
                        NomeFantasia = reader["NOMEFANTASIA"].ToString(),
                        CNPJ = reader["CNPJ"].ToString(),
                        Endereco = new EnderecoRepository(_conn, _trans).PopularObjetoEnderecoView(reader)
                    });
                }
            }

            return empresas;
        }

        public string GetEstadoEmpresa(int id)
        {
            string sqlSelect = @"SELECT  ES.Uf 
                                    FROM dbo.Empresa EN,
                                         dbo.Endereco E,
                                         dbo.Estado ES                                     
                                WHERE EN.idEndereco = E.Id
                                 AND E.IdEstado = ES.Id 
                                 AND EN.ID = @id";

            SqlCommand cmd = new SqlCommand(sqlSelect.ToString(), _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@id", id));
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return reader["UF"].ToString();
                }
            }
            return string.Empty;
        }

        public void Remove(int id)
        {
            string sqlDelete = @"DELETE FROM dbo.Empresa
                                WHERE Id =@id ";

            SqlCommand cmd = new SqlCommand(sqlDelete.ToString(), _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@id", id));

            cmd.ExecuteNonQuery();
        }

        public void Update(Empresa obj)
        {
            string sqlUpdate = @"UPDATE dbo.Empresa
                                    SET NOMEFANTASIA =@nome
                                        ,CNPJ=@cnpj
                                        ,IDENDERECO =@endereco
                                    WHERE Id =@id ";

            SqlCommand cmd = new SqlCommand(sqlUpdate, _conn);

            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@nome", obj.NomeFantasia));
            cmd.Parameters.Add(new SqlParameter("@cnpj", obj.CNPJ));
            cmd.Parameters.Add(new SqlParameter("@endereco", obj.IdEndereco));
            cmd.Parameters.Add(new SqlParameter("@id", obj.Id));

            cmd.ExecuteNonQuery();
        }
    }
}
