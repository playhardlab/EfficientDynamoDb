using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Configs.Http;
using EfficientDynamoDb.Internal.Operations.Shared;
using EfficientDynamoDb.Internal.Signing;
using Microsoft.IO;

namespace EfficientDynamoDb.Internal
{
    public class DynamoDbNativeClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DynamoDbContextConfig config;

        public DynamoDbNativeClient(DynamoDbContextConfig config)
        {
            _httpClientFactory = new DefaultHttpClientFactory();
            this.config = config;
        }


        public ValueTask<HttpResponseMessage> SendAsync(string target, object body, CancellationToken cancellationToken = default)
            => SendAsync(target, JsonSerializer.Serialize(body), cancellationToken);

        public async ValueTask<HttpResponseMessage> SendAsync(string target, string body, CancellationToken cancellationToken = default)
        {
            var httpContent = new DynamoDbContent(target, body);

            using var request = new HttpRequestMessage(HttpMethod.Post, config.RegionEndpoint.RequestUri)
            {
                Content = httpContent
            };

            try
            {
                var httpClient = _httpClientFactory.CreateHttpClient();
                var stream = await httpContent.ReadAsStreamAsync().ConfigureAwait(false);
                var credentials = await config.CredentialsProvider.GetCredentialsAsync(cancellationToken).ConfigureAwait(false);

                var metadata = new SigningMetadata(config.RegionEndpoint, credentials, DateTime.UtcNow, httpClient.DefaultRequestHeaders,
                    httpClient.BaseAddress);
                AwsRequestSigner.Sign(request, (RecyclableMemoryStream)stream, in metadata);

                var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                    await ErrorHandler.ProcessErrorAsync(config.Metadata, response, cancellationToken).ConfigureAwait(false);

                return response;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                request.Content = null;
            }
        }
    }
}