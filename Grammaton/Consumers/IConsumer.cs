namespace Grammaton
{
	public interface IConsumer
	{
		bool HasName { get; }
		string Name { get; }
		Capture Consume(Capture baseCapture, string input);
		Capture ConsumeAll(Capture baseCapture, string input);
		Capture Consume(Capture baseCapture, string input, out string consumed, out string output);
		IConsumer As(string name);
	}
}