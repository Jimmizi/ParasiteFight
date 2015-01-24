using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayManager : MonoBehaviour {

    public GameObject PlayerPrefab;
    public GameObject camera;
    public GUIText winText;

    private List<GameObject> PlayerList = new List<GameObject>();

    int round = 1;

    public Sprite Three, Two, One, Go;
    bool gameStart = false;
    float timer = 4;


    int[] scores;
    bool[] alive;

    int deadCount = 0;

	void Start () 
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

        for (int i = 0; i < 4; i++)
        {
            GameObject go = (GameObject)Instantiate(PlayerPrefab);
            PlayerList.Add(go);
            PlayerList[i].GetComponent<PlayerControl>().playerNumber = i + 1;
            camera.GetComponent<CameraControl>().Targets.Add(PlayerList[i]);

            alive[i] = true;
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
        if (!gameStart)
        {
            timer -= Time.deltaTime;

            if (timer < 4 && timer > 3)
                this.GetComponent<SpriteRenderer>().sprite = Three;

            if (timer < 3 && timer > 2)
                this.GetComponent<SpriteRenderer>().sprite = Two;

            if (timer < 2 && timer > 1)
                this.GetComponent<SpriteRenderer>().sprite = One;

            if (timer < 1 && timer > 0)
                this.GetComponent<SpriteRenderer>().sprite = Go;

            if (timer < 0)
            {
                SpawnPlayers();
                camera.GetComponent<CameraControl>().enabled = true;
                this.GetComponent<SpriteRenderer>().enabled = false;
                gameStart = true;
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
