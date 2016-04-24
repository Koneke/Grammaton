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

		public override Capture Consume(
			Capture baseCapture,
			string input,
			out string consumed,
			out string output
		) {
			foreach (var consumer in this.consumers)
			{
				var childCapture = consumer.Consume(baseCapture, input, out consumed, out output);

				var capture = this.HasName
					? new Capture().Name(this.Name)
					: baseCapture;

				capture.AddChild(childCapture);

				return capture;
			}

			output = input;
			consumed = null;
			return null;
		}
	}
}