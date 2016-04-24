using System.Collections.Generic;

namespace Grammaton
{
	public class GroupConsumer : ConsumerBase
	{
		private readonly List<IConsumer> consumers;

		public GroupConsumer(params IConsumer[] consumers)
		{
			this.consumers = new List<IConsumer>(consumers);
		}

		public override ConsumeResult Consume(
			Capture baseCapture,
			string input,
			out string consumed,
			out string output
		) {
			var currentput = input;
			output = input;
			consumed = "";

			var capture = this.SpawnIfWanted(baseCapture);
			var childCaptures = new List<Capture>();

			foreach (var consumer in this.consumers)
			{
				string consumerConsumed;
				var childResult = consumer.Consume(capture, currentput, out consumerConsumed, out output);

				if (childResult.Success)
				{
					currentput = output;
					consumed += consumerConsumed;

					childCaptures.AddRange(childResult.Captures);
				}
				else
				{
					output = input;
					consumed = null;
					return new ConsumeResult(false);
				}
			}

			return this.CreateResult(true, childCaptures);
		}
	}
}