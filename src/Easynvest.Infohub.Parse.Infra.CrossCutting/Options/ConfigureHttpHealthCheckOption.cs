using System;

namespace Easynvest.Infohub.Parse.Infra.CrossCutting.Options
{
    public class ConfigureHttpHealthCheckOption
    {
        public int Retries { get; set; }
        public int DelayBetweenRetriesFromMilliseconds { get; set; }
        public int TimeoutPerRequesttrarFromMilliseconds { get; set; }
        public TimeSpan DelayBetweenRetries => TimeSpan.FromMilliseconds(DelayBetweenRetriesFromMilliseconds);
        public TimeSpan TimeoutPerRequest => TimeSpan.FromMilliseconds(TimeoutPerRequesttrarFromMilliseconds);
    }
}
