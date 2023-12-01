using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int highscore;
    public int currentScore;
    public GameObject[] weapons;
    [SerializeField] private int currentweaponIndex = 0;

    void Start()
    {
        instance = this;
        SwitchWeapon(currentweaponIndex);
    }

    void Update()
    {
        if(currentScore > highscore)
        {
            highscore = currentScore;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2);
        }

        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchWeapon(3);
        }
    }

    void SwitchWeapon(int newIndex)
    {
        weapons[currentweaponIndex].SetActive(false);

        weapons[newIndex].SetActive(true);

        currentweaponIndex = newIndex;
    }
}
