using System.Collections.Generic;

namespace Grammaton
{
	public class OptionalConsumer : ConsumerBase
	{
		private readonly IConsumer consumer;

		public OptionalConsumer(IConsumer consumer)
		{
			this.consumer = consumer;
		}

		public override ConsumeResult Consume(
			Capture baseCapture,
			string input,
			out string consumed,
			out string output
		) {
			var childResult = this.consumer.Consume(baseCapture, input, out consumed, out output);

			var result = new ConsumeResult(true);
			var capture = this.HasName
				? this.SpawnCapture(consumed).SetParent(baseCapture)
				: baseCapture;

			if (childResult.Success)
			{
				capture.AddChildren(childResult.Captures);
			}
			else
			{
				output = input;
				consumed = null;
			}

			var children = this.HasName
				? new List<Capture> { capture }
				: childResult.Captures;

			result.AddCaptures(children);

			return result;
		}
	}
}