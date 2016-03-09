namespace Grammaton.Utils
{
	public static class Utils
	{
		public static TerminalConsumer Terminal(string terminal)
		{
			return new TerminalConsumer(terminal);
		}

		public static AnyConsumer Any(params IConsumer[] consumers)
		{
			return new AnyConsumer(consumers);
		}

		public static ManyConsumer Many(IConsumer consumer, int? minimum = null, int? maximum = null)
		{
			return new ManyConsumer(consumer, minimum, maximum);
		}

		public static RegexConsumer Regex(string terminal)
		{
			return new RegexConsumer(terminal);
		}

		public static GroupConsumer Group(params IConsumer[] consumers)
		{
			return new GroupConsumer(consumers);
		}

		public static OptionalConsumer Optional(IConsumer consumer)
		{
			return new OptionalConsumer(consumer);
		}

		public static T As<T>(string name, T consumer) where T : IConsumer
		{
			consumer.As(name);
			return consumer;
		}
	}
}