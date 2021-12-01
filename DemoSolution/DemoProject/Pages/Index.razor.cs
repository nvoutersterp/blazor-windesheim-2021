using DemoProject.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoProject.Pages
{
    public partial class Index : ComponentBase
    {
        public string naam = "JP";

        public List<ProductModel> Products { get; set; } = new()
        {
            new ProductModel
            {
                Id = 4,
                Description = "Muis 4",
                Price = 110M,
                PhotoUrl = "https://resource.logitechg.com/content/dam/gaming/en/products/g502-lightspeed-gaming-mouse/g502-lightspeed-hero.png"
            },
            new ProductModel
            {
                Id = 8,
                Description = "Glas water",
                Price = 1.10M,
                PhotoUrl = "https://www.perfectmanage.eu/userfiles/2034/images/Glas%20water.jpg"
            },
            new ProductModel
            {
                Id = 15,
                Description = "Mondkapje",
                Price = 0.20M,
                PhotoUrl = "https://www.checkfit.nl/289-large_default/mondkapje-3-laags-10-stuks.jpg"
            }
        };

        void ChangeName()
        {
            this.naam += "Frank";
        }
    }
}
