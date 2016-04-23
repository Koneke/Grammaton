namespace Grammaton
{
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

		public override bool Consume(string input, out string consumed, out string output)
		{
			var count = 0;
			var currentput = input;
			output = input;
			consumed = "";

			while (!this.maximum.HasValue || count < this.maximum)
			{
				string consumerConsumed;
				if (this.consumer.Consume(currentput, out consumerConsumed, out output))
				{
					currentput = output;
					consumed += consumerConsumed;
					count++;
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
				return false;
			}

			return true;
		}
	}
}