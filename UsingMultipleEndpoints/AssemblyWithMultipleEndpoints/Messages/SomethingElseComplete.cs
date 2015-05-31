namespace AssemblyWithMultipleEndpoints.Messages
{
    using NServiceBus;

    public class SomethingElseComplete : IMessage
    {
        public string ProcessId { get; set; }
    }
}