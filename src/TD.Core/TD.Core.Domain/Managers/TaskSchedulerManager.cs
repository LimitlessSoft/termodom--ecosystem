using TD.Core.Contracts.IManagers;
using TD.Core.Contracts.Tasks;

namespace TD.Core.Domain.Managers
{
    /// <summary>
    /// Manage tasks inside task scheduler.
    /// All task inside scheduler are running/idle based on scheduler state.
    /// Newly added tasks inherit state.
    /// </summary>
    public class TaskSchedulerManager : ITaskSchedulerManager
    {
        private List<System.Threading.Tasks.Task> _asyncTasks { get; set; } = new List<System.Threading.Tasks.Task>();
        private List<Core.Contracts.Tasks.Task> _tasks { get; set; } = new List<Core.Contracts.Tasks.Task>();
        private TaskSchedulerState _state { get; set; } = TaskSchedulerState.Idle;
        private CancellationTokenSource _cancellationTokenSource { get; set; }
        private CancellationToken _cancellationToken { get; set; }
        public TaskSchedulerState State => _state;

        /// <summary>
        /// Adds task to the task scheduler.
        /// </summary>
        /// <param name="task"></param>
        public void AddTask(Core.Contracts.Tasks.Task task)
        {
            _tasks.Add(task);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="runTasksAsync">If true tasks will run asynchonously each on separate thread with timeout between running.
        /// If false tasks will run synchonously one after another, waiting for pevious task to finish.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public System.Threading.Tasks.Task RunTasksAsync(bool runTasksAsync)
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                if (_state == TaskSchedulerState.Running)
                    throw new Exception("Task scheduler is already running! Stop it before starting it again!");

                _cancellationTokenSource = new CancellationTokenSource();
                _cancellationToken = _cancellationTokenSource.Token;

                _state = TaskSchedulerState.Running;

                if (runTasksAsync)
                    StartRunningTasksAsAsync();
                else
                    StartRunningTasksAsSync();
            });
        }

        private void StartRunningTasksAsAsync()
        {
            foreach (var task in _tasks)
            {
                _asyncTasks.Add(System.Threading.Tasks.Task.Run(() =>
                {
                    while (!_cancellationToken.IsCancellationRequested)
                    {
                        System.Threading.Tasks.Task.Run(task.Action, _cancellationToken).Wait();
                        if (_cancellationToken.IsCancellationRequested)
                            return;

                        Timeout(task.Timeout, _cancellationToken);
                    }
                }));
            }

            _ = WatchAsyncTasksAsync();
        }

        private void StartRunningTasksAsSync()
        {
            while (!_cancellationToken.IsCancellationRequested)
                foreach (var task in _tasks)
                    task.Action();
        }

        private System.Threading.Tasks.Task WatchAsyncTasksAsync()
        {
            return System.Threading.Tasks.Task.Run(() =>
            {
                bool clearStatus = true;

                foreach (var asyncTask in _asyncTasks)
                    if (asyncTask.Status == TaskStatus.Running)
                        clearStatus = false;

                if (clearStatus)
                    _state = TaskSchedulerState.Idle;

                Thread.Sleep(TimeSpan.FromSeconds(1));
            });
        }

        private void Timeout(TimeSpan timeout, CancellationToken cancelationToken)
        {
            for (int i = 0; i < timeout.TotalSeconds; i++)
            {
                Thread.Sleep(1000);
                if (cancelationToken.IsCancellationRequested)
                    return;
            }
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
