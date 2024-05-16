using MTG.Services;
using System.Linq;
using Microsoft.AspNetCore.Components;


namespace MTG.Components.Pages
{
    public partial class SearchResults
    {

        IQueryable myCards;
        Search services = new Search();
        [Parameter] public string? searchString { get; set; }
        SearchParameters search = new SearchParameters();
        private void NavigateToCard(string cardName)
        {
            NavigationManager.NavigateTo($"/Card/{cardName}");
        }
        public void printSearch()
        {

            Console.WriteLine(search.toString());
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            search = search.FromString(searchString);
            printSearch();
            myCards = services.GetCards(search);
        }
    }
}
