using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameplaySceneController : UIController
{
    public static GameplaySceneController Instance { get; set; }

    public GameObject promptMessagePrefab;
    public GameObject peluitVfxPrefab;
    public int cumulativeScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ShowPromptMessage(string message)
    {
        GameObject promptMsg = Instantiate(promptMessagePrefab, transform);
        promptMsg.GetComponent<PromptMessageHandler>().textMessage.text = message;
        promptMsg.transform.DOMoveY(promptMsg.transform.position.y + 25, 1);
        Destroy(promptMsg, 1.2f);
    }
}
