namespace Grammaton
{
	using System;
	using System.Collections.Generic;

	using static Grammaton.Utils.Utils;

	class Program
	{
		private static Capture MakeRoot()
		{
			return new Capture().SetName("root");
		}

		static void Main(string[] args)
		{
			var test = Many(
				Regex("[bf]oo")
					.As("bf"))
				.As("bfs");

			Capture root;
			List<string> q;
			var querier = new Querier();

			root = MakeRoot();
			test.ConsumeAll(root, "booboofoo");
			q = querier.Query("bfs/bf", root);

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
				new Capture().SetName("root"),
				"(for x (in foo) (do nothing))");

			var a = 0;
		}
	}
}