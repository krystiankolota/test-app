using Microsoft.Extensions.Options;
using RandomDataFetcher.Application.Common.Interfaces.Infrastructure;
using RandomDataFetcher.Infrastructure.Settings;
namespace RandomDataFetcher.Infrastructure.BlobStorage
{
    public sealed class RandomLogsPayloadPayloadStorage : BaseBlobStorageService, IRandomLogsPayloadStorage
    {
        private readonly PayloadStorageSettings _payloadStorageSettings;

        public RandomLogsPayloadPayloadStorage(IOptions<PayloadStorageSettings> options)
            : base(options.Value.ConnectionString)
        {
            _payloadStorageSettings = options.Value;
        }

        public async Task<string> GetAsync(string url)
        {
            var payload = await GetAsync(new Uri(url));
            return payload.ToString();
        }

        public async Task<string> GetAsync(Guid id)
        {
            var payload = await GetAsync(_payloadStorageSettings.ContainerName, id.ToString());
            return payload.ToString();
        }

        public async Task<(Guid Id, string BlobUrl)> StoreAsync(string payloadJson)
        {
            var id = Guid.NewGuid();
            var blobUrl = await UpsertAsync(
                BinaryData.FromString(payloadJson),
                id.ToString(),
                _payloadStorageSettings.ContainerName);

            return ValueTuple.Create(id, blobUrl);
        }
    }
}
