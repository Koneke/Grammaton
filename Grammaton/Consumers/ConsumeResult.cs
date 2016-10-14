namespace Grammaton
{
	using System.Collections.Generic;

	public class ConsumeResult
	{
		public bool Success { get; }
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

		public ConsumeResult AddCaptures(IEnumerable<Capture> captures)
		{
			foreach (var capture in captures)
			{
				this.AddCapture(capture);
			}

			return this;
		}

		public override string ToString()
		{
			var successString = this.Success ? "Success" : "Failure";
			return $"{successString}! Capcount: {this.Captures.Count}";
		}
	}
}