using System.Linq;
using System.Xml.Linq;
using UnityEngine;

public static class Palabras
{
    private static string currentPoem = string.Empty;
    private static int iterator = 0;

    // Carga un poema o lo que sea que haya de manera random.
    // De momento solo son poemas.
    private static void LoadRandomPoem()
    {
        XDocument doc = XDocument.Load(string.Concat(System.Environment.CurrentDirectory, @"\Assets\Scripts\Poemas.xml"));
        var poems = doc.Descendants("Poemas");
        currentPoem = poems.ElementAt(Random.Range(0, Constants.MAX_POEMS)).Element("Poema").Value;
    }

    // Retorna dos palabras del poema actual.
    public static string GetWords()
    {
        if (string.IsNullOrEmpty(currentPoem) || iterator >= currentPoem.Length)
        {
            iterator = 0;
            LoadRandomPoem();
            Debug.Log(currentPoem);
        }

        return NextTwoWords();
    }

    // Usa el iterator para saber que dos palabras son las siguientes
    private static string NextTwoWords()
    {
        int words = 0;
        string wordsToReturn = string.Empty;

        while (words < 2 && iterator < currentPoem.Length)
        {
            if (currentPoem[iterator] == ' ')
            {
                words++;
            }

            wordsToReturn = string.Concat(wordsToReturn, currentPoem[iterator]);
            iterator++;
        }

        // Eliminamos el ultimo espacio
        wordsToReturn = wordsToReturn.Substring(0, wordsToReturn.Length - 1);

        return wordsToReturn;
    }
}