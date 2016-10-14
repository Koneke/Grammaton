using System.Collections.Generic;

namespace Grammaton
{
	public class AnyConsumer : ConsumerBase
	{
		private readonly List<IConsumer> consumers;

		public AnyConsumer(params IConsumer[] consumers)
		{
			this.consumers = new List<IConsumer>();
			this.consumers.AddRange(consumers);
		}

		public override ConsumeResult Consume(
			Capture baseCapture,
			string input,
			out string consumed,
			out string output
		) {
			foreach (var consumer in this.consumers)
			{
				var childResult = consumer.Consume(baseCapture, input, out consumed, out output);
				if (!childResult.Success) continue;

				var result = new ConsumeResult(true);
				var capture = this.HasName
					? this.SpawnCapture(consumed).SetParent(baseCapture)
					: baseCapture;

				capture.AddChildren(childResult.Captures);

				var children = this.HasName
					? new List<Capture> { capture }
					: childResult.Captures;

				result.AddCaptures(children);

				return result;
			}

			output = input;
			consumed = null;
			return new ConsumeResult(false);
		}
	}
}