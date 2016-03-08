using System.Text.RegularExpressions;

namespace Grammaton
{
	using System;
	using System.Collections.Generic;

	public interface IConsumer
	{
		bool HasName { get; }
		string Name { get; }
		bool Consume(string input, out string consumed, out string output);
		void As(string name);
	}

	public class ConsumerBase
	{
		public bool HasName => this.Name != null;

		public string Name { get; private set; }

		public void As(string name)
		{
			this.Name = name;
		}
	}

	public class TerminalConsumer : ConsumerBase, IConsumer
	{
		private readonly string terminal;
		private readonly bool ignoreCase;

		public TerminalConsumer(string terminal, bool ignoreCase = false)
		{
			this.terminal = terminal;
			this.ignoreCase = ignoreCase;
		}

		public bool Consume(string input, out string consumed, out string output)
		{
			var comparison = this.ignoreCase
				? StringComparison.OrdinalIgnoreCase
				: StringComparison.Ordinal;

			if (input.StartsWith(this.terminal, comparison))
			{
				consumed = input.Substring(0, this.terminal.Length);
				output = input.Substring(
					this.terminal.Length,
					input.Length - this.terminal.Length);
				return true;
			}

			output = input;
			consumed = null;
			return false;
		}
	}

	public class AnyConsumer : ConsumerBase, IConsumer
	{
		private readonly List<IConsumer> consumers;

		public AnyConsumer(params IConsumer[] consumers)
		{
			this.consumers = new List<IConsumer>();
			this.consumers.AddRange(consumers);
		}

		public bool Consume(string input, out string consumed, out string output)
		{
			foreach (var consumer in this.consumers)
			{
				if (consumer.Consume(input, out consumed, out output))
				{
					return true;
				}
			}

			output = input;
			consumed = null;
			return false;
		}
	}

	public class ManyConsumer : ConsumerBase, IConsumer
	{
		private readonly IConsumer consumer;
		private readonly int? minimum;
		private readonly int? maximum;

		public ManyConsumer(IConsumer consumer, int? minimum = null, int? maximum = null)
		{
			this.consumer = consumer;
			this.minimum = minimum;
			this.maximum = maximum;
		}

		public bool Consume(string input, out string consumed, out string output)
		{
			var count = 0;
			var currentput = input;
			output = input;
			consumed = "";

			while (!this.maximum.HasValue || count < this.maximum)
			{
				string consumerConsumed;
				if (this.consumer.Consume(currentput, out consumerConsumed, out output))
				{
					currentput = output;
					consumed += consumerConsumed;
					count++;
				}
				else
				{
					break;
				}
			}

			if (this.minimum.HasValue && count < this.minimum)
			{
				consumed = null;
				output = input;
				return false;
			}

			return true;
		}
	}

	public class RegexConsumer : ConsumerBase, IConsumer
	{
		private readonly string pattern;

		public RegexConsumer(string pattern)
		{
			this.pattern = "^" + pattern;
		}

		public bool Consume(string input, out string consumed, out string output)
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

	public class Rule
	{
		private readonly List<IConsumer> consumers;

		public Rule(params IConsumer[] consumers)
		{
			this.consumers = new List<IConsumer>(consumers);
		}

		public bool Test(string input)
		{
			var currentput = input;

			foreach (var consumer in this.consumers)
			{
				string consumed;
				if (!consumer.Consume(currentput, out consumed, out currentput))
				{
					return false;
				}
			}

			return true;
		}
	}

	class Program
	{
		static TerminalConsumer Terminal(string terminal)
		{
			return new TerminalConsumer(terminal);
		}

		static AnyConsumer Any(params IConsumer[] consumers)
		{
			return new AnyConsumer(consumers);
		}

		static ManyConsumer Many(IConsumer consumer, int? minimum = null, int? maximum = null)
		{
			return new ManyConsumer(consumer, minimum, maximum);
		}

		static RegexConsumer Regex(string terminal)
		{
			return new RegexConsumer(terminal);
		}


		static void Main(string[] args)
		{
			var testRule = new Rule(Terminal("test"));
			var a = testRule.Test("test");

			testRule = new Rule(Any(Terminal("test"), Terminal("foo")));
			var c = testRule.Test("test");
			var b = testRule.Test("foo");

			testRule = new Rule(
				Many(Terminal("test")),
				Terminal("foo"));
			var d = testRule.Test("testtestfoo");

			testRule = new Rule(
				Many(Any(Terminal("foo"), Terminal("bar"))),
				Terminal("qux"));
			var e = testRule.Test("foofoobarfoobarfoobarbarqux");

			testRule = new Rule(
				Regex(@"\d\daaa"));
			var f = testRule.Test("12axa");
			var g = testRule.Test("12aaa");

			var functionCall = new Rule(
				Regex(""),
				Terminal("("),
				Regex(""),
				Many(Regex(",\s?

			var rule = new Rule(
				Terminal("hello"),
				Terminal(" "),
				Terminal("world"),
				Any(Terminal("!"), Terminal("?")));
		}
	}
}