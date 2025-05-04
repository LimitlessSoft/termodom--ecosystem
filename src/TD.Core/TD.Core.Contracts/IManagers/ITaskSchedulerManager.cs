using TD.Core.Contracts.Tasks;

namespace TD.Core.Contracts.IManagers
{
	public interface ITaskSchedulerManager
	{
		TaskSchedulerState State { get; }
		System.Threading.Tasks.Task RunTasksAsync(bool runTasksAsync);
		void AddTask(Tasks.Task task);
		void Stop();
	}
}
