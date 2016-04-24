namespace Grammaton.Consumers
{
	using System.Collections.Generic;

	public class ConsumeResult
	{
		public bool Success { get; private set; }
		public List<Capture> Captures;
	}
}