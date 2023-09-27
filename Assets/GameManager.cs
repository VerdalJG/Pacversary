using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates
{
    Start, Phase1, Phase2, End
}

public class GameManager : MonoBehaviour
{
    [SerializeField] public static GameStates state = GameStates.Start;

    public static GameObject[] cupcakeArray;
    public static GameObject[] blocksArray;
    public GameObject candles;

    // Start is called before the first frame update
    void Start()
    {
        cupcakeArray = GameObject.FindGameObjectsWithTag("Cake");
        blocksArray = GameObject.FindGameObjectsWithTag("Block");
    }

    void Update()
    {
        if (Input.anyKeyDown && state == GameStates.Start)
        {
            state = GameStates.Phase1;
        }

        if (state == GameStates.Phase1)
        {
            if (CleanMovement.cakesEaten == cupcakeArray.Length)
            {
                state = GameStates.Phase2;
            }
        }
        if (state == GameStates.Phase2)
        {
            CleanMovement.speed = 400;
            if (CleanMovement.blocksEaten == blocksArray.Length)
            {
                state = GameStates.End;
            }
        }

        if (state == GameStates.End)
        {
            candles.SetActive(true);
        }
    }
}
