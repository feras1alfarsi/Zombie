using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int highscore;
    public int currentScore;
    public Text highScoreText;
    public Text currentScoreText;
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

        highScoreText.text = highscore.ToString();
        currentScoreText.text = currentScore.ToString();
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
