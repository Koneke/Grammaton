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

		public override ConsumeResult Consume(
			Capture baseCapture,
			string input,
			out string consumed,
			out string output
		) {
			var count = 0;
			var currentput = input;
			output = input;
			consumed = "";

			var c = this.HasName
				? this.SpawnCapture(consumed).SetParent(baseCapture)
				: baseCapture;
			var childCaptures = new List<Capture>();

			while (!this.maximum.HasValue || count < this.maximum)
			{
				string consumerConsumed;
				var childResult = this.consumer.Consume(c, currentput, out consumerConsumed, out output);

				if (childResult.Success)
				{
					currentput = output;
					consumed += consumerConsumed;
					count++;

					childCaptures.AddRange(childResult.Captures);
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
				return new ConsumeResult(false);
			}

			var result = new ConsumeResult(true);
			var children = this.HasName
				? new List<Capture> { c }
				: childCaptures;
			result.AddCaptures(children);
			return result;

			return this.CreateResult(true, childCaptures);
		}
	}
}