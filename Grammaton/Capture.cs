namespace Grammaton
{
	using System.Collections.Generic;

	public class Capture
	{
		public static Capture Empty = new Capture();

		public bool HasName => !string.IsNullOrEmpty(this.captureName);

		public Capture Parent;

		private List<Capture> children;
		private string captureName;
		private string captured;
		private string capture => null;

		public Capture()
		{
			this.children = new List<Capture>();
		}

		public Capture(string capture)
			: this()
		{
			this.captured = capture;
		}

		public Capture Name(string name)
		{
			this.captureName = name;
			return this;
		}

		public Capture AddChild(Capture childCapture)
		{
			this.children.Add(childCapture);
			return this;
		}

		public Capture AddChildren(IEnumerable<Capture> childCaptures)
		{
			this.children.AddRange(childCaptures);
			return this;
		}

		public override string ToString()
		{
			return $"{this.captureName}: {this.captured}";
		}

		public Capture SetConsumed(string consumed)
		{
			this.captured = consumed;
			return this;
		}

		public Capture SetParent(Capture parent)
		{
			this.Parent = parent;
			return this;
		}
	}
}