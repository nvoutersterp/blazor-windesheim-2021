using Bunit;
using DemoProject.Shared;
using DemoProject.Tests.Models;
using Microsoft.AspNetCore.Components.Web;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DemoProject.Tests
{
	public class AutocompleterTests
	{
		List<Car> cars;
		IRenderedComponent<Autocompleter<Car>> fixture;
		Autocompleter<Car> sut;

		[SetUp]
		public void Setup()
		{
			var ctx = new Bunit.TestContext();
			fixture = ctx.RenderComponent<Autocompleter<Car>>(parameters =>
			{
				parameters.Add(x => x.ItemTemplate, autocompleterItem => $"<li>{autocompleterItem.Item.Make} {autocompleterItem.Item.Model}</li>");
			});
			sut = fixture.Instance; // system under test
			cars = new List<Car>
			{
				new Car { Make = "BMW", Model ="e46"},
				new Car { Make = "Opel", Model ="Astra"},
				new Car { Make = "Fiat", Model = "Multipla" },
				new Car { Make = "Mazda", Model = "MX5" },
				new Car { Make = "Toyota", Model = "MR2" },
				new Car { Make = "Bugati", Model = "Veyron" },
				new Car { Make = "Mazda", Model = "RX7" },
				new Car { Make = "Opel", Model = "Carsa" }
			};
		}

		[Test]
		public void Autocomplete_WithBasicQuery_ShouldMakeSuggestions()
		{
			sut.Data = cars;
			sut.Query = "e";

			sut.Autocomplete();

			var expected = new List<Car>
			{
				cars[0],
				cars[1],
				cars[5],
				cars[7],
			};
			Assert.AreEqual(expected, sut.Suggestions.Select(x => x.Item).ToList());
		}

		[Test]
		public void Autocomplete_WithCapitalQuery_ShouldMakeCaseInsensitiveSuggestions()
		{
			sut.Data = cars;
			sut.Query = "O";

			sut.Autocomplete();

			var expected = new List<Car>
			{
				cars[1],
				cars[4],
				cars[5],
				cars[7]
			};
			Assert.AreEqual(expected, sut.Suggestions.Select(x => x.Item).ToList());
		}

		[Test]
		public void Autocomplete_QueryThatMatchesMultipleProperties_ShouldAddItemsUniquely()
		{
			sut.Data = cars;
			sut.Query = "a";

			sut.Autocomplete();

			var expected = cars.Skip(1).ToList();
			Assert.AreEqual(expected, sut.Suggestions.Select(x => x.Item).ToList());
		}

		[Test]
		public void Next_NothingHighlighted_HighlightFirstItem()
        {
			sut.Data = cars;
			sut.Query = "a";

			sut.Autocomplete();
			sut.Next();

			Assert.AreEqual(true, sut.Suggestions.First().IsHighlighted);
			Assert.AreEqual(1, sut.Suggestions.Count(x => x.IsHighlighted));
		}

		[Test]
		public void Next_FirstItemHighlighted_HighlightSecondItem()
		{
			sut.Data = cars;
			sut.Query = "a";

			sut.Autocomplete();
			sut.Next();
			sut.Next();

			Assert.AreEqual(true, sut.Suggestions[1].IsHighlighted);
			Assert.AreEqual(1, sut.Suggestions.Count(x => x.IsHighlighted));
		}

		[Test]
		public void Next_LastItemHighlighted_HighlightFirstItem()
		{
			sut.Data = cars;
			sut.Query = "a";

			sut.Autocomplete();
            foreach (var suggestion in sut.Suggestions)
            {
				sut.Next();
            }
			sut.Next();

			Assert.AreEqual(true, sut.Suggestions.First().IsHighlighted);
			Assert.AreEqual(1, sut.Suggestions.Count(x => x.IsHighlighted));
		}

		[Test]
		public void Autocomplete_BasicQuery_RenderCars()
        {
			sut.Data = cars;
			sut.Query = "e";
			sut.Autocomplete();
			fixture.Render();

			Assert.AreEqual("BMW e46", fixture.Find("li").TextContent);
        }

		[Test]
		public void HandleKeyUp_Letter_ShouldAutocomplete()
		{
			sut.Data = cars;
			sut.Query = "e";
			sut.HandleKeyUp(new KeyboardEventArgs { Key = "o" });

			Assert.NotNull(sut.Suggestions);
		}

		[Test]
		public void HandleKeyUp_ArrowKey_ShouldNext()
		{
			sut.Data = cars;
			sut.Query = "e";
			sut.Autocomplete();
			sut.HandleKeyUp(new KeyboardEventArgs { Key = "ArrowDown" });

			Assert.True(sut.Suggestions.First().IsHighlighted);
		}
	}
}