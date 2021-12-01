using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Shared
{
	public partial class Autocompleter<T> : ComponentBase
	{
		[Parameter]
		public List<T> Data { get; set; }

		public string Query { get; set; }

		public List<AutocompleterItem> Suggestions { get; set; }

		public ElementReference MijnElement { get; set; }

		[Parameter]
		public RenderFragment<AutocompleterItem> ItemTemplate { get; set; }

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

		public void HandleKeyUp(KeyboardEventArgs args)
		{
			if (args.Key == "ArrowDown")
			{
				Next();
			}
			else
			{
				Autocomplete();
			}
		}

		public class AutocompleterItem
		{
			public bool IsHighlighted { get; set; }

			public T Item { get; set; }
		}
	}
}
