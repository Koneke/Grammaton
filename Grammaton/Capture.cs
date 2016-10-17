namespace Grammaton
{
	using System.Collections.Generic;

	public class Capture
	{
		public static Capture Empty = new Capture();

		public Capture Parent;

		public bool HasName => !string.IsNullOrEmpty(this.Name);
		public string Name;

		public List<Capture> Children;
		public string Captured;
		private string capture => null;

		public Capture()
		{
			this.Children = new List<Capture>();
		}

		public Capture(string capture)
			: this()
		{
			this.Captured = capture;
		}

		public Capture SetName(string name)
		{
			this.Name = name;
			return this;
		}

		public Capture AddChild(Capture childCapture)
		{
			this.Children.Add(childCapture);
			return this;
		}

		public Capture AddChildren(IEnumerable<Capture> childCaptures)
		{
			foreach (var capture in childCaptures)
			{
				this.AddChild(capture);
			}

			return this;
		}

		public override string ToString()
		{
			return $"{this.Name}: {this.Captured}";
		}

		public Capture SetConsumed(string consumed)
		{
			this.Captured = consumed;
			return this;
		}

		public Capture SetParent(Capture parent)
		{
			this.Parent = parent;
			this.Parent.AddChild(this);
			return this;
		}
	}
}