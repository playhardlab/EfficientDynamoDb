using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.IO;

namespace EfficientDynamoDb.Internal.Operations.Shared
{
    public class DynamoDbContent : HttpContent
    {
        internal static readonly RecyclableMemoryStreamManager MemoryStreamManager = new RecyclableMemoryStreamManager();
        private static readonly JsonWriterOptions JsonWriterOptions = new JsonWriterOptions { SkipValidation = true };
        private readonly string body;
        private MemoryStream? _pooledContentStream;

        
        public DynamoDbContent(string amzTarget, string body)
        {
            Headers.Add("X-AMZ-Target", amzTarget);
            Headers.ContentType = new MediaTypeHeaderValue("application/x-amz-json-1.0");
            this.body = body;
        }

        protected override async Task<Stream> CreateContentReadStreamAsync()
        {
            if (_pooledContentStream != null)
                return _pooledContentStream;

            _pooledContentStream = MemoryStreamManager.GetStream();

            await SerializeToStreamAsync(_pooledContentStream, null).ConfigureAwait(false);

            _pooledContentStream.Position = 0;
            return _pooledContentStream;

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
                _pooledContentStream?.Dispose();
        }


        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        {
            if (_pooledContentStream?.Length > 0)
            {
                await _pooledContentStream.CopyToAsync(stream).ConfigureAwait(false);
                return;
            }

            var buffer = Encoding.UTF8.GetBytes(body);
            await stream.WriteAsync(buffer,0, buffer.Length);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }

    }
}