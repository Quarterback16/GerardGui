namespace Butler.Interfaces
{
	public interface IJob
	{
		bool IsTimeTodo(out string whyNot);

		string DoJob();
	}
}