using Autofac;
using AutoMapper;
using Microsoft.IO;
using Miningcore.Blockchain.Bitcoin;
using Miningcore.Blockchain.Bitcoin.Configuration;
using Miningcore.Configuration;
using Miningcore.Messaging;
using Miningcore.Mining;
using Miningcore.Nicehash;
using Miningcore.Persistence;
using Miningcore.Persistence.Repositories;
using Miningcore.Time;
using Newtonsoft.Json;

namespace Miningcore.Blockchain.Decred;

[CoinFamily(CoinFamily.Decred)]
public class DecredPool : BitcoinPool
{
    public DecredPool(IComponentContext ctx,
        JsonSerializerSettings serializerSettings,
        IConnectionFactory cf,
        IStatsRepository statsRepo,
        IMapper mapper,
        IMasterClock clock,
        IMessageBus messageBus,
        RecyclableMemoryStreamManager rmsm,
        NicehashService nicehashService) :
        base(ctx, serializerSettings, cf, statsRepo, mapper, clock, messageBus, rmsm, nicehashService)
    {
    }

    protected override BitcoinJobManager CreateJobManager()
    {
        return ctx.Resolve<DecredJobManager>(
            new TypedParameter(typeof(IExtraNonceProvider), new BitcoinExtraNonceProvider(poolConfig.Id, clusterConfig.InstanceId)));
    }

    public override double HashrateFromShares(double shares, double interval)
    {
        // Base implementation uses share difficulty as stored (already scaled down by share multiplier).
        // Multiply back by the Decred share multiplier so reported hashrate matches miner-reported rates.
        var baseRate = base.HashrateFromShares(shares, interval);
        var shareMultiplier = poolConfig.Template.As<BitcoinTemplate>()?.ShareMultiplier ?? 1d;

        return baseRate * shareMultiplier;
    }
}
