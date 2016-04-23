namespace Grammaton
{
	public interface IConsumer
	{
		bool HasName { get; }
		string Name { get; }
		bool Consume(string input);
		bool ConsumeAll(string input);
		bool Consume(string input, out string consumed, out string output);
		void As(string name);
	}
}