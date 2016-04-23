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

		public override bool Consume(string input, out string consumed, out string output)
		{
			var currentput = input;
			output = input;
			consumed = "";

			foreach (var consumer in this.consumers)
			{
				string consumerConsumed;
				if (consumer.Consume(currentput, out consumerConsumed, out output))
				{
					currentput = output;
					consumed += consumerConsumed;
				}
				else
				{
					output = input;
					consumed = null;
					return false;
				}
			}

			return true;
		}
	}
}