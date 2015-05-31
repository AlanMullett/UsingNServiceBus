namespace AssemblyWithMultipleEndpoints.EndpointConfiguration
{
    using NServiceBus;

    public class DoingSomethingElseEndpoint : IConfigureThisEndpoint
    {
        public static string EndpointName = "DoSomethingElse";

        public void Customize(BusConfiguration configuration)
        {
            configuration.EndpointName(EndpointName);
        }
    }
}
