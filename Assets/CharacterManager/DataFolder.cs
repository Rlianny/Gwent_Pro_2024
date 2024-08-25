using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
public class DataFolder
{
    public string[] FilesRoot { get; set; }
    public DataFile[] Files { get; set; }
    public int NumberOfFiles { get; private set; }
    public Dictionary<string, Dictionary<string, float>> TF;
    public Dictionary<string, Dictionary<string, float>> Relevance;
    public static Dictionary<string, float> IDF { get; set; }

    public DataFolder(string root)
    {
        TF = new Dictionary<string, Dictionary<string, float>>();
        Relevance = new Dictionary<string, Dictionary<string, float>>();
        IDF = new Dictionary<string, float>();

        FilesRoot = Directory.EnumerateFiles(root, "*.txt").ToArray();

        NumberOfFiles = FilesRoot.Length;

        Files = new DataFile[NumberOfFiles];

        Debug.Log("Looking for pictures...");

        int count = 0;

        foreach (string path in FilesRoot)
        {
            DataFile file = new DataFile(path);
            Files[count] = file;
            count++;

            TF.Add(file.FileName, file.WordFreq);
        }


        foreach (DataFile file in Files)
        {
            Dictionary<string, float> wordRelevance = new();

            foreach (string word in file.WordFreq.Keys)
            {
                if (!IDF.ContainsKey(word))
                    IDF.Add(word, idfCalculator(word));

                wordRelevance.Add(word, relevanceCalculator(file.FileName, word));
            }

            Relevance.Add(file.FileName, wordRelevance);
        }

        Debug.Log($"Has been looked {NumberOfFiles} pictures for your characters");

    }

    private int countContains(string word)
    {
        int count = 0;
        foreach (string doc in TF.Keys)
        {
            if (TF[doc].ContainsKey(word))
            {
                count++;
            }
        }
        return count;
    }

    private float idfCalculator(string word)
    {
        float IDF = (float)Math.Log10((float)NumberOfFiles / ((float)countContains(word) + 1)) + 1;
        return IDF;
    }

    private float relevanceCalculator(string document, string word)
    {
        float tf = TF[document][word];
        float idf = idfCalculator(word);

        float relevance = tf * idf;

        return relevance;
    }

}
