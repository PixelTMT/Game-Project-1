using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class game_manager : MonoBehaviour
{
    [SerializeField]
    Player_Control player;

    [SerializeField]
    GameObject PlayerUI;

    [SerializeField]
    GameObject GameOverUI;

    [SerializeField]
    GameObject GameFinishUI;

    public bool _GameOver = false;
    bool _GameFinish = false;
    private IEnumerator Start()
    {
        PlayerUI = Instantiate(PlayerUI);
        while (!_GameFinish && !_GameOver)
        {
            if (player._live <= 0)
            {
                _GameOver = true;
                GameFinish("Game Over");
                player.gameObject.SetActive(false);
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public void GameFinish(string msg)
    {
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        GameOverUI = Instantiate(GameOverUI);
        foreach (Transform t in GameOverUI.transform)
        {
            if(t.name == "Gameover")
            {
                t.GetComponent<TextMeshProUGUI>().text = msg;
            }
            else if (t.name == "Score")
            {
                t.GetComponent<TextMeshProUGUI>().text = $"Score: {player._score}";
            }
        }
    }
}