using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CookWithUs.Application.ViewModels.DataStructures
{
    internal partial class RecipeVM : ObservableObject
    {
        public RecipeVM(string title, IEnumerable<string> ingredients)
        {
            Title = title;

            Ingredients = new ObservableCollection<string>(ingredients);
        }

        public string Title { get; }

        public ObservableCollection<string> Ingredients { get; }

        public bool CanBeDisplayed(string keywords)
        {
            var keywordsCollection = keywords.Split(' ');

            return Ingredients.Any(i => keywordsCollection.Any(k => k == i));
        }

        [RelayCommand]
        private void OpenDetailedWindow()
        {

        }
    }
}
