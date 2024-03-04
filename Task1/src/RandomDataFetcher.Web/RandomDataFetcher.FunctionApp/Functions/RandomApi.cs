using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using RandomDataFetcher.Application.Common.Interfaces.JsonSerialization;
using RandomDataFetcher.Application.Services.RandomLogs.Queries.GetLogQuery;
using RandomDataFetcher.Application.Services.RandomLogs.Queries.ListLogsQuery;
using RandomDataFetcher.Contracts;
using RandomDataFetcher.FunctionApp.Common.Constants;
namespace RandomDataFetcher.FunctionApp.Functions;

public class RandomApiFunctions
{
    private readonly ISender _sender;
    private readonly ISerializationService<string> _serializationService;
    private readonly ILogger<RandomApiFunctions> _logger;

    public RandomApiFunctions(
        ISender sender,
        ISerializationService<string> serializationService,
        ILogger<RandomApiFunctions> logger)
    {
        _sender = sender;
        _serializationService = serializationService;
        _logger = logger;
    }

    [FunctionName(FunctionNames.ListLogs)]
    public async Task<HttpResponseMessage> RunListLogs(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "logs")] HttpRequest request)
    {
        try
        {
            var requestDataString = await request.ReadAsStringAsync();

            if (string.IsNullOrEmpty(requestDataString))
            {
                return ReturnBadRequestResult("Request body can't be null or empty");
            }

            var deserializedRequest = _serializationService.Deserialize<ListLogEntriesRequest>(requestDataString);

            if (string.IsNullOrEmpty(requestDataString))
            {
                return ReturnBadRequestResult("Invalid request body");
            }

            var result = await _sender.Send(new ListLogsQuery(deserializedRequest.From, deserializedRequest.To));

            return ReturnOkResult(_serializationService.Serialize(result));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred during {FunctionName}", FunctionNames.ListLogs);
            return ReturnErrorResult();
        }
    }

    [FunctionName(FunctionNames.GetLog)]
    public async Task<HttpResponseMessage> RunGetLog(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "log")] HttpRequest request)
    {
        try
        {
            var requestDataString = await request.ReadAsStringAsync();

            if (string.IsNullOrEmpty(requestDataString))
            {
                return ReturnBadRequestResult("Request body can't be null or empty");
            }

            var deserializedRequest = _serializationService.Deserialize<GetLogPayloadRequest>(requestDataString);

            if (string.IsNullOrEmpty(requestDataString))
            {
                return ReturnBadRequestResult("Invalid request body");
            }

            if (!Guid.TryParse(deserializedRequest.Id, out var logPayloadId))
            {
                return ReturnBadRequestResult("Invalid id");
            }

            var result = await _sender.Send(new GetLogQuery(logPayloadId));

            return ReturnOkResult(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred during {FunctionName}", FunctionNames.ListLogs);
            return ReturnErrorResult();
        }
    }

    private static HttpResponseMessage ReturnOkResult(string data, string mediaType = "application/json")
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(data, Encoding.UTF8, mediaType)
        };
    }

    private static HttpResponseMessage ReturnBadRequestResult(string message)
    {
        return new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(message)
        };
    }

    private static HttpResponseMessage ReturnErrorResult(string message = "Internal server error occurred")
    {
        return new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent(message)
        };
    }
}
