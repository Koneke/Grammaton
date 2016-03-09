namespace GrammatonTests.Simple
{
	using Xunit;
	using static Grammaton.Utils.Utils;

	public class TerminalTests
	{
		[Fact]
		public void TerminalConsumeConsumesCorrectInput()
		{
			const string test = "test";
			var consumer = Terminal(test);
			Assert.True(consumer.Consume(test));
		}

		[Fact]
		public void TerminalConsumeAllConsumesCorrectInput()
		{
			const string test = "test";
			var consumer = Terminal(test);
			Assert.True(consumer.ConsumeAll(test));
		}

		[Fact]
		public void TerminalConsumeReturnsFalseOnBadInput()
		{
			const string test = "test";
			const string bad = "bad";
			var consumer = Terminal(test);
			Assert.False(consumer.Consume(bad));
		}

		[Fact]
		public void TerminalConsumeAllReturnsFalseOnTooMuchInput()
		{
			const string test = "test";
			var consumer = Terminal(test);
			Assert.False(consumer.ConsumeAll(test + " too much"));
		}
	}

	public class AnyTests
	{
		[Fact]
		public void AnyConsumeConsumesCorrectInput()
		{
			const string first = "first";
			const string second = "second";
			var consumer = Any(Terminal(first), Terminal(second));
			Assert.True(consumer.Consume(first));
			Assert.True(consumer.Consume(second));
		}

		[Fact]
		public void AnyConsumeAllConsumesAll()
		{
			const string first = "first";
			const string second = "second";
			var consumer = Any(Terminal(first), Terminal(second));
			Assert.True(consumer.ConsumeAll(first));
			Assert.True(consumer.ConsumeAll(second));
		}

		[Fact]
		public void AnyConsumeReturnsFalseOnBadInput()
		{
			const string first = "first";
			const string second = "second";
			const string bad = "bar";
			var consumer = Any(Terminal(first), Terminal(second));
			Assert.False(consumer.Consume(bad));
		}

		[Fact]
		public void AnyConsumeAllReturnsFalseOnTooMuchInput()
		{
			const string first = "first";
			const string second = "second";
			var consumer = Any(Terminal(first), Terminal(second));
			Assert.False(consumer.ConsumeAll(first + " too much"));
			Assert.False(consumer.ConsumeAll(second + " too much"));
		}
	}

	public class ManyTests
	{
		[Fact]
		public void ManyConsumeConsumesCorrectInput()
		{
			const string test = "test";
			var consumer = Many(Terminal(test));
			Assert.True(consumer.Consume(test));
		}

		[Fact]
		public void ManyWithMinimumConsumesCorrectInput()
		{
			const string test = "test";
			var consumer = Many(Terminal(test), minimum: 3);
			Assert.True(consumer.Consume(test + test + test));
		}

		[Fact]
		public void ManyWithMinimumReturnsFalseOnBadInput()
		{
			const string test = "test";
			var consumer = Many(Terminal(test), minimum: 3);
			Assert.False(consumer.Consume(test + test));
		}

		[Fact]
		public void ManyWithMaximumConsumesCorrectInput()
		{
			const string test = "test";
			var consumer = Many(Terminal(test), maximum: 3);
			Assert.True(consumer.Consume(string.Empty));
			Assert.True(consumer.Consume(test));
			Assert.True(consumer.Consume(test + test));
			Assert.True(consumer.Consume(test + test + test));
		}

		[Fact]
		public void ManyWithMaximumStopsConsumingAtMaximum()
		{
			const string test = "test";
			var consumer = Many(Terminal(test), maximum: 3);
			string consumed, output;
			Assert.True(consumer.Consume(test + test + test + test, out consumed, out output));
			Assert.Equal(test, output);
		}
	}

	public class GroupTests
	{
		[Fact]
		public void GroupConsumeConsumesCorrectInput()
		{
			const string first = "first";
			const string second = "second";
			var consumer = Group(Terminal(first), Terminal(second));
			Assert.True(consumer.Consume(first + second));
		}

		[Fact]
		public void GroupConsumeAllConsumesCorrectInput()
		{
			const string first = "first";
			const string second = "second";
			var consumer = Group(Terminal(first), Terminal(second));
			Assert.True(consumer.ConsumeAll(first + second));
		}

		[Fact]
		public void GroupConsumeReturnsFalseOnBadInput()
		{
			const string first = "first";
			const string second = "second";
			const string bad = "bad";
			var consumer = Group(Terminal(first), Terminal(second));
			Assert.False(consumer.Consume(first + bad + second));
			Assert.False(consumer.Consume(second + first));
			Assert.False(consumer.Consume(bad + first + second));
		}

		[Fact]
		public void GroupConsumeAllReturnsFalseOnTooMuchInput()
		{
			const string first = "first";
			const string second = "second";
			var consumer = Group(Terminal(first), Terminal(second));
			Assert.False(consumer.ConsumeAll(first + second + first));
		}
	}

	public class OptionalTests
	{
		[Fact]
		public void OptionalConsumeConsumesCorrectInput()
		{
			const string test = "test";
			var consumer = Optional(Terminal(test));
			Assert.True(consumer.Consume(test));
		}

		[Fact]
		public void OptionalConsumeConsumesEmptyInput()
		{
			const string test = "test";
			var consumer = Optional(Terminal(test));
			Assert.True(consumer.Consume(string.Empty));
		}
	}
}