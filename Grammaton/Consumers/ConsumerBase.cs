namespace Grammaton
{
	public abstract class ConsumerBase : IConsumer
	{
		public bool HasName => this.Name != null;

		public string Name { get; private set; }

		public Capture Consume(Capture baseCapture, string input)
		{
			string consumed, output;
			return this.Consume(baseCapture, input, out consumed, out output);
		}

		public Capture ConsumeAll(Capture baseCapture, string input)
		{
			string consumed, output;
			var result = this.Consume(baseCapture, input, out consumed, out output);

			if (result == null || output != string.Empty)
			{
				throw new System.Exception();
			}

			return result;
		}

		public abstract Capture Consume(Capture baseCapture, string input, out string consumed, out string output);

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
	}
}