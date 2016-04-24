namespace Grammaton
{
	using System.Collections.Generic;

	public abstract class ConsumerBase : IConsumer
	{
		public bool HasName => this.Name != null;

		public string Name { get; private set; }

		public ConsumeResult Consume(Capture baseCapture, string input)
		{
			string consumed, output;
			return this.Consume(baseCapture, input, out consumed, out output);
		}

		public ConsumeResult ConsumeAll(Capture baseCapture, string input)
		{
			string consumed, output;
			var result = this.Consume(baseCapture, input, out consumed, out output);

			if (result == null || output != string.Empty)
			{
				throw new System.Exception();
			}

			return result;
		}

		public abstract ConsumeResult Consume(
			Capture baseCapture,
			string input,
			out string consumed,
			out string output);

		public IConsumer As(string name)
		{
			this.Name = name;
			return this;
		}

		public Capture SpawnIfWanted(Capture baseCapture)
		{
			Capture capture;

			if (this.HasName)
			{
				capture = new Capture().Name(this.Name);
				baseCapture?.AddChild(capture);
			}
			else
			{
				capture = baseCapture;
			}

			return capture;
		}

		public ConsumeResult CreateResult(bool success, List<Capture> childCaptures)
		{
			if (!success)
			{
				return new ConsumeResult(false);
			}

			var result = new ConsumeResult(true);

			if (this.HasName)
			{
				var cap = new Capture().Name(this.Name);

				foreach (var childCapture in childCaptures)
				{
					cap.AddChild(childCapture);
				}

				result.AddCapture(cap);
			}
			else
			{
				foreach (var childCapture in childCaptures)
				{
					result.AddCapture(childCapture);
				}
			}

			return result;
		}
	}
}