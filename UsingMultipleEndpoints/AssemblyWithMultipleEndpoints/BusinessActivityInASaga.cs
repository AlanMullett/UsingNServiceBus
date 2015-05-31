namespace AssemblyWithMultipleEndpoints
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Messages;
    using NServiceBus;
    using NServiceBus.Saga;

    public class BusinessActivityInASaga : Saga<SagaData>,
        IAmStartedByMessages<RunBusinessActivity>,
        IHandleMessages<SomethingElseComplete>
    {
        public void Handle(RunBusinessActivity message)
        {
            Debug.WriteLine("Handling the RunBusinessActivity message");

            Data.ProcessId = message.ProcessId;

            Bus.Send<DoSomethingElse>(m => m.ProcessId = message.ProcessId);
        }

        public void Handle(SomethingElseComplete message)
        {
            Debug.WriteLine("Handling the SomethingElseComplete message");
            
            MarkAsComplete();
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<SagaData> mapper)
        {
            mapper.ConfigureMapping<SomethingElseComplete>(m => m.ProcessId).ToSaga(s => s.ProcessId);
        }
    }
}
