using System.Windows.Media.Imaging;

namespace CookWithUs.Application.Entities
{
    internal class FoodRecipe
    {
        public int Id { get; init; }
        public required string Tilte { get; init; }

        public required IReadOnlyCollection<string> Ingredients { get; init; }

        public required IReadOnlyCollection<DescriptionStep> DescriptionSteps { get; init; }

        public required BitmapImage Image { get; init; }

        public required string Type { get; init; }

        public bool IsStar { get; init; }
    }
}
