using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Grammaton
{
	using System;
	using System.Collections.Generic;
	using static Grammaton.Utils.Utils;

	public interface IConsumer
	{
		bool HasName { get; }
		string Name { get; }
		bool Consume(string input);
		bool ConsumeAll(string input);
		bool Consume(string input, out string consumed, out string output);
		void As(string name);
	}

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

	public class TerminalConsumer : ConsumerBase
	{
		private readonly string terminal;
		private readonly bool ignoreCase;

		public TerminalConsumer(string terminal, bool ignoreCase = false)
		{
			this.terminal = terminal;
			this.ignoreCase = ignoreCase;
		}

		public override bool Consume(string input, out string consumed, out string output)
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

	public class AnyConsumer : ConsumerBase
	{
		private readonly List<IConsumer> consumers;

		public AnyConsumer(params IConsumer[] consumers)
		{
			this.consumers = new List<IConsumer>();
			this.consumers.AddRange(consumers);
		}

		public override bool Consume(string input, out string consumed, out string output)
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

	public class ManyConsumer : ConsumerBase
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

		public override bool Consume(string input, out string consumed, out string output)
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

	public class GroupConsumer : ConsumerBase
	{
		private readonly List<IConsumer> consumers;

		public GroupConsumer(params IConsumer[] consumers)
		{
			this.consumers = new List<IConsumer>(consumers);
		}

		public override bool Consume(string input, out string consumed, out string output)
		{
			var currentput = input;
			output = input;
			consumed = "";

			foreach (var consumer in this.consumers)
			{
				string consumerConsumed;
				if (consumer.Consume(currentput, out consumerConsumed, out output))
				{
					currentput = output;
					consumed += consumerConsumed;
				}
				else
				{
					output = input;
					consumed = null;
					return false;
				}
			}

			return true;
		}
	}

	public class OptionalConsumer : ConsumerBase
	{
		private readonly IConsumer consumer;

		public OptionalConsumer(IConsumer consumer)
		{
			this.consumer = consumer;
		}

		public override bool Consume(string input, out string consumed, out string output)
		{
			if (this.consumer.Consume(input, out consumed, out output))
			{
				return true;
			}

			output = input;
			consumed = null;
			return true;
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
		static void Main(string[] args)
		{
			Func<IConsumer, IConsumer, IConsumer> separatedListConsumerBuilder = (consumer, separator) =>
				Group(consumer, Many(Group(separator, consumer)));

			var identifierConsumer = Group(Regex("[_a-zA-Z]"), Many(Regex("[_a-zA-Z0-9]")));
			var whitespaceConsumer = Many(Any(Terminal(" "), Terminal("\t")));

			var argConsumer = separatedListConsumerBuilder(
				identifierConsumer,
				Group(Terminal(","), Optional(whitespaceConsumer)));

			var argumentsConsumer = Group(
				Optional(whitespaceConsumer),
				identifierConsumer,
				Many(Group(Terminal(","), Optional(whitespaceConsumer), identifierConsumer)),
				Optional(whitespaceConsumer));

			var functionCallConsumer = Group(
				identifierConsumer,
				Optional(whitespaceConsumer),
				Terminal("("),
				Optional(argumentsConsumer),
				Terminal(")"),
				Optional(whitespaceConsumer),
				Terminal(";"));

			var a = functionCallConsumer.Consume("foo(test, bar);");
			var b = functionCallConsumer.Consume("foo();");
			var c = functionCallConsumer.Consume("foo ( test	);");

			var rule = Group(
				As("greeting", Any(
					Terminal("hello"),
					Terminal("bye"))),
				Many(
					minimum: 1,
					consumer: Any(
						Terminal(" "),
						Terminal("\t"))),
				Terminal("world"),
				As("punctuation", Many(
					Any(
						Terminal("!"),
						Terminal("?")))));
		}
	}
}