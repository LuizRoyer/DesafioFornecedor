using Fornecedores.IRepositories;
using Fornecedores.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace Fornecedores.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private SqlConnection _conn;
        private SqlTransaction _trans;
        private IEstadoRepository _estadoRepository;
        private ICidadeRepository _cidadeRepository;
        private IBairroRepository _bairroRepository;
        private IPessoaRepository _pessoaRepository;
        private IPesJuridicaRepository _pesJuridicaRepository;
        private IPesFisicaRepository _pesFisicaRepository;
        private IEnderecoRepository _enderecoRepository;
        private IEmpresaRepository _empresaRepository;
        private IContatoRepository _contatoRepository;
        private IFornecedorRepository _fornecedorRepository;

        public UnitOfWork()
        {
            _conn = new SqlConnection(ConnectionSql());
            _conn.Open();
            _trans = _conn.BeginTransaction();
        }

        public IEstadoRepository EstadoRepository()
        {
            if (_estadoRepository == null)
                _estadoRepository = new EstadoRepository(_conn, _trans);
            return _estadoRepository;
        }
        public IEnderecoRepository EnderecoRepository()
        {
            if (_enderecoRepository == null)
                _enderecoRepository = new EnderecoRepository(_conn, _trans);
            return _enderecoRepository;
        }
        public ICidadeRepository CidadeRepository()
        {
            if (_cidadeRepository == null)
                _cidadeRepository = new CidadeRepository(_conn, _trans);
            return _cidadeRepository;
        }

        public IBairroRepository BairroRepository()
        {
            if (_bairroRepository == null)
                _bairroRepository = new BairroRepository(_conn, _trans);
            return _bairroRepository;
        }
        public IEmpresaRepository EmpresaRepository()
        {
            if (_empresaRepository == null)
                _empresaRepository = new EmpresaRepository(_conn, _trans);
            return _empresaRepository;
        }
        public IPessoaRepository PessoaRepository()
        {
            if (_pessoaRepository == null)
                _pessoaRepository = new PessoaRepository(_conn, _trans);
            return _pessoaRepository;
        }
        public IPesFisicaRepository PesFisicaRepository()
        {
            if (_pesFisicaRepository == null)
                _pesFisicaRepository = new PesFisicaRepository(_conn, _trans);
            return _pesFisicaRepository;
        }

        public IPesJuridicaRepository PesJuridicaRepository()
        {
            if (_pesJuridicaRepository == null)
                _pesJuridicaRepository = new PesJuridicaRepository(_conn, _trans);
            return _pesJuridicaRepository;
        }

        public IContatoRepository ContatoRepository()
        {
            if (_contatoRepository == null)
                _contatoRepository = new ContatoRepository(_conn, _trans);
            return _contatoRepository;
        }

        public IFornecedorRepository FornecedorRepository()
        {
            if (_fornecedorRepository == null)
                _fornecedorRepository = new FornecedorRepository(_conn, _trans);
            return _fornecedorRepository;
        }
        public void Commit()
        {
            _trans.Commit();
            _conn.Close();
        }
        public void Rollback()
        {
            try
            {
                _trans.Rollback();
                _conn.Close();
            }
            catch
            { }
        }
        /// <summary>
        /// Metodo para Obter a Conecção no Appsettings
        /// </summary>
        /// <returns></returns>
        private string ConnectionSql()
        {
            string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json")
                .Build();
            return configuration.GetConnectionString("DefaultConnection");
        }
    }
}
