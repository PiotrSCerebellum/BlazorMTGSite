using Microsoft.Build.Logging;
using MTG.Models;
using MTG.Services;

namespace MTG.Components.Pages
{
    public partial class Collection
    {
        IQueryable<Search.SimpleCardModel> myCards;
        Search services = new Search();
        string username;
        bool showCardChooser = false;
        public List<string> colors;
        public List<string> checkedColors;
        string cardName;

        private void NavigateToCard(string cardName)
        {
            NavigationManager.NavigateTo($"/Card/{cardName}");
        }
        void OnchangeColor(string color, object isChecked)
        {
            if ((bool)isChecked)
                checkedColors.Add(color);
            else
                checkedColors.Remove(color);
        }
        protected void filterCards() {
            myCards = myCards.Where(w => w.Name.Contains(cardName));
            StateHasChanged();
        }

        protected async Task GetUsername()
        {
            var result = await ProtectedSessionStorage.GetAsync<string>("User");
            if (result.Success)
            {
                username = result.Value;
                Console.WriteLine("Awaited returned " + username + result.Value);
            }
        }
        protected async Task RemoveCard(long Id)
        {
           await services.RemoveCardFromCollection(username, Id);
           myCards = myCards.Where(c => c.Id != Id);
           StateHasChanged();
           await InvokeAsync(StateHasChanged);
           Console.WriteLine("removed card:" + Id + " from collection of " + username);
                //NavigationManager.NavigateTo("/collection");

            
        }
        protected override async Task OnInitializedAsync()
        {
            base.OnInitialized();
            var result = await ProtectedSessionStorage.GetAsync<string>("User");
            if (result.Success)
            {
                username = result.Value;
                colors=checkedColors = services.GetColors();
            }
            if (username == null)
            {
                NavigationManager.NavigateTo("/Login");
            }
            else
            {

                if (!string.IsNullOrEmpty(username))
                {
                    myCards = await services.GetCollectionAsync(username);
                }
                foreach (var card in myCards)
                {
                    Console.WriteLine(card.Name);
                }
            }
        }




    }
}
