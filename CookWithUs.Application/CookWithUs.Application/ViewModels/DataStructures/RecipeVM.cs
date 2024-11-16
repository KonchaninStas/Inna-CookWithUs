using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookWithUs.Application.Entities;
using CookWithUs.Application.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace CookWithUs.Application.ViewModels.DataStructures
{
    internal partial class RecipeVM : ObservableObject
    {
        private readonly FoodRecipe _foodRecipe;
        public RecipeVM(FoodRecipe foodRecipe)
        {
            _foodRecipe = foodRecipe;

            Ingredients = new ObservableCollection<string>(foodRecipe.Ingredients);
        }

        public string Title => FoodRecipe.Tilte;

        public IReadOnlyCollection<DescriptionStep> DescriptionSteps => FoodRecipe.DescriptionSteps;

        public BitmapImage Image => FoodRecipe.Image;

        public ObservableCollection<string> Ingredients { get; }

        public FoodRecipe FoodRecipe => _foodRecipe;

        public bool CanBeDisplayed(string keywords)
        {
            var keywordsCollection = keywords.Split(' ');

            return Ingredients.Any(i => keywordsCollection.Any(k => i.Contains(k, StringComparison.OrdinalIgnoreCase)));
        }

        [RelayCommand]
        private void OpenDetailedWindow()
        {
            new DetailedWindow
            {
                DataContext = this,
            }.ShowDialog();
        }
    }
}
