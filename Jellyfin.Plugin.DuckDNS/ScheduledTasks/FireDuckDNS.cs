using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Model.Tasks;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;
using System.Collections;

namespace Jellyfin.Plugin.DuckDNS
{
    public class FireRetentionTask : IScheduledTask
    {
        private readonly ILogger<FireRetentionTask> _logger;
		private readonly WebClient cli = new WebClient();

        public FireRetentionTask(ILogger<FireRetentionTask> logger)
        {
            _logger = logger;
        }

        public string Key => "DuckDNSScheduledTask";

        public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
        {
			// update every 5 min
            return new[] {
                new TaskTriggerInfo
                {
                    Type = TaskTriggerInfo.TriggerInterval,
                    IntervalTicks = TimeSpan.FromMinutes(5).Ticks
                }
            };
        }

        public Task Execute(CancellationToken cancellationToken, IProgress<double> progress)
        {
			// read config			
			string Addresses = Plugin.Instance.Configuration.Addresses;
			string Token = Plugin.Instance.Configuration.Token;
			bool Active = Plugin.Instance.Configuration.Active;
			
			if(Active && Addresses.Length > 0 && Token.Length > 0)
			{
				string[] Domains = Addresses.Split(";");
				
				foreach(string Domain in Domains)
				{
					// generate update url
					string updateURLv4 = "https://www.duckdns.org/update?domains=" + Domain + "&token=" + Token + "&ip=";
					string resultIPv4 = cli.DownloadString(updateURLv4);
					
					_logger.LogInformation("DuckDNS ipv4 update for '" + Domain + "' fired = " + resultIPv4);
				}
				
				_logger.LogInformation("DuckDNS update finished!");
			}
			
            return Task.CompletedTask;
        }

        public string Name => "Update DuckDNS-Record";
        public string Category => "DuckDNS";
        public string Description => "";
    }
}