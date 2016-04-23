namespace Grammaton
{
	public class OptionalConsumer : ConsumerBase
	{
		private readonly IConsumer consumer;

		public OptionalConsumer(IConsumer consumer)
		{
			this.consumer = consumer;
		}

		public override bool Consume(string input, out string consumed, out string output)
		{
			if (this.consumer.Consume(input, out consumed, out output))
			{
				return true;
			}

			output = input;
			consumed = null;
			return true;
		}
	}
}