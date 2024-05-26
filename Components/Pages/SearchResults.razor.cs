using MTG.Services;
using System.Linq;
using Microsoft.AspNetCore.Components;
using static MTG.Services.Search;


namespace MTG.Components.Pages
{
    public partial class SearchResults
    {

        IQueryable<SimpleCardModel> myCards;
        Search services = new Search();
        [Parameter] public string? searchString { get; set; }
        SearchParameters search = new SearchParameters();
        private void NavigateToCard(string cardName)
        {
            NavigationManager.NavigateTo($"/Card/{cardName}");
        }
        private void NextPage()
        {
            if(52<=myCards.Count())
            {
                search.page++;
                NavigationManager.NavigateTo("/SearchResult/" + search.toString());
            }
        }
        private void PreviousPage()
        {
            if (search.page > 0)
            {
                search.page--;
                NavigationManager.NavigateTo("/SearchResult/" + search.toString());
            }
        }
        private bool CanNavigateNext()
        {
            return myCards.Count() >= 52;
        }

        private bool CanNavigatePrevious()
        {
            return search.page > 0;
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
