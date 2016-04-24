namespace Grammaton
{
	public interface IConsumer
	{
		bool HasName { get; }
		string Name { get; }
		ConsumeResult Consume(Capture baseCapture, string input);
		ConsumeResult ConsumeAll(Capture baseCapture, string input);
		ConsumeResult Consume(Capture baseCapture, string input, out string consumed, out string output);
		IConsumer As(string name);
	}
}