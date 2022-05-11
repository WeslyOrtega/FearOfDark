using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwapper : MonoBehaviour
{
    public GameObject[] players;
    public int currPlayer = 0;

    // Start is called before the first frame update
    void Start()
    {
        players[currPlayer].GetComponent<Playable>().isFocused = true;
        players[currPlayer].GetComponent<Playable>().turnSwapLightOn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Swap"))
        {
            Playable prevCharacter = players[currPlayer].GetComponent<Playable>();
            prevCharacter.isFocused = false;
            prevCharacter.turnSwapLightOff();

            currPlayer = ++currPlayer % players.Length;

            Playable currCharacter = players[currPlayer].GetComponent<Playable>();
            currCharacter.isFocused = true;
            currCharacter.turnSwapLightOn();
        }
    }
}
