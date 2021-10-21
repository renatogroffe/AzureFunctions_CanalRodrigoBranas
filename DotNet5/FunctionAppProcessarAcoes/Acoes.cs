using System.Linq;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using FunctionAppProcessarAcoes.Data;

namespace FunctionAppProcessarAcoes
{
    public class Acoes
    {
        private readonly AcoesRepository _repository;

        public Acoes(AcoesRepository repository)
        {
            _repository = repository;
        }

        [Function("Acoes")]
        [OpenApiOperation(operationId: "Acoes", tags: new[] { "Acoes" }, Summary = "Acao", Description = "Consulta às cotações de ações.", Visibility = OpenApiVisibilityType.Important)]
        [OpenApiParameter(name: "codigo", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "Código da ação")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Acao[]), Summary = "Histórico de cotações de ações", Description = "Histórico de cotações de ações")]        
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
            FunctionContext executionContext, string codigo = null)
        {
            var logger = executionContext.GetLogger("Acoes");
            
            var historicoAcoes = _repository.GetAll(codigo);
            logger.LogInformation($"No. de registros encontrados: {historicoAcoes.Count()}");

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(historicoAcoes);
            return response;
        }
    }
}