using System.Collections;

namespace MTG.Services
{
    public class Search
    {
        public string cardName { get; set; }
        public string cardText { get; set; }
        public int range=50;
        public bool exactColors = false;

        List<string> types = new List<string>();

        public Hashtable colors = new Hashtable() {
        {"red", false},{"white", false},{"blue", false},{"green", false},
            {"black", false},{"colorless",false } };
        public int cmc;
        public Hashtable rarities = new Hashtable() {
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

        public string toString()
        {
            string result = "";
            result += "Name: " + cardName + "\n";
            result += "Card Text: " + cardText + "\n";
            result += "Range: " + range + "\n";
            result += "Types: ";
            foreach (string type in types)
            {
                result += type + " ";
            }
            result += "\n";
            result += "Colors: ";
            foreach (DictionaryEntry color in colors)
            {
                if((bool)color.Value)
                    { result += color.Key + " "; }   
            }
            result += "\n";
            result += "Exact Colors: " + exactColors + "\n";
            result += "CMC: " + cmc + "\n";
            result += "Rarities: ";
            foreach (DictionaryEntry rarity in rarities)
            {
                if((bool)rarity.Value)
                    { result += rarity.Key + " "; }
            }
            result += "\n";
            return result;
        }



    }
}
