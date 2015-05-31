namespace AssemblyWithMultipleEndpoints
{
    using NServiceBus.Saga;

    public class SagaData : ContainSagaData 
    {
        public virtual string ProcessId { get; set; }
    }
}
