using Quartz;

namespace VideoPro
{
	public class HelCunJob : IJob
	{
		public void Execute(IJobExecutionContext context)
		{
			Steamer.SaveImage();
		}
	}
}
