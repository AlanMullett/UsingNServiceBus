namespace AssemblyWithMultipleEndpoints.EndpointConfiguration
{
    using NServiceBus;

    public class SecondHandlerInSagaEndpoint : IConfigureThisEndpoint
    {
        public static string EndpointName = "BusinessActivityStep2";

        public void Customize(BusConfiguration configuration)
        {
            configuration.EndpointName(EndpointName);
        }
    }
}
