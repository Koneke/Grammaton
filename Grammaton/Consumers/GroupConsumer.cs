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

		public override Capture Consume(
			Capture baseCapture,
			string input,
			out string consumed,
			out string output
		) {
			var currentput = input;
			output = input;
			consumed = "";

			var capture = this.SpawnIfWanted(baseCapture);
			var childCaptures = new List<Capture>();

			foreach (var consumer in this.consumers)
			{
				string consumerConsumed;
				var childCapture = consumer.Consume(capture, currentput, out consumerConsumed, out output);

				if (childCapture != null)
				{
					currentput = output;
					consumed += consumerConsumed;

					// if it doesn't have a name, it has already propagated the catch.
					if (childCapture != Capture.Empty && childCapture.HasName)
					{
						childCaptures.Add(childCapture);
					}
				}
				else
				{
					output = input;
					consumed = null;
					return null;
				}
			}

			foreach (var childCapture in childCaptures)
			{
				capture.AddChild(childCapture);
			}

			// if we're not named, we're not going to return any meaningful capture,
			// but we don't want to return null because that means that we failed to consume.
			// so we give Capture.Empty.
			// this is not entirely beautiful, we'll have to think about this a bit.
			// maybe return some kind of result object, which'd contain our capture if we got one.
			return this.HasName
				? capture
				: Capture.Empty;
		}
	}
}