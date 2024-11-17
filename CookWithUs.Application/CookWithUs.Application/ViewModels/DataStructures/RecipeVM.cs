using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookWithUs.Application.Database;
using CookWithUs.Application.Entities;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace CookWithUs.Application.ViewModels.DataStructures
{
    internal partial class RecipeVM : ObservableRecipient
    {
        private readonly Action _isStarChanged;
        private readonly FoodRecipe _foodRecipe;
        private readonly MainWindowVM _mainWindowVM;

        public RecipeVM(MainWindowVM mainWindowVM, Action isStarChanged, FoodRecipe foodRecipe)
        {
            _isStarChanged = isStarChanged;
            _foodRecipe = foodRecipe;
            _mainWindowVM = mainWindowVM;

            Ingredients = new ObservableCollection<string>(foodRecipe.Ingredients);
            IsStar = foodRecipe.IsStar;
            CanStarBeChanged = true;
        }

        [ObservableProperty]
        private bool _canStarBeChanged;

        public string Title => FoodRecipe.Tilte;

        public IReadOnlyCollection<DescriptionStep> DescriptionSteps => FoodRecipe.DescriptionSteps;

        public BitmapImage Image => FoodRecipe.Image;

        public ObservableCollection<string> Ingredients { get; }

        public FoodRecipe FoodRecipe => _foodRecipe;

        [ObservableProperty]
        private bool _isStar;

        partial void OnIsStarChanged(bool value)
        {
            _foodRecipe.IsStar = value;
            RecipesReader.SetStar(_foodRecipe);
            _isStarChanged();
        }

        public bool CanBeDisplayed(string keywords)
        {
            var keywordsCollection = keywords.Split(' ');

            foreach (var keyword in keywordsCollection)
            {
                if (!Ingredients.Any(i => i.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                {
                    return false;
                }
            }
            return true;
        }

        [RelayCommand]
        private void OpenDetailedWindow()
        {
            _mainWindowVM.OpenDetails(this);
        }
    }
}
