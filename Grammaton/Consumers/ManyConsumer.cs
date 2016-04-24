namespace Grammaton
{
	using System.Collections.Generic;

	public class ManyConsumer : ConsumerBase
	{
		private readonly IConsumer consumer;
		private readonly int? minimum;
		private readonly int? maximum;

		public ManyConsumer(IConsumer consumer, int? minimum = null, int? maximum = null)
		{
			this.consumer = consumer;
			this.minimum = minimum;
			this.maximum = maximum;
		}

		public override Capture Consume(
			Capture baseCapture,
			string input,
			out string consumed,
			out string output
		) {
			var count = 0;
			var currentput = input;
			output = input;
			consumed = "";

			var capture = this.SpawnIfWanted(baseCapture);
			var childCaptures = new List<Capture>();

			while (!this.maximum.HasValue || count < this.maximum)
			{
				string consumerConsumed;
				var childCapture = this.consumer.Consume(capture, currentput, out consumerConsumed, out output);

				if (childCapture != null)
				{
					currentput = output;
					consumed += consumerConsumed;
					count++;

					// if it doesn't have a name, it has already propagated the catch.
					if (childCapture.HasName)
					{
						childCaptures.Add(childCapture);
					}
				}
				else
				{
					break;
				}
			}

			if (this.minimum.HasValue && count < this.minimum)
			{
				consumed = null;
				output = input;
				return null;
			}

			foreach (var childCapture in childCaptures)
			{
				capture.AddChild(childCapture);
			}

			return capture;
		}
	}
}