using System.Text.RegularExpressions;

namespace Grammaton
{
	public class RegexConsumer : ConsumerBase
	{
		private readonly string pattern;

		public RegexConsumer(string pattern)
		{
			this.pattern = "^" + pattern;
		}

		public override Capture Consume(
			Capture baseCapture,
			string input,
			out string consumed,
			out string output
		) {
			var regex = new Regex(this.pattern);
			var match = regex.Match(input);

			if (match.Success)
			{
				consumed = match.Value;
				output = input.Substring(match.Length, input.Length - match.Length);
				return new Capture(consumed).Name(this.Name);
			}

			output = input;
			consumed = null;
			return null;
		}
	}
}