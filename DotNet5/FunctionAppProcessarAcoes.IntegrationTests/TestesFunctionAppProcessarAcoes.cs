using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Xunit;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using FluentAssertions;
using Serilog;
using Serilog.Core;
using FunctionAppProcessarAcoes.IntegrationTests.Models;

namespace FunctionAppProcessarAcoes.IntegrationTests
{
    public class TestesFunctionAppProcessarAcoes
    {
        private const string COD_CORRETORA = "00000";
        private const string NOME_CORRETORA = "Corretora Testes";
        private static IConfiguration Configuration { get; }
        private static Logger Logger { get; }

        static TestesFunctionAppProcessarAcoes()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.json")
                .AddEnvironmentVariables().Build();

            Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        [Theory]
        [InlineData("ABCD", 100.98)]
        [InlineData("EFGH", 200.9)]
        [InlineData("IJKL", 1_400.978)]
        public void TestarFunctionApp(string codigo, double valor)
        {
            var queueName = Configuration["AzureStorage:Queue"];
            Logger.Information($"Tópico: {queueName}");

            var cotacaoAcao = new Acao()
            {
                Codigo = codigo,
                Valor = valor,
                CodCorretora = COD_CORRETORA,
                NomeCorretora = NOME_CORRETORA
            };
            var conteudoAcao = JsonSerializer.Serialize(cotacaoAcao);
            Logger.Information($"Dados: {conteudoAcao}");

            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(
                    Configuration["AzureStorage:ConnectionString"]);
            CloudQueueClient queueClient =
                storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(queueName);

            if (queue.CreateIfNotExists())
                Logger.Information($"Criada a fila {queueName} no Azure Storage");

            queue.AddMessage(new CloudQueueMessage(conteudoAcao));
            Logger.Information(
               $"Azure Storage Queue - Envio para a fila {queueName} concluído | " +
                conteudoAcao);

            Logger.Information("Aguardando o processamento da Function App...");
            Thread.Sleep(
                Convert.ToInt32(Configuration["IntervaloProcessamento"]));

            var httpClient = new HttpClient();
            var dadosAcoes = httpClient.GetFromJsonAsync<Acao[]>(
                String.Format(Configuration["UrlResultadoAcoes"], codigo)).Result;

            var resultadoAcao = dadosAcoes.SingleOrDefault();

            resultadoAcao.Should().NotBeNull();
            resultadoAcao.Codigo.Should().Be(codigo);
            resultadoAcao.Valor.Should().Be(valor);
            resultadoAcao.CodCorretora.Should().Be(COD_CORRETORA);
            resultadoAcao.NomeCorretora.Should().Be(NOME_CORRETORA);
        }
    }
}