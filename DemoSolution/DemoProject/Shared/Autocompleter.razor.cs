using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Shared
{
	public partial class Autocompleter<T> : ComponentBase
	{

		public List<T> Data { get; set; }

		public string Query { get; set; }

		public List<AutocompleterItem> Suggestions { get; set; }

		public void Autocomplete()
		{
			Suggestions = new List<AutocompleterItem>();

			foreach (var item in Data)
			{
				// reflection
				var props = item.GetType().GetProperties();
				foreach (var prop in props)
				{
					if (prop.PropertyType == typeof(string))
					{
						var value = (string)prop.GetValue(item);
						if (value.ToLower().Contains(Query.ToLower()))
						{
							Suggestions.Add(new AutocompleterItem
							{
								Item = item
							});
							break;
						}
					}
				}
			}
		}

		public void Next()
		{
			var index = Suggestions.FindIndex(x => x.IsHighlighted);
			if (index > -1)
			{
				Suggestions[index].IsHighlighted = false;
				Suggestions[(index + 1) % Suggestions.Count].IsHighlighted = true;
				return;
			}

			Suggestions.First().IsHighlighted = true;
		}

		public class AutocompleterItem
		{
			public bool IsHighlighted { get; set; }

			public T Item { get; set; }
		}
	}
}
