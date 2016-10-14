namespace GrammatonTests.Simple
{
	using Xunit;
	using Grammaton;
	using static Grammaton.Utils.Utils;

	public class TerminalTests
	{
		public Capture MakeRoot()
		{
			return new Capture().Name("root");
		}

		[Fact]
		public void TerminalConsumeConsumesCorrectInput()
		{
			const string test = "test";
			var consumer = Terminal(test);
			Assert.True(consumer.Consume(MakeRoot(), test).Success);
		}

		[Fact]
		public void TerminalConsumeAllConsumesCorrectInput()
		{
			const string test = "test";
			var consumer = Terminal(test);
			Assert.True(consumer.ConsumeAll(MakeRoot(), test).Success);
		}

		[Fact]
		public void TerminalConsumeReturnsFalseOnBadInput()
		{
			const string test = "test";
			const string bad = "bad";
			var consumer = Terminal(test);
			Assert.False(consumer.Consume(MakeRoot(), bad).Success);
		}

		[Fact]
		public void TerminalConsumeAllReturnsFalseOnTooMuchInput()
		{
			const string test = "test";
			var consumer = Terminal(test);
			Assert.Throws<System.Exception>(() => consumer.ConsumeAll(MakeRoot(), test + " too much"));
		}
	}

	public class AnyTests
	{
		public Capture MakeRoot()
		{
			return new Capture().Name("root");
		}

		[Fact]
		public void AnyConsumeConsumesCorrectInput()
		{
			const string first = "first";
			const string second = "second";
			var consumer = Any(Terminal(first), Terminal(second));
			Assert.True(consumer.Consume(MakeRoot(), first).Success);
			Assert.True(consumer.Consume(MakeRoot(), second).Success);
		}

		[Fact]
		public void AnyConsumeAllConsumesAll()
		{
			const string first = "first";
			const string second = "second";
			var consumer = Any(Terminal(first), Terminal(second));
			Assert.True(consumer.ConsumeAll(MakeRoot(), first).Success);
			Assert.True(consumer.ConsumeAll(MakeRoot(), second).Success);
		}

		[Fact]
		public void AnyConsumeReturnsFalseOnBadInput()
		{
			const string first = "first";
			const string second = "second";
			const string bad = "bar";
			var consumer = Any(Terminal(first), Terminal(second));
			Assert.False(consumer.Consume(MakeRoot(), bad).Success);
		}

		[Fact]
		public void AnyConsumeAllReturnsFalseOnTooMuchInput()
		{
			const string first = "first";
			const string second = "second";
			var consumer = Any(Terminal(first), Terminal(second));
			Assert.Throws<System.Exception>(() => consumer.ConsumeAll(MakeRoot(), first + " too much").Success);
			Assert.Throws<System.Exception>(() => consumer.ConsumeAll(MakeRoot(), second + " too much").Success);
		}
	}

	public class ManyTests
	{
		public Capture MakeRoot()
		{
			return new Capture().Name("root");
		}

		[Fact]
		public void ManyConsumeConsumesCorrectInput()
		{
			const string test = "test";
			var consumer = Many(Terminal(test));
			Assert.True(consumer.Consume(MakeRoot(), test).Success);
		}

		[Fact]
		public void ManyWithMinimumConsumesCorrectInput()
		{
			const string test = "test";
			var consumer = Many(Terminal(test), minimum: 3);
			Assert.True(consumer.Consume(MakeRoot(), test + test + test).Success);
		}

		[Fact]
		public void ManyWithMinimumReturnsFalseOnBadInput()
		{
			const string test = "test";
			var consumer = Many(Terminal(test), minimum: 3);
			Assert.False(consumer.Consume(MakeRoot(), test + test).Success);
		}

		[Fact]
		public void ManyWithMaximumConsumesCorrectInput()
		{
			const string test = "test";
			var consumer = Many(Terminal(test), maximum: 3);
			Assert.True(consumer.Consume(MakeRoot(), string.Empty).Success);
			Assert.True(consumer.Consume(MakeRoot(), test).Success);
			Assert.True(consumer.Consume(MakeRoot(), test + test).Success);
			Assert.True(consumer.Consume(MakeRoot(), test + test + test).Success);
		}

		[Fact]
		public void ManyWithMaximumStopsConsumingAtMaximum()
		{
			const string test = "test";
			var consumer = Many(Terminal(test), maximum: 3);
			string consumed, output;
			Assert.True(consumer.Consume(MakeRoot(), test + test + test + test, out consumed, out output).Success);
			Assert.Equal(test, output);
		}
	}

	public class GroupTests
	{
		public Capture MakeRoot()
		{
			return new Capture().Name("root");
		}

		[Fact]
		public void GroupConsumeConsumesCorrectInput()
		{
			const string first = "first";
			const string second = "second";
			var consumer = Group(Terminal(first), Terminal(second));
			Assert.True(consumer.Consume(MakeRoot(), first + second).Success);
		}

		[Fact]
		public void GroupConsumeAllConsumesCorrectInput()
		{
			const string first = "first";
			const string second = "second";
			var consumer = Group(Terminal(first), Terminal(second));
			Assert.True(consumer.ConsumeAll(MakeRoot(), first + second).Success);
		}

		[Fact]
		public void GroupConsumeReturnsFalseOnBadInput()
		{
			const string first = "first";
			const string second = "second";
			const string bad = "bad";
			var consumer = Group(Terminal(first), Terminal(second));
			Assert.False(consumer.Consume(MakeRoot(), first + bad + second).Success);
			Assert.False(consumer.Consume(MakeRoot(), second + first).Success);
			Assert.False(consumer.Consume(MakeRoot(), bad + first + second).Success);
		}

		[Fact]
		public void GroupConsumeAllReturnsFalseOnTooMuchInput()
		{
			const string first = "first";
			const string second = "second";
			var consumer = Group(Terminal(first), Terminal(second));
			Assert.Throws<System.Exception>(() => consumer.ConsumeAll(MakeRoot(), first + second + first).Success);
		}
	}

	public class OptionalTests
	{
		public Capture MakeRoot()
		{
			return new Capture().Name("root");
		}

		[Fact]
		public void OptionalConsumeConsumesCorrectInput()
		{
			const string test = "test";
			var consumer = Optional(Terminal(test));
			Assert.True(consumer.Consume(MakeRoot(), test).Success);
		}

		[Fact]
		public void OptionalConsumeConsumesEmptyInput()
		{
			const string test = "test";
			var consumer = Optional(Terminal(test));
			Assert.True(consumer.Consume(MakeRoot(), string.Empty).Success);
		}
	}
}