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

		public override bool Consume(string input, out string consumed, out string output)
		{
			foreach (var consumer in this.consumers)
			{
				if (consumer.Consume(input, out consumed, out output))
				{
					return true;
				}
			}

			output = input;
			consumed = null;
			return false;
		}
	}
}