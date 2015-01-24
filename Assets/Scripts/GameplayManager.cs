using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayManager : MonoBehaviour
{

    public GameObject P1, P2, P3, P4;
    public GameObject camera;
    public GUIText winText;

    private List<GameObject> PlayerList = new List<GameObject>();

    int round = 1;

    public Sprite Three, Two, One, Go;
    bool countdown = true;
    float timer = 4;

    int[] scores;
    bool[] alive;

    int deadCount = 0;

    void Start()
    {
        winText.enabled = false;
        scores = new int[4];
        alive = new bool[4];

        for (int i = 0; i < 4; i++)
        {
            scores[i] = 0;
        }
        camera.GetComponent<CameraControl>().enabled = false;
        //SpawnPlayers();
    }

    void SpawnPlayers()
    {
        PlayerList.Clear();
        camera.GetComponent<CameraControl>().Targets.Clear();

        GameObject go = (GameObject)Instantiate(P1);
        PlayerList.Add(go);

        go = (GameObject)Instantiate(P2);
        PlayerList.Add(go);

        go = (GameObject)Instantiate(P3);
        PlayerList.Add(go);

        go = (GameObject)Instantiate(P4);
        PlayerList.Add(go);

        

        for (int i = 0; i < 4; i++)
        {
            alive[i] = true;
            PlayerList[i].GetComponent<PlayerControl>().playerNumber = i + 1;
            camera.GetComponent<CameraControl>().Targets.Add(PlayerList[i]);
        }
        

        PlayerList[0].transform.position = new Vector3(-20, 15, 0);
        PlayerList[1].transform.position = new Vector3(-8, 15, 0);
        PlayerList[2].transform.position = new Vector3(8, 15, 0);
        PlayerList[3].transform.position = new Vector3(20, 15, 0);

        deadCount = 0;
    }

    public void PlayerDeath(int player)
    {
        alive[player - 1] = false;
        camera.GetComponent<CameraControl>().Targets[player - 1] = null;
        deadCount++;
    }

    void RoundEnd()
    {
        for (int i = 0; i < 4; i++)
        {
            if (alive[i])
                scores[i]++;

            Destroy(PlayerList[i]);

            if (scores[i] >= 3)
                GameOver(i + 1);
        }

        SpawnPlayers();
    }

    void GameOver(int playerWon)
    {
        winText.enabled = true;
        winText.GetComponent<GUIText>().text = "Player " + playerWon + " wins!";
        StartCoroutine("BackToMenu");
        deadCount = 0;
    }

    void Update()
    {
        if (countdown)
        {
            timer -= Time.deltaTime;

            if (timer < 4 && timer > 3)
            {
                this.GetComponent<SpriteRenderer>().sprite = Three;
            }
            else if (timer < 3 && timer > 2)
            {
                this.GetComponent<SpriteRenderer>().sprite = Two;
            }
            else if (timer < 2 && timer > 1)
            {
                this.GetComponent<SpriteRenderer>().sprite = One;
            }
            else if (timer < 1 && timer > 0)
            {
                this.GetComponent<SpriteRenderer>().sprite = Go;
            }
            else if (timer < 0)
            {
                this.GetComponent<SpriteRenderer>().enabled = false;
                camera.GetComponent<CameraControl>().enabled = true;
                SpawnPlayers();
                countdown = false;
            }


        }
        if (deadCount >= 3)
            RoundEnd();
    }

    IEnumerator BackToMenu()
    {
        float timer = 0;

        while (timer < 2)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        Application.LoadLevel("menu");
    }
}
