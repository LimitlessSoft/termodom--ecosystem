namespace TD.Core.Contracts.Tasks
{
    public class Task
    {
        public TimeSpan Timeout { get; set; }
        public Action Action { get; set; }

        public Task(Action action)
        {
            Action = action;
            Timeout = TimeSpan.Zero;
        }

        public Task(Action action, TimeSpan timeout)
        {
            Timeout = timeout;
            Action = action;
        }
    }
}
