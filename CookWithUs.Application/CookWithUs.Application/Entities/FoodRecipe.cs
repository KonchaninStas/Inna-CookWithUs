using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace CookWithUs.Application.Entities
{
    internal class FoodRecipe
    {
        public required string Tilte { get; init; }

        public required string Ingredients { get; init; }

        public required string Description { get; init; }

        public required BitmapImage Image { get; init; }
    }
}
