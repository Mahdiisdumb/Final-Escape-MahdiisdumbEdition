using UnityEngine;
using System.Collections;
public class SonicExeTaunt : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite[] tauntSprites;
    public float tauntSpeed = 0.5f;
    private Coroutine tauntCoroutine;
    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        tauntCoroutine = StartCoroutine(TauntCycle());
    }
    IEnumerator TauntCycle()
    {
        while (true)
        {
            for (int i = 0; i < tauntSprites.Length; i++)
            {
                if (spriteRenderer == null) yield break;
                spriteRenderer.sprite = tauntSprites[i];
                yield return new WaitForSeconds(tauntSpeed);
            }
        }
    }
    void OnDestroy()
    {
        if (tauntCoroutine != null)
            StopCoroutine(tauntCoroutine);
    }
}