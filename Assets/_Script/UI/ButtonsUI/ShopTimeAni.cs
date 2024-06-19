using System.Collections;
using UnityEngine;

public class ShopTimeAni : MonoBehaviour
{
    [SerializeField] Vector3 startPos;

    public void Play()
    {
        StartCoroutine("ScaleRotAnim");
    }

    private IEnumerator ScaleRotAnim()
    {
        RectTransform t = GetComponent<RectTransform>();
        t.anchoredPosition = startPos;
        float timer = 1.5f;
        float angle = 0;
        while (timer >= 0)
        {
            float scaleMultiplier = Mathf.Abs(Mathf.Sin(angle));
            t.localScale = Vector3.one * scaleMultiplier;
            angle += Time.deltaTime * 4;
            timer-=Time.deltaTime;
            yield return null;
        }
        timer = 1f;

        t.localScale = Vector3.one;
        while (timer >= 0)
        {
            t.anchoredPosition = Vector3.Lerp(startPos + Vector3.up * 1000f + Vector3.right* 1500f, startPos, timer);
            timer -= Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
