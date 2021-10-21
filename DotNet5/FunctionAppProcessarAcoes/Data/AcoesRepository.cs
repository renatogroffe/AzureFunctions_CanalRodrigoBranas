using System;
using System.Collections.Generic;
using Dapper;
using Dapper.Contrib.Extensions;
using FunctionAppProcessarAcoes.Models;

namespace FunctionAppProcessarAcoes.Data
{
    public class AcoesRepository
    {
        public AcoesRepository()
        {
        }

        public void Save(DadosAcao dadosAcao)
        {
            using var conexao = DbConnectionFactory.Create();
            conexao.Insert<Acao>(new ()
            {
                Codigo = dadosAcao.Codigo,
                DataReferencia = DateTime.Now,
                Valor = dadosAcao.Valor,
                CodCorretora = dadosAcao.CodCorretora,
                // FIXME: Simulação de falha
                NomeCorretora = dadosAcao.CodCorretora
                //NomeCorretora = dadosAcao.NomeCorretora
            });
        }

        public IEnumerable<Acao> GetAll(string codigo)
        {
            using var conexao = DbConnectionFactory.Create();
            
            if (String.IsNullOrWhiteSpace(codigo))
                return conexao.Query<Acao>(
                    "SELECT * FROM Acoes ORDER BY Id DESC");
            else
                return conexao.Query<Acao>(
                    "SELECT * FROM Acoes " +
                    "WHERE (Codigo= @CodigoPesquisado) " +
                    "ORDER BY Id DESC",
                    new { CodigoPesquisado = codigo });
        }
    }
}