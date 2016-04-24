namespace Grammaton
{
	using System;
	using static Grammaton.Utils.Utils;

	class Program
	{
		static void Main(string[] args)
		{
			Func<IConsumer, IConsumer, IConsumer> separatedListConsumerBuilder = (consumer, separator) =>
				Group(consumer, Many(Group(separator, consumer)));

			var identifierConsumer = Group(
				Regex("[_a-zA-Z]"),
				Many(Regex("[_a-zA-Z0-9]"))
			);

			var whitespaceConsumer = Many(Any(Terminal(" "), Terminal("\t")));

			var val = Any(
				Regex("[0-9]+"),
				//Regex("\"[^\"]+\""),
				identifierConsumer,
				new SlowbindingConsumer("stmt")
			);

			var stmt = Group(
				Terminal("("),
				val,
				Many(
					Group(
						Terminal(" "),
						val
					)
				),
				Terminal(")")
			);

			SlowbindingConsumer.Register("stmt", stmt);

			var test = Terminal("foo").As("test capture");
			var result = test.ConsumeAll(null, "foo");

			test = Many(Terminal("foo").As("foo capture")).As("test capture");
			result = test.ConsumeAll(null, "foofoo");

			test = Group(
				Group(Terminal("foo").As("foocap")),
				Terminal("bar").As("barcap")).As("groupcap");
			result = test.ConsumeAll(null, "foobar");

			var res = stmt.ConsumeAll(
				new Capture().Name("root"),
				"(for x (in foo) (do nothing))"
			);

			var a = 0;
		}
	}
}