namespace Grammaton
{
	public class OptionalConsumer : ConsumerBase
	{
		private readonly IConsumer consumer;

		public OptionalConsumer(IConsumer consumer)
		{
			this.consumer = consumer;
		}

		public override Capture Consume(
			Capture baseCapture,
			string input,
			out string consumed,
			out string output
		) {
			var capture = this.HasName
				? baseCapture.AddChild(new Capture().Name(this.Name))
				: baseCapture;
			var childCapture = this.consumer.Consume(capture, input, out consumed, out output);

			if (childCapture != null)
			{
				capture.AddChild(childCapture);
			}

			output = input;
			consumed = null;
			return capture;
		}
	}
}