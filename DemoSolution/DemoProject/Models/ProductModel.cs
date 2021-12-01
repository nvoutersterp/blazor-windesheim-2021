using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Models
{
    public class ProductModel
    {
		public int Id { get; set; }

		public string Description { get; set; }

		public decimal Price { get; set; }

		public string PhotoUrl { get; set; }
	}
}
