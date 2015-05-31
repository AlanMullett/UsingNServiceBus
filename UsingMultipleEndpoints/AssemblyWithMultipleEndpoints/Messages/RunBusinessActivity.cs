namespace AssemblyWithMultipleEndpoints.Messages
{
    using NServiceBus;

    public class RunBusinessActivity :  ICommand
    {
        public string ProcessId { get; set; }
    }
}
