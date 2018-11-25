using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

public static class Palabras
{
    private static List<string> currentPoem = null;

    private static void LoadRandomPoem()
    {
        currentPoem = new List<string>();
        XDocument doc = XDocument.Load(string.Concat(System.Environment.CurrentDirectory, @"\Assets\Scripts\Poemas.xml"));
        var poems = doc.Descendants("Poemas");
        string currentPoemTemp = poems.ElementAt(UnityEngine.Random.Range(0, Constants.MAX_POEMS)).Element("Poema").Value;
        var splitted = currentPoemTemp.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
        currentPoem.AddRange(splitted);
    }

    public static string GetWord()
    {
        if(currentPoem == null) {
            LoadRandomPoem();
        }

        return currentPoem[UnityEngine.Random.Range(0, currentPoem.Count)];
    }
}