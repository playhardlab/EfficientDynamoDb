using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using AWSSDK.Core.NetStandard.Amazon.Runtime.Pipeline.HttpHandler;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Benchmarks.AwsDdbSdk.Entities;

namespace Benchmarks.AwsDdbSdk.Benchmarks
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    public class QueryBenchmark
    {
        private DynamoDBContext _context;

        private const string KeysOnlyEntityPk = "keys_only_bench";
        private const string MediumEntityPk = "medium_bench";
        
        [GlobalSetup]
        public async Task SetupAsync()
        {
            _context = GetContext();

            // await SetupKeysOnlyBenchAsync().ConfigureAwait(false);
            await SetupMediumBenchAsync().ConfigureAwait(false);
        }
        
        // [Benchmark]
        public async Task<int> KeysOnlyBenchmarkAsync()
        {
            var entities = await _context.QueryAsync<KeysOnlyEntity>(KeysOnlyEntityPk).GetRemainingAsync().ConfigureAwait(false);

            return entities.Count;
        }
        
        [Benchmark]
        public async Task<int> MediumBenchmarkAsync()
        {
            var entities = await _context.QueryAsync<MediumStringFieldsEntity>(MediumEntityPk).GetRemainingAsync().ConfigureAwait(false);

            return entities.Count;
        }

        private async Task SetupKeysOnlyBenchAsync()
        {
            const int desiredEntitiesCount = 1000;
            HttpHandlerConfig.IsCacheEnabled = true;
            var entities = await _context.QueryAsync<KeysOnlyEntity>(KeysOnlyEntityPk).GetRemainingAsync().ConfigureAwait(false);
            if (entities.Count >= desiredEntitiesCount)
                return;

            HttpHandlerConfig.IsCacheEnabled = false;
            await Task.WhenAll(Enumerable.Range(0, desiredEntitiesCount)
                    .Select(i => _context.SaveAsync(new KeysOnlyEntity {Pk = KeysOnlyEntityPk, Sk = $"sk_{i:0000}"})))
                .ConfigureAwait(false);

            HttpHandlerConfig.IsCacheEnabled = true;
            await _context.QueryAsync<KeysOnlyEntity>(KeysOnlyEntityPk).GetRemainingAsync().ConfigureAwait(false);
        }
        
        private async Task SetupMediumBenchAsync()
        {
            const int desiredEntitiesCount = 1000;
            HttpHandlerConfig.IsCacheEnabled = true;
            var entities = await _context.QueryAsync<MediumStringFieldsEntity>(MediumEntityPk).GetRemainingAsync().ConfigureAwait(false);
            if (entities.Count >= desiredEntitiesCount)
                return;

            HttpHandlerConfig.IsCacheEnabled = false;
            await Task.WhenAll(Enumerable.Range(0, desiredEntitiesCount)
                    .Select(i => _context.SaveAsync(new MediumStringFieldsEntity {Pk = MediumEntityPk, Sk = $"sk_{i:0000}"})))
                .ConfigureAwait(false);

            HttpHandlerConfig.IsCacheEnabled = true;
            await _context.QueryAsync<MediumStringFieldsEntity>(MediumEntityPk).GetRemainingAsync().ConfigureAwait(false);
        }

        private DynamoDBContext GetContext()
        {
            var ddbConfig = new AmazonDynamoDBConfig {RegionEndpoint = RegionEndpoint.USEast1};
            var dbClient = new AmazonDynamoDBClient(
                new BasicAWSCredentials(Environment.GetEnvironmentVariable("DEV_AWS_PUBLIC_KEY"), Environment.GetEnvironmentVariable("DEV_AWS_PRIVATE_KEY")),
                ddbConfig);

            var contextConfig = new DynamoDBContextConfig
            {
                TableNamePrefix = "production_",
                Conversion = DynamoDBEntryConversion.V2
            };

            return new DynamoDBContext(dbClient, contextConfig);
        }
    }
}