using Fornecedores.Entities;
using Fornecedores.Entities.ViewModels;
using Fornecedores.IRepositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Fornecedores.Repositories
{
    public class FornecedorRepository : IFornecedorRepository
    {
        private readonly SqlConnection _conn;
        private readonly SqlTransaction _trans;

        public FornecedorRepository(SqlConnection conn, SqlTransaction trans)
        {
            this._conn = conn;
            this._trans = trans;
        }
        public void Add(Fornecedor obj)
        {
            string sqlInsert = @"INSERT INTO dbo.Fornecedor
                                    (IDPESSOA ,IDEMPRESA ,DATACADASTRO)
                                values
                                    (@idPessoa ,@idEmpresa,@datacadastro)";

            SqlCommand cmd = new SqlCommand(sqlInsert, _conn);
            cmd.Transaction = _trans;

            cmd.Parameters.Add(new SqlParameter("@idPessoa", obj.IdPessoa));
            cmd.Parameters.Add(new SqlParameter("@idEmpresa", obj.IdEmpresa));
            cmd.Parameters.Add(new SqlParameter("@datacadastro", obj.DataCadastro));

            cmd.ExecuteNonQuery();
        }

        public Fornecedor Get(int id, int idPessoa, int idEmpresa)
        {
            StringBuilder sqlSelect = new StringBuilder();

            sqlSelect.Append(@"SELECT Id,  IdEmpresa , idPessoa, DataCadastro
                                    FROM dbo.Fornecedor 
                                       WHERE 1=1");

            if (idPessoa == 0 && idEmpresa == 0)
                sqlSelect.Append(" AND Id = @id");
            if (idPessoa > 0)
                sqlSelect.Append(" AND IdPessoa = @pessoa");
            if (idEmpresa > 0)
                sqlSelect.Append(" AND IdEmpresa = @empresa");

            SqlCommand cmd = new SqlCommand(sqlSelect.ToString(), _conn);

            if (idPessoa == 0 && idEmpresa == 0)
                cmd.Parameters.Add(new SqlParameter("@id", id));
            if (idPessoa > 0)
                cmd.Parameters.Add(new SqlParameter("@pessoa", idPessoa));
            if (idEmpresa > 0)
                cmd.Parameters.Add(new SqlParameter("@empresa", idEmpresa));

            cmd.Transaction = _trans;
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    return new Fornecedor
                    {
                        Id = Convert.ToInt32(reader["ID"].ToString()),
                        IdPessoa = Convert.ToInt32(reader["IDPESSOA"].ToString()),
                        IdEmpresa = Convert.ToInt32(reader["IDEMPRESA"].ToString()),
                        DataCadastro = reader["DATACADASTRO"] == DBNull.Value ? new DateTime(9999, 01, 01) : Convert.ToDateTime(reader["DATACADASTRO"]),

                    };
                }
            }

            return new Fornecedor();
        }

        public Fornecedor Get(int id)
        {
            return this.Get(id, default(int), default(int));
        }

        public List<FornecedorView> GetAll(string nome, string CPF, string CNPJ, DateTime dataCadastro)
        {
            List<FornecedorView> listaFornecedores = new List<FornecedorView>();
            StringBuilder sqlSelect = new StringBuilder();

            #region SELECT Dos Fornecedores com Os seus Dados
            sqlSelect.Append(@"SELECT 
                                           F.ID,
                                           F.DataCadastro ,
                                           F.IdEmpresa,
                                           E.NomeFantasia,
                                           E.CNPJ AS CnpjEmpresa,
                                           E.id Endereco,
                                           EN.Cep,
                                           EN.IdEstado,
                                           ES.Uf,
                                           ES.Nome AS NomeEstado,
                                           EN.idCidade,
                                           CDD.Nome AS NomeCidade,
                                           EN.IdBairro,
                                           B.Nome AS NomeBairro,
                                           EN.Logradouro,
                                           EN.Complemento,
                                          ISNULL(PF.Id,0) IdPesFisica ,
                                           P.Id idPessoa,
                                           P.Nome AS NomeFornecedor,
                                           P.Sobrenome,                                      
                                           CASE   
											  WHEN P.Tipo ='F' THEN 'Físico' 
											  WHEN P.Tipo ='J' THEN 'Jurídico' 
										   END  Tipo,
                                          ISNULL(PF.Cpf,'') as Cpf,
                                          ISNULL(PF.DataNascimento,'') DataNascimento,
                                          ISNULL( PF.Rg,'') Rg,
                                          ISNULL(J.Cnpj,'') Cnpj,
                                          ISNULL(J.Id,0) IdPesJuridica 
                                                                                    
                                    FROM dbo.Fornecedor F,                                         
										  dbo.Empresa E,
                                          dbo.Endereco EN,
                                          dbo.Estado Es,
                                          dbo.Bairro B,
                                          dbo.Cidade CDD,
                                          dbo.Pessoa P
                                    LEFT JOIN dbo.PesFisica PF ON PF.IdPessoa = P.Id
                                    LEFT JOIN dbo.PesJuridica J ON J.IdPessoa = P.Id                                                              
                                                                  
                                    WHERE F.IdPessoa = P.Id
                                      AND F.IdEmpresa = E.Id                                     
                                      AND E.IdEndereco = EN.Id
                                      AND EN.IdEstado = Es.Id                                 
                                      AND EN.idCidade = CDD.Id
                                      AND EN.IdBairro = B.Id
                                      AND CDD.IdEstado = Es.Id
                                      AND CDD.Id = B.IdCidade ");

            if (!string.IsNullOrWhiteSpace(nome))
                sqlSelect.Append(" AND P.NOME = @nome");
            if (!string.IsNullOrWhiteSpace(CPF))
                sqlSelect.Append(" AND PF.CPF = @cpf");
            if (!string.IsNullOrWhiteSpace(CNPJ))
                sqlSelect.Append(" AND E.CNPJ = @cnpj");
            if (dataCadastro.ToString() != "01/01/0001 00:00:00")
                sqlSelect.Append(" AND F.DATACADASTRO = @data");

            #endregion

            SqlCommand cmd = new SqlCommand(sqlSelect.ToString(), _conn);
            cmd.Transaction = _trans;
            if (!string.IsNullOrWhiteSpace(nome))
                cmd.Parameters.Add(new SqlParameter("@nome", nome));
            if (!string.IsNullOrWhiteSpace(CPF))
                cmd.Parameters.Add(new SqlParameter("@cpf", CPF));
            if (!string.IsNullOrWhiteSpace(CNPJ))
                cmd.Parameters.Add(new SqlParameter("@cnpj", CNPJ));
            if (dataCadastro.ToString() != "01/01/0001 00:00:00")
                cmd.Parameters.Add(new SqlParameter("@data", dataCadastro));
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    FornecedorView fornecedores = new FornecedorView
                    {
                        NomeFantasia = reader["NomeFantasia"].ToString().Trim(),
                        CnpjEmpresa = reader["CnpjEmpresa"].ToString().Trim(),
                        IdEndereco = Convert.ToInt32(reader["Endereco"].ToString().Trim()),
                        Cep = reader["Cep"].ToString().Trim(),
                        IdEstado = Convert.ToInt32(reader["IdEstado"].ToString()),
                        Uf = reader["UF"].ToString().Trim(),
                        NomeEstado = reader["NomeEstado"].ToString().Trim(),
                        IdCidade = Convert.ToInt32(reader["IDCIDADE"].ToString()),
                        NomeCidade = reader["NomeCidade"].ToString().Trim(),
                        IdBairro = Convert.ToInt32(reader["IdBairro"].ToString()),
                        NomeBairro = reader["NomeBairro"].ToString().Trim(),
                        Complemento = reader["Complemento"].ToString().Trim(),
                        Logradouro = reader["Logradouro"].ToString().Trim(),
                        IdPesFisica = Convert.ToInt32(reader["IDPESFISICA"].ToString()),
                        IdPessoa = Convert.ToInt32(reader["IDPESSOA"].ToString()),
                        NomeFornecedor = reader["NomeFornecedor"].ToString().Trim(),
                        Sobrenome = reader["SOBRENOME"].ToString().Trim(),
                        Tipo = reader["Tipo"].ToString().Trim(),
                        Cpf = reader["Cpf"].ToString().Trim(),
                        DataNascimento = reader["DataNascimento"] == DBNull.Value ? new DateTime(9999, 01, 01) : Convert.ToDateTime(reader["DataNascimento"]),
                        Rg = reader["RG"].ToString().Trim(),
                        Cnpj = reader["CNPJ"].ToString().Trim(),
                        IdPesJuridica = Convert.ToInt32(reader["IDPESJURIDICA"].ToString()),
                        DataCadastro = reader["DataCadastro"] == DBNull.Value ? new DateTime(9999, 01, 01) : Convert.ToDateTime(reader["DataCadastro"]),
                        IdFornecedor = Convert.ToInt32(reader["Id"].ToString()),
                        IdEmpresa = Convert.ToInt32(reader["IdEmpresa"].ToString()),
                    };

                    listaFornecedores.Add(fornecedores);
                }
            }

            return listaFornecedores;
        }
        public void Remove(int id)
        {
            string sqlDelete = @"DELETE FROM dbo.Fornecedor
                                        WHERE Id =@id ";

            SqlCommand cmd = new SqlCommand(sqlDelete, _conn);
            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("@id", id));

            cmd.ExecuteNonQuery();
        }

        public void Update(Fornecedor obj)
        {
            string sqlUpdate = @"UPDATE dbo.Fornecedor
                                      SET IDPESSOA =@idPessoa
                                         ,IDEMPRESA =@idEmpresa
                                         ,DATACADASTRO =@datacadastro 
                                    WHERE Id =@id ";

            SqlCommand cmd = new SqlCommand(sqlUpdate, _conn);

            cmd.Transaction = _trans;
            cmd.Parameters.Add(new SqlParameter("id", obj.Id));
            cmd.Parameters.Add(new SqlParameter("@idPessoa", obj.IdPessoa));
            cmd.Parameters.Add(new SqlParameter("@idEmpresa", obj.IdEmpresa));
            cmd.Parameters.Add(new SqlParameter("@datacadastro", obj.DataCadastro));
            cmd.ExecuteNonQuery();
        }
    }
}
