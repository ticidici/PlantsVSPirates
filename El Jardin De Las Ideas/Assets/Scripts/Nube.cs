using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Nube : MonoBehaviour
{
    public Text dropPrefab;
    private RectTransform rt;
    private float timer = 0f;
    private List<Text> drops;
    public int CompletedWords { get; private set; }

    void Awake() {
        drops = new List<Text>();
        rt = GetComponent<RectTransform>();
    }

    void Update() {
        if (timer > Random.Range(1f, 4f)) {
            Text newDrop = Instantiate(dropPrefab, GetSpawnPoint(), Quaternion.identity, gameObject.transform);
            newDrop.text = Palabras.GetWords();
            drops.Add(newDrop);
            timer = 0f;
        }

        timer += Time.deltaTime;
    }

    public void CheckInputPlayer(string s) {

        List<Text> aux = new List<Text>();

        // Pillar todos los textos con la misma longitud
        foreach (var item in drops) {
            if (item.text.StartsWith(s)) {
                item.color = Color.red;
                if (item.text.Length == s.Length) {
                    CompletedWords++;
                }
            }
            else {
                item.color = Color.black;
            }
        }
    }

    private Vector3 GetSpawnPoint() {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        // Rango horizontal de donde sale la gota
        var rangeHorizontal = corners.GroupBy(n => n.x)
                                    .Select(g => g.First().x)
                                    .ToList();

        // A una altura fija, dependiendo de la imagen de la nube se ha de
        // modificar la altura del spwan de las drops
        var VerticalPoint = corners.GroupBy(n => n.y)
                            .Select(g => g.First().y).FirstOrDefault();

        float x = Random.Range(rangeHorizontal[0], rangeHorizontal[1]);
        float y = VerticalPoint;

        return new Vector3(x, y, 0);
    }
}
