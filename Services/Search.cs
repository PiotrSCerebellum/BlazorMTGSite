using System.Collections;

namespace MTG.Services
{
    public class Search
    {
        public string name { get; set; }
        public string cardText { get; set; }
        public int range;

        List<string> types = new List<string>();

        Hashtable colors = new Hashtable() {
        {"red", false},{"white", false},{"blue", false},{"green", false},
            {"black", false},{"colorless",false } };
        int cmc;
        Hashtable rarities = new Hashtable() {
        {"common", false},{"uncommon", false},{"rare", false},{"mythic", false},
            {"special", false},{"bonus",false } };
        
        public void colorCheck(string color)
        {
            colors[color] = !(bool)colors[color];
        }
        public void addType(string type)
        {
            types.Add(type.ToLower());
        }
        public void removeType(string type)
        {
            types.Remove(type.ToLower());
        }
        public void raritiesCheck(string rarity)
        {
            rarities[rarity] = !(bool)rarities[rarity];
        }




    }
}
