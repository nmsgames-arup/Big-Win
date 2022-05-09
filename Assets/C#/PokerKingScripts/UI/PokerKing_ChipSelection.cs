using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerKing_ChipSelection : MonoBehaviour
{
    public Image ChipImage;
    public Sprite[] ChipFrame;

    void OnEnable()
    {
        StartCoroutine(StartChipAnimation());
    }
    void OnDisable()
    {
        StopChipAnimation();
    }

    public IEnumerator StartChipAnimation()
    {
        ChipImage.gameObject.SetActive(true);
        foreach (var item in ChipFrame)
        {
            ChipImage.sprite = item;
            yield return new WaitForSeconds(0.06f);
        }
        StartCoroutine(StartChipAnimation());
    }
    public void StopChipAnimation()
    {
        StopCoroutine(StartChipAnimation());
        ChipImage.gameObject.SetActive(false);
    }
}
