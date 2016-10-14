namespace Grammaton
{
	using System;
	using static Grammaton.Utils.Utils;

	class Program
	{
		static void Main(string[] args)
		{
			var test = Group(
				Regex("[bf]oo").As("b/foo-cap"),
				Terminal("bar").As("bar-cap"));

			var resfoo = test.ConsumeAll(new Capture().Name("root"), "foobar");
			var resbar = test.ConsumeAll(new Capture().Name("root"), "boobar");

			Func<IConsumer, IConsumer, IConsumer> separatedListConsumerBuilder = (consumer, separator) =>
				Group(consumer, Many(Group(separator, consumer)));

			var identifierConsumer = Group(
				Regex("[_a-zA-Z]"),
				Many(Regex("[_a-zA-Z0-9]")));

			var whitespaceConsumer = Many(Any(Terminal(" "), Terminal("\t")));

			var val = Any(
				Regex("[0-9]+"),
				identifierConsumer,
				new SlowbindingConsumer("stmt")).As("value");

			var stmt = Group(
				Terminal("("),
				val,
				Many(
					Group(
						Terminal(" "),
						val)),
				Terminal(")"));

			SlowbindingConsumer.Register("stmt", stmt);

			var res = stmt.ConsumeAll(
				new Capture().Name("root"),
				"(for x (in foo) (do nothing))");

			var a = 0;
		}
	}
}