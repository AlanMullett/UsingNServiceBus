namespace AssemblyWithMultipleEndpoints.EndpointConfiguration
{
    using NServiceBus;

    public class FirstHandlerInSagaEndpoint : IConfigureThisEndpoint
    {
        public static string EndpointName = "BusinessActivity";
        public void Customize(BusConfiguration configuration)
        {
            configuration.EndpointName(EndpointName);
        }
    }
}
