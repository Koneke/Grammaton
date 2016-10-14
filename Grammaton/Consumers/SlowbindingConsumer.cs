namespace Grammaton
{
	using System.Collections.Generic;

	public class SlowbindingConsumer : ConsumerBase
	{
		private static Dictionary<string, IConsumer> consumers = new Dictionary<string, IConsumer>();
		private string name;

		public static void Register(string name, IConsumer consumer)
		{
			consumers.Add(name, consumer);
		}

		public SlowbindingConsumer(string name)
		{
			this.name = name;
		}

		public override ConsumeResult Consume(
			Capture baseCapture,
			string input,
			out string consumed,
			out string output
		) {
			return consumers[this.name].Consume(baseCapture, input, out consumed, out output);
		}
	}
}