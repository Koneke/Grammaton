namespace Grammaton
{
	using System.Linq;
	using System.Collections.Generic;

	public class Querier
	{
		public List<string> Query(string query, Capture root)
		{
			var parts = query.Split('/');
			var current = new [] { root };

			var paths = new List<List<Capture>>();
			paths.Add(new List<Capture> { root });

			foreach (var part in parts)
			{
				if (paths.Count == 0)
				{
					break;
				}

				var oldPaths = new List<List<Capture>>();
				var newPaths = new List<List<Capture>>();

				foreach (var path in paths)
				{
					var tail = path.Last();
					var tailChildren = tail.Children;
					var matching = path.Last().Children.Where(c => c.Name == part);

					foreach (var match in matching)
					{
						var newPath = new List<Capture>(path);
						newPath.Add(match);
						newPaths.Add(newPath);
					}

					oldPaths.Add(path);
				}

				paths.RemoveAll(p => oldPaths.Contains(p));
				paths.AddRange(newPaths);
			}

			return paths.Select(p => p.Last().Captured).ToList();
		}
	}
}