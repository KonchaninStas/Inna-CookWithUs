using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookWithUs.Application.Database;
using CookWithUs.Application.ViewModels.DataStructures;
using System.Collections.ObjectModel;

namespace CookWithUs.Application.ViewModels
{
    internal partial class MainControlVM : ObservableRecipient
    {

        private readonly List<RecipeVM> _allResipes;
        private readonly MainWindowVM _mainWindowVM;

        public MainControlVM(MainWindowVM mainWindowVM)
        {
            _mainWindowVM = mainWindowVM;
            _allResipes = new List<RecipeVM>(RecipesReader.GetAllRecipes().Select(r => new RecipeVM(mainWindowVM, RequestDisplayStarChanged, r)));

            Recipes = new ObservableCollection<RecipeVM>();
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SearchCommand))]
        private string _searchKeywords;

        partial void OnSearchKeywordsChanged(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Recipes.Clear();
            }
        }

        [ObservableProperty]
        private ObservableCollection<RecipeVM> _recipes;

        [RelayCommand(CanExecute = nameof(IsSearchEnabled))]
        private void Search()
        {
            Recipes.Clear();

            foreach (RecipeVM recipe in _allResipes)
            {
                if (recipe.CanBeDisplayed(SearchKeywords))
                {
                    Recipes.Add(recipe);
                }
            }
        }

        private bool IsSearchEnabled()
        {
            return SearchKeywords?.Length > 0;
        }

        [RelayCommand]
        private void DisplayAll()
        {
            IReadOnlyCollection<RecipesGroupVM> enumerable = _allResipes.GroupBy(r => r.FoodRecipe.Type).
                Select(g => new RecipesGroupVM(g.Key, g)).ToArray();

            _mainWindowVM.DisplayAll(enumerable);
        }

        [RelayCommand(CanExecute = nameof(IsDisplayStarEnabled))]
        private void DisplayStarRecipes()
        {
            try
            {
                _allResipes.ForEach(r => r.CanStarBeChanged = false);
                IReadOnlyCollection<RecipesGroupVM> enumerable = _allResipes.Where(r => r.IsStar).GroupBy(r => r.FoodRecipe.Type).
                    Select(g => new RecipesGroupVM(g.Key, g)).ToArray();

                _mainWindowVM.DisplayStar(enumerable);
            }
            finally
            {
                _allResipes.ForEach(r => r.CanStarBeChanged = true);
            }
        }

        private bool IsDisplayStarEnabled()
        {
            return _allResipes.Any(r => r.IsStar);
        }

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DisplayStarRecipesCommand))]
        private bool _requestStar;

        private void RequestDisplayStarChanged()
        {
            RequestStar = !RequestStar;
        }
    }
}
