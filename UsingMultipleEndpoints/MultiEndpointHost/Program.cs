namespace MultiEndpointHost
{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;
    using AssemblyWithMultipleEndpoints;
    using AssemblyWithMultipleEndpoints.EndpointConfiguration;
    using AssemblyWithMultipleEndpoints.Messages;
    using NServiceBus;

    class Program
    {
        private static void Main(string[] args)
        {
            using (var db = new SqlConnection())
            {
                db.ConnectionString = ConfigurationManager.ConnectionStrings["NServiceBus/Persistence"].ConnectionString;
                db.Open();
                var dbCmd = db.CreateCommand();
                dbCmd.CommandText = "IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='dbo' AND TABLE_NAME='SagaData')) BEGIN DROP TABLE SagaData END";
                dbCmd.ExecuteNonQuery();
            }

            RunOnce();
        }

        private static void RunOnce()
        {
            var cts = new CancellationTokenSource();
            
            Task[] tasks =
            {
                StartEndpoint(new FirstHandlerInSagaEndpoint(), cts.Token),
                StartEndpoint(new SecondHandlerInSagaEndpoint(), cts.Token),
                StartEndpoint(new DoingSomethingElseEndpoint(), cts.Token)
            };

            // let everything settle down
            Task.Delay(20000).Wait();

            // send a message that does something
            BusConfiguration config = new BusConfiguration();
            config.UseTransport<AzureServiceBusTransport>();
            using (ISendOnlyBus bus = Bus.CreateSendOnly(config))
            {
                bus.Send<RunBusinessActivity>(m => m.ProcessId = Guid.NewGuid().ToString());
            }

            // and then close the system down after another ten seconds
            cts.CancelAfter(10000);
            Task.WaitAll(tasks);
        }

        static Task StartEndpoint(IConfigureThisEndpoint endpoint, CancellationToken token)
        {
            return Task.Factory.StartNew(() =>
            {
                var config = new BusConfiguration();
                endpoint.Customize(config);

                config.ScaleOut().UseSingleBrokerQueue();
                config.UseTransport<AzureServiceBusTransport>();
                config.UsePersistence<NHibernatePersistence>();

                var bus = Bus.Create(config);
                using (bus.Start())
                {
                    token.WaitHandle.WaitOne();
                }
            });
        }
    }
}
