namespace AssemblyWithMultipleEndpoints.Messages
{
    using NServiceBus;

    public class DoSomethingElse : ICommand
    {
        public string ProcessId { get; set; }
    }
}