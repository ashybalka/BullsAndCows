using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class AdReward : MonoBehaviour
{
    private void OnEnable() => YandexGame.RewardVideoEvent += Rewarded;
    private void OnDisable() => YandexGame.RewardVideoEvent -= Rewarded;
    void Rewarded(int id)
    {
        if (id == 0)
            GameObject.Find("GameManager").GetComponent<GameManager>().AddHint();
    }
}
