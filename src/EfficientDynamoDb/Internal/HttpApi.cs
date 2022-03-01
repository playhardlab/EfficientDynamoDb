using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using EfficientDynamoDb.Configs.Http;
using EfficientDynamoDb.Exceptions;
using EfficientDynamoDb.Internal.JsonConverters;
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

    public class HttpApi
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpApi()
        {
            _httpClientFactory = new DefaultHttpClientFactory();
        }
        public HttpApi(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async ValueTask<HttpResponseMessage> SendAsync(DynamoDbContextConfig config, HttpContent httpContent, CancellationToken cancellationToken = default)
        {
            try
            {
                int internalServerErrorRetries = 0,
                    limitExceededRetries = 0,
                    provisionedThroughputExceededRetries = 0,
                    requestLimitExceededRetries = 0,
                    serviceUnavailableRetries = 0,
                    throttlingRetries = 0;
                while (true)
                {
                    TimeSpan delay;
                    try
                    {
                        using var request = new HttpRequestMessage(HttpMethod.Post, config.RegionEndpoint.RequestUri)
                        {
                            Content = httpContent
                        };
                        //var body = await request.Content.ReadAsStringAsync();
                        try
                        {
                            var httpClient = _httpClientFactory.CreateHttpClient();
                            var stream = await httpContent.ReadAsStreamAsync().ConfigureAwait(false);
                            var credentials = await config.CredentialsProvider.GetCredentialsAsync(cancellationToken).ConfigureAwait(false);
                            
                            var metadata = new SigningMetadata(config.RegionEndpoint, credentials, DateTime.UtcNow, httpClient.DefaultRequestHeaders,
                                httpClient.BaseAddress);
                            AwsRequestSigner.Sign(request, (RecyclableMemoryStream) stream, in metadata);

                            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

                            if (!response.IsSuccessStatusCode)
                                await ErrorHandler.ProcessErrorAsync(config.Metadata, response, cancellationToken).ConfigureAwait(false);

                            return response;
                        }
                        finally
                        {
                            request.Content = null;
                        }
                    }
                    catch (InternalServerErrorException)
                    {
                        if (!config.RetryStrategies.InternalServerErrorStrategy.TryGetRetryDelay(internalServerErrorRetries++, out delay))
                            throw;
                    }
                    catch (LimitExceededException)
                    {
                        if (!config.RetryStrategies.LimitExceededStrategy.TryGetRetryDelay(limitExceededRetries++, out delay))
                            throw;
                    }
                    catch (ProvisionedThroughputExceededException)
                    {
                        if (!config.RetryStrategies.ProvisionedThroughputExceededStrategy.TryGetRetryDelay(provisionedThroughputExceededRetries++, out delay))
                            throw;
                    }
                    catch (RequestLimitExceededException)
                    {
                        if (!config.RetryStrategies.RequestLimitExceededStrategy.TryGetRetryDelay(requestLimitExceededRetries++, out delay))
                            throw;
                    }
                    catch (ServiceUnavailableException)
                    {
                        if (!config.RetryStrategies.ServiceUnavailableStrategy.TryGetRetryDelay(serviceUnavailableRetries++, out delay))
                            throw;
                    }
                    catch (ThrottlingException)
                    {
                        if (!config.RetryStrategies.ThrottlingStrategy.TryGetRetryDelay(throttlingRetries++, out delay))
                            throw;
                    }

                    await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
                }
            }
            finally
            {
                httpContent.Dispose();
            }
        }

        public async ValueTask<TResponse> SendAsync<TResponse>(DynamoDbContextConfig config, HttpContent httpContent, CancellationToken cancellationToken = default)
        {
            using var response = await SendAsync(config, httpContent, cancellationToken).ConfigureAwait(false);

            await using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            return (await JsonSerializer.DeserializeAsync<TResponse>(responseStream,
                new JsonSerializerOptions {Converters = {new DdbEnumJsonConverterFactory(), new UnixDateTimeJsonConverter()}}, cancellationToken).ConfigureAwait(false))!;
        }
    }
}