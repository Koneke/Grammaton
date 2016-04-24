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

		public override Capture Consume(
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
				var capture = new Capture(consumed).Name(this.Name);
				return capture;
			}

			output = input;
			consumed = null;
			return null;
		}
	}
}