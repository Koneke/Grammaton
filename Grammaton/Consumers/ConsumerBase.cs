namespace Grammaton
{
	public abstract class ConsumerBase : IConsumer
	{
		public bool HasName => this.Name != null;

		public string Name { get; private set; }

		public bool Consume(string input)
		{
			string consumed, output;
			return this.Consume(input, out consumed, out output);
		}

		public bool ConsumeAll(string input)
		{
			string consumed, output;
			var result = this.Consume(input, out consumed, out output);
			return result && output == string.Empty;
		}

		public abstract bool Consume(string input, out string consumed, out string output);

		public void As(string name)
		{
			this.Name = name;
		}
	}
}