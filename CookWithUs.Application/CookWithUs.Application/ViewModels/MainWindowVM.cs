using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CookWithUs.Application.Database;
using CookWithUs.Application.ViewModels.DataStructures;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;

namespace CookWithUs.Application.ViewModels
{
    internal partial class MainWindowVM : ObservableObject
    {
       
        private readonly List<RecipeVM> _allResipes;

        public MainWindowVM()
        {

            var x = RecipesReader.GetAllRecipes();
            _allResipes = new List<RecipeVM>
            {
                new RecipeVM("Title 1", ["first"]),
                new RecipeVM("Title 2", ["first", "second"]),
                new RecipeVM("Title 3", ["second"])
            };

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

        public ObservableCollection<RecipeVM> Recipes { get; }

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
    }
}
