using MTG.Services;

namespace MTG.Components.Pages
{
    public partial class Collection
    {
        IQueryable<Search.SimpleCardModel> myCards;
        Search services = new Search();
        string username;

        private void NavigateToCard(string cardName)
        {
            NavigationManager.NavigateTo($"/Card/{cardName}");
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
