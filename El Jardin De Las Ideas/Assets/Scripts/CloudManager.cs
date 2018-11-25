using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CloudManager : MonoBehaviour
{
    public Text dropPrefab;
    private RectTransform rt;
    private float timer = 0f;
    private List<Text> drops;
    private float probSpawnDrop;
    public int CompletedWords { get; private set; }
    private GameManager gameManager;

    void Awake() {
        drops = new List<Text>();
        rt = GetComponent<RectTransform>();
        probSpawnDrop = Random.Range(Constants.MIN_RANGE_TO_SPAWN_DROP, Constants.MAX_RANGE_TO_SPAWN_DROP);
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update() {
        if (timer > probSpawnDrop) {
            Text newDrop = Instantiate(dropPrefab, GetSpawnPoint(), Quaternion.identity, gameObject.transform);
            newDrop.text = Palabras.GetWords();
            drops.Add(newDrop);
            timer = 0f;
            probSpawnDrop = Random.Range(Constants.MIN_RANGE_TO_SPAWN_DROP, Constants.MAX_RANGE_TO_SPAWN_DROP);
        }

        timer += Time.deltaTime;
    }

    public void CheckInputPlayer(string s) {

        List<Text> aux = new List<Text>();

        // Pillar todos los textos con la misma longitud
        foreach (var item in drops) { // TODO mejorar eesta escena tarantinesca
            if (item.text.ToLower().StartsWith(s.ToLower())) {
                item.color = Color.red;
                aux.Add(item);
                if (item.text.Length == s.Length) {
                    CompletedWords++;
                    StartCoroutine(DestroyAndFadeDrop(item));
                    InputManager.instance.EmptyBuffer();
                    if (CompletedWords == Constants.WORD_REQUIRED_TO_GROW_FLOWER) {
                        CompletedWords = 0;
                        gameManager.makePlantAppear();
                    }
                    SetAllDropsBlack();
                }
            }
            else {
                item.color = Color.black;
            }
        }

        // No tenemos ningun match.
        if (aux.Count == 0) {
            InputManager.instance.EmptyBuffer();
        }
    }

    private void SetAllDropsBlack() {
        foreach (var item in drops) {
            item.color = Color.black;
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

    private IEnumerator DestroyAndFadeDrop(Text drop) {
        for (float f = 1f; f >= 0; f -= 0.1f) {
            Color c = drop.color;
            c.a = f;
            drop.color = c;
            yield return null;
        }

        drops.Remove(drop);
        Destroy(drop.gameObject);
    }
}
