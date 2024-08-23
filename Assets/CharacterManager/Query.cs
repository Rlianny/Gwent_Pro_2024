using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
public class Query
{
    public string InputQuery { get; set; }
    public Dictionary<string, float> DataQuery;
    public string[] QueryWordsArray { get; private set; }

    public Query(string input)
    {
        InputQuery = input;

        QueryWordsArray = CharacterTools.TxtProcessor(InputQuery);
        DataQuery = GetQueryRelevance(QueryWordsArray);

        //Debug.Log($"The Query {input} has been processed");
    }
    static private Dictionary<string, float> GetQueryRelevance(string[] QueryWordsArray)
    {
        Dictionary<string, float> DataQuery = new Dictionary<string, float>();

        foreach (string word in QueryWordsArray)
        {
            if (!DataQuery.Keys.Contains(word))
                DataQuery.Add(word, 1);

            else
                DataQuery[word]++;
        }

        Dictionary<string, float> DataQueryToReturn = new();

        foreach (string key in DataQuery.Keys)
        {
            //DataQuery[key] = DataQuery[key] / QueryWordsArray.Length;
            if (DataFolder.IDF.ContainsKey(key))
            {
                DataQueryToReturn.Add(key, DataQuery[key] * DataFolder.IDF[key]);
                //DataQuery[key] = DataQuery[key] * DataFolder.IDF[key];
            }
            else DataQueryToReturn.Add(key, DataQuery[key] / QueryWordsArray.Length);
        }

        return DataQueryToReturn;
    }
}

