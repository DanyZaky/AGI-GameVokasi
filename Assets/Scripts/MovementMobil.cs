using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementMobil : MonoBehaviour
{
    public float speed;
    public float targetX, targetY;

    private bool isFeel;
    private float timeFeel = 3, timeFeelCounter;

    public GameObject feel;
    private GameCondition gc;
    void Start()
    {
        feel.SetActive(false);
        gc = GameObject.Find("GameManager").GetComponent<GameCondition>();

        isFeel = false;
        timeFeelCounter = timeFeel;
    }

    void Update()
    {
        Movement(speed);

        if(isFeel == true)
        {
            timeFeelCounter -= 1f * Time.deltaTime;
            gc.score -= 4f * Time.deltaTime;
            feel.SetActive(true);

            if (timeFeelCounter < 0)
            {
                isFeel = false;
                timeFeelCounter = timeFeel;
                feel.SetActive(false);
            }
        }
    }

    private void Movement(float speedms)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, targetY, 0f), speedms * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Grab")
        {
            isFeel = true;
        }
    }
}