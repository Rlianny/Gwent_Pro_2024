using System;
using System.IO; // EnumerateFiles, StreamReader
using System.Collections.Generic; // Dictionary
using UnityEngine;

public static class Engine
{
    public static SearchResult Query(string query, DataFolder Content)
    {
        string suggestion = query;
        Query ToSearch = new Query(query);

        Dictionary<string, Dictionary<string, float>> Docs = Content.Relevance;

        List<SearchItem> docs_Scores = new List<SearchItem>();

        foreach (DataFile file in Content.Files)
        {
            string tittle = file.FileName;
            string snippet = file.FragmentWithWords(ToSearch.QueryWordsArray, file.FileRoot, DataFolder.IDF);
            float score = ScoreCalculator(ToSearch, Docs, file.FileName);

            SearchItem item = new SearchItem(tittle, snippet, score);

            if (item.Snippet != null)
            {
                docs_Scores.Add(item);
            }
        }

        SearchItem[] Items = new SearchItem[20];

        SearchItem[] sortedScores = Sort(docs_Scores.ToArray());

        for (int i = 0; i < Items.Length; i++)
        {
            Items[i] = sortedScores[i];
        }

        return new SearchResult(Items, suggestion);
    }

    private static float ScoreCalculator(Query ToSearch, Dictionary<string, Dictionary<string, float>> Docs, string file)
    {
        float dotProduct = 0;
        float dim1 = 0;
        float dim2 = 0;

        foreach (string word in Docs[file].Keys)
        {
            if (!ToSearch.DataQuery.ContainsKey(word))
                dotProduct += 0;
            else
            {
                dotProduct += ToSearch.DataQuery[word] * Docs[file][word];
                dim2 += (float)Math.Pow(ToSearch.DataQuery[word], 2);
            }

            dim1 += (float)Math.Pow(Docs[file][word], 2);
        }
        return dotProduct / ((dim1 == 0 || dim2 == 0) ? 1 : (float)(Math.Sqrt(dim1) * Math.Sqrt(dim2)));
    }

    private static SearchItem[] Sort(SearchItem[] docs_score)
    {
        for (int i = 0; i < docs_score.Length; i++)
        {
            for (int j = i; j < docs_score.Length; j++)
            {
                if (docs_score[j].Score > docs_score[i].Score)
                {
                    SearchItem temp = docs_score[j];
                    docs_score[j] = docs_score[i];
                    docs_score[i] = temp;
                }
            }
        }

        return docs_score;
    }
}