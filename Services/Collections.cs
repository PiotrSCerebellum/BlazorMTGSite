﻿using Microsoft.Identity.Client;
using MTG.Models;

namespace MTG.Services
{
    public class Collections
    {
        public static string[] CollectionToList(string collection) 
        { 
            string[] subs = collection.Split(',');
            return subs;
        }

        public static string CollectionTosString(string[] collection)
        {
            string collectionString = string.Join(",", collection);
            return collectionString;
        }
    }
}
