using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.TextCore.Text;
public class DataFile
{
    public string FileRoot { get; private set; }
    public string FileName { get; private set; }
    public string FileContent { get; private set; }
    public int FileWords { get; private set; }
    public string[] AllWordsOnFile { get; set; }
    public Dictionary<string, float> WordFreq { get; set; }


    public DataFile(string root)
    {
        FileRoot = root;

        FileName = GetFileName(root);

        FileContent = GetFileContent(root);

        AllWordsOnFile = CharacterTools.TxtProcessor(FileContent);

        FileWords = AllWordsOnFile.Length;

        WordFreq = TFCalculator(AllWordsOnFile);
    }

    private static string GetFileName(string root)
    {
        string FileName = Path.GetFileName(root);
        FileName = Path.ChangeExtension(FileName, null);

        return FileName;
    }

    private static string GetFileContent(string root)
    {
        StreamReader reader = new StreamReader(root);
        string FileContent = reader.ReadToEnd();
        reader.Close();

        return FileContent;
    }

    private static Dictionary<string, float> TFCalculator(string[] AllWordsOnFile)
    {
        Dictionary<string, float> WordFreq = new Dictionary<string, float>();
        float maxFreq = 0;

        foreach (string word in AllWordsOnFile)
        {
            if (!WordFreq.Keys.Contains(word))
            {
                WordFreq.Add(word, 1);
            }
            else
            {
                WordFreq[word]++;
                maxFreq = Math.Max(maxFreq, WordFreq[word]);
            }
        }

        Dictionary<string, float> WordFreqToReturn = new();

        foreach (string key in WordFreq.Keys)
        {
            WordFreqToReturn.Add(key, WordFreq[key] / maxFreq);
        }

        return WordFreqToReturn;
    }

    public string FragmentWithWords(string[] query, string root, Dictionary<string, float> IDF)
    {
        StreamReader reader = new StreamReader(root);
        string content = reader.ReadToEnd().ToLower();
        reader.Close();
        content = CharacterTools.Transform(content);

        string[] words = content.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        int part1 = 0;
        int part2 = 0;

        string[] Query = WordsImportant(query, IDF);

        while (words.Length > 80)
        {
            string[] Part1 = Partition(words, 0, words.Length / 2);
            string[] Part2 = Partition(words, words.Length / 2, words.Length);

            int count = 0;

            for (int i = 0; i < Math.Min(Part1.Length, Part2.Length); i++)
            {
                count++;
                if (Query.Contains(Part1[i]))
                {
                    part1++;
                }
                if (Query.Contains(Part2[i]))
                {
                    part2++;
                }
            }
            if (Part1.Length > Part2.Length)
            {
                for (int i = count; i < Part1.Length; i++)
                {
                    if (Query.Contains(Part1[i]))
                    {
                        part1++;
                    }
                }
            }
            if (Part2.Length > Part1.Length)
            {
                for (int i = count; i < Part2.Length; i++)
                {
                    if (Query.Contains(Part2[i]))
                    {
                        part2++;
                    }
                }
            }
            if (part1 >= part2)
                words = Part1;
            else
                words = Part2;
            part1 = 0;
            part2 = 0;
        }

        string result = "";

        foreach (string a in words)
        {
            if (Query.Contains(a))
                result += "**" + a + "** ";
            else
                result += a + " ";
        }

        return "........" + result + "........";
    }

    string[] Partition(string[] words, int startIndex, int endIndex)
    {
        string[] result = new string[endIndex - startIndex];
        int position = 0;
        for (int i = startIndex; i < endIndex; i++)
        {
            result[position] = words[i];
            position++;
        }
        return result;
    }
    string[] WordsImportant(string[] AllWordsOnFile, Dictionary<string, float> IDF)
    {
        List<string> result = new List<string>();

        foreach (string word in AllWordsOnFile)
        {
            if (IDF.ContainsKey(word) && IDF[word] != 0 && word.Length > 3)
                result.Add(word);
        }

        return result.ToArray();
    }
}