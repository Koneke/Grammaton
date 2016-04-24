namespace Grammaton
{
	using System.Collections.Generic;

	public class Rule
	{
		private readonly List<IConsumer> consumers;

		public Rule(params IConsumer[] consumers)
		{
			this.consumers = new List<IConsumer>(consumers);
		}

		public bool Test(string input)
		{
			var currentput = input;

			foreach (var consumer in this.consumers)
			{
				string consumed;
				var capture = consumer.Consume(
					new Capture().Name("root"),
					currentput,
					out consumed,
					out currentput
				);

				if (capture == null)
				{
					return false;
				}
			}

			return true;
		}
	}
}