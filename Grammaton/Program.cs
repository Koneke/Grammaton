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

			var stmt = Group(
				Terminal("("),
				Many(whitespaceConsumer),
				identifierConsumer,
				Many(
					Group(
						Terminal(","),
						Many(whitespaceConsumer, 1, null),
						identifierConsumer)),
				Many(whitespaceConsumer),
				Terminal(")")
			);

			var val = Any(
				Regex("[0-9]+"),
				//Regex("\"[^\"]+\""),
				identifierConsumer,
				new SlowbindingConsumer("stmt")
			);

			stmt = Group(
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

			var res = stmt.ConsumeAll("(for x (in foo) (do nothing))");

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