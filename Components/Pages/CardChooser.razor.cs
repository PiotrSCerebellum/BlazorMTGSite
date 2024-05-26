using Microsoft.AspNetCore.Components;
using MTG.Services;
using System.Linq;


namespace MTG.Components.Pages
{
    public partial class CardChooser
    {
        public List<string> colors;
        public List<string> rarities;
        public List<string> types;
        bool showList = false;

        ElementReference results;

        SearchParameters search = new SearchParameters();
        Search services = new Search();

        protected override void OnInitialized()
        {
            base.OnInitialized();
            colors = services.GetColors();
            rarities = services.GetRarities();
            types = services.GetTypes();


        }
        void OnchangeType(ChangeEventArgs change)
        {

            if (types.Contains(change.Value.ToString()))
            {
                search.addType(change.Value.ToString());
            }

        }
        void OnchangeColor(string color, object isChecked)
        {
            if ((bool)isChecked)
                search.colors.Add(color);
            else
                search.colors.Remove(color);
        }
        void OnchangeRarity(string rarity, object isChecked)
        {

            if ((bool)isChecked)
                search.rarities.Add(rarity);
            else
                search.rarities.Remove(rarity);
        }
        void OnchangeExact(ChangeEventArgs change)
        {

            if (change.Value.ToString() == "exactly")
            {
                search.exactColors = true;
            }
            else
            {
                search.exactColors = false;
            }
        }

        public void executeSearch()
        {
            NavigationManager.NavigateTo("/SearchResult/" + search.toString());
        }
    }
}
