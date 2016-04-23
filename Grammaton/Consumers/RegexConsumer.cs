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

		public override bool Consume(string input, out string consumed, out string output)
		{
			var regex = new Regex(this.pattern);
			var match = regex.Match(input);

			if (match.Success)
			{
				consumed = match.Value;
				output = input.Substring(match.Length, input.Length - match.Length);
				return true;
			}

			output = input;
			consumed = null;
			return false;
		}
	}
}