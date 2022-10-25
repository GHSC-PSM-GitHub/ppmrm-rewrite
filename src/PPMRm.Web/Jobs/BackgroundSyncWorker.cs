using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.BackgroundWorkers;
using Volo.Abp.Threading;

namespace PPMRm.Web.Jobs
{
    
    public class BackgroundSyncWorker : AsyncPeriodicBackgroundWorkerBase
    {
        public BackgroundSyncWorker(
                AbpAsyncTimer timer,
                IServiceScopeFactory serviceScopeFactory
            ) : base(
                timer,
                serviceScopeFactory)
        {
            Timer.Period = 24 * 60 * 60 * 1000; //1 day 
        }

        protected async override Task DoWorkAsync(
            PeriodicBackgroundWorkerContext workerContext)
        {
           

            //Resolve dependencies
            var backgroundJobService = workerContext
                .ServiceProvider
                .GetRequiredService<IBackgroundJobManager>();
            
            var utcDateTime = DateTime.UtcNow;
            var utcDate = new DateTime(utcDateTime.Year, utcDateTime.Month, utcDateTime.Day, 0, 0, 0, DateTimeKind.Utc);

            if(utcDateTime.Day == 1)
            {
                // Wait until 12AM CST for the last changeset

                var midnightCstTime = new DateTime(utcDateTime.Year, utcDateTime.Month, utcDateTime.Day, 7, 0, 0, DateTimeKind.Utc); // add 7 hours
                
                var delayTillMidnightCST = midnightCstTime > utcDateTime ? midnightCstTime.Subtract(utcDateTime) : TimeSpan.FromSeconds(0);
                
                Logger.LogInformation("Starting:");
                var lastPeriod = utcDateTime.Subtract(TimeSpan.FromDays(1));
                var lastPeriodId = Convert.ToInt32($"{lastPeriod.Year}{lastPeriod.ToString("MM")}");
                
                //await Task.Delay(10000);
                //await backgroundJobService.EnqueueAsync(new PPMRm.Jobs.OrderSyncJobArgs
                //{
                //    PeriodId = lastPeriodId
                //}, delay: delayTillMidnightCST);

                Logger.LogInformation("Completed: Syncing ...");
            }
        }
    }
}
