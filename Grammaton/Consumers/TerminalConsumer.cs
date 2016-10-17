using System;

namespace Grammaton
{
	public class TerminalConsumer : ConsumerBase
	{
		private readonly string terminal;
		private readonly bool ignoreCase;

		public TerminalConsumer(string terminal, bool ignoreCase = false)
		{
			this.terminal = terminal;
			this.ignoreCase = ignoreCase;
		}

		public override ConsumeResult Consume(
			Capture baseCapture,
			string input,
			out string consumed,
			out string output
		) {
			var comparison = this.ignoreCase
				? StringComparison.OrdinalIgnoreCase
				: StringComparison.Ordinal;

			if (input.StartsWith(this.terminal, comparison))
			{
				consumed = input.Substring(0, this.terminal.Length);
				output = input.Substring(
					this.terminal.Length,
					input.Length - this.terminal.Length);

				var result = new ConsumeResult(true);

				if (this.HasName)
				{
					var capture = new Capture(consumed).SetName(this.Name);
					result.AddCapture(capture);
					baseCapture.AddChild(capture);
				}

				return result;
			}

			output = input;
			consumed = null;
			return new ConsumeResult(false);
		}
	}
}