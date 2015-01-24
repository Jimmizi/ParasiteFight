using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayManager : MonoBehaviour {

    public GameObject PlayerPrefab;
    public GameObject camera;

    private List<GameObject> PlayerList = new List<GameObject>();

    int round = 1;

    int[] scores;
    bool[] alive;

    int deadCount = 0;

	void Start () 
    {
        scores = new int[4];
        alive = new bool[4];

        for (int i = 0; i < 4; i++)
        {
            scores[i] = 0;
        }

        SpawnPlayers();
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
        }

        SpawnPlayers();
    }

    void Update() 
    {
        if (deadCount >= 3)
            RoundEnd();

        
	}
}
