using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class SearchResult
{
    public SearchItem[] ItemsArray;

    public SearchResult(SearchItem[] items, string suggestion)
    {
        if (items == null) {
            throw new ArgumentNullException("items");
        }

        this.ItemsArray = items;
        this.Suggestion = suggestion;
    }

    public SearchResult() : this(new SearchItem[0], " ") 
    {

    }

    public string Suggestion { get; private set; }

    public IEnumerable<SearchItem> Items() {
        return this.ItemsArray;
    }

    public int Count { get { return this.ItemsArray.Length; } }
}