namespace Grammaton
{
	using System.Collections.Generic;

	public class ConsumeResult
	{
		public bool Success { get; private set; }
		public List<Capture> Captures;

		public ConsumeResult(bool success)
		{
			this.Success = success;
			this.Captures = new List<Capture>();
		}

		public ConsumeResult AddCapture(Capture capture)
		{
			this.Captures.Add(capture);
			return this;
		}
	}
}