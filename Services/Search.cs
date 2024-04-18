using System.Collections;

namespace MTG.Services
{
    public class Search
    {
        public string cardName { get; set; }
        public string cardText { get; set; }
        public int cmc=-1;
        public int range=50;
        public int page=0;
        public bool exactColors = false;

        public List<string> types = new List<string>();

        public List<string> colors = new List<string>();

        public List<string> rarities = new List<string>();
        
        public void addType(string type)
        {
            types.Add(type.ToLower());
        }
        public void removeType(string type)
        {
            types.Remove(type.ToLower());
        }

        public string toString()
        {
            string result = "";
            result += "Name:" + cardName + ";";
            result += "CardText:" + cardText + ";";
            result += "Range:" + range + ";";
            result += "Types:";
            foreach (string type in types)
            {   
                if (type != "")
               { result += type + ","; }
            }
            result += ";";
            result += "Colors:";
            foreach (string color in colors)
            {
                if (color != "")
                    { result += color + ","; }   
            }
            result += ";";
            result += "ExactColors:" + exactColors + ";";
            result += "CMC:" + cmc + ";";
            result += "Rarities:";
            foreach (string rarity in rarities)
            {
                if (rarity != "")
                    { result += rarity + ","; }
            }
            result += ";";
            return result;
        }
        public Search FromString(string str)
        {
            Search search = new Search();
            string[] properties = str.Split(';');

            foreach (string prop in properties)
            {
                string[] parts = prop.Split(':', 2);
                if (parts.Length != 2)
                {
                    continue; // Handle malformed parts
                }

                string propertyName = parts[0].Trim();
                string propertyValue = parts[1].Trim();

                switch (propertyName)
                {
                    case "Name":
                        search.cardName = propertyValue;
                        break;
                    case "CardText":
                        search.cardText = propertyValue;
                        break;
                    case "Range":
                        search.range = int.Parse(propertyValue);
                        break;
                    case "Types":
                        search.types = propertyValue.Split(',').Where(str => !string.IsNullOrEmpty(str)).ToList();
                        break;
                    case "Colors":
                        var colors = propertyValue.Split(',').Where(str => !string.IsNullOrEmpty(str)).ToList();
                        foreach (string color in colors)
                        {
                            search.colors.Add(color);
                        }
                        break;
                    case "ExactColors":
                        search.exactColors = bool.Parse(propertyValue);
                        break;
                    case "CMC":
                        search.cmc = int.Parse(propertyValue);
                        break;
                    case "Rarities":
                        var rarities = propertyValue.Split(',').Where(str => !string.IsNullOrEmpty(str)).ToList();
                        foreach (var rarityCode in rarities)
                        {
                            search.rarities.Add(rarityCode);
                        }
                        break;
                }
            }

            return search;
        }



    }
}
