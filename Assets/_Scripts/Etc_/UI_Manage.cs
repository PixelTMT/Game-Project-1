using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Manage : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI _LiveText;

    [SerializeField]
    TextMeshProUGUI _ScoreText;
    IEnumerator Start()
    {
        Player_Control player = FindObjectOfType<Player_Control>();
        int score, live;
        score = player._score;
        live = player._live;
        while(true)
        {
            score = player._score;
            live = player._live;
            _ScoreText.text = $"Score: {score}";
            _LiveText.text = $"{live}x";
            yield return new WaitUntil(() => score != player._score || live != player._live);

            yield return new WaitForFixedUpdate();
        }
    }
}
