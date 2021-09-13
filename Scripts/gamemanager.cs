using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gamemanager : MonoBehaviour
{
    public List<bird> birds;
    public List<Pig> pig;
    public static gamemanager _instance;
    private Vector3 originPos;

    public GameObject win;
    public GameObject lose;

    public GameObject[] stars;

    private int starsNum=0;

    private int totalNum = 10;

    private void Awake()
    {
        _instance = this;
        if (birds.Count > 0)
        {
         originPos = birds[0].transform.position;
        }
        
    }

    private void Start()
    {
        Initialized();
    }

    private void Initialized()
    {
        for(int i = 0; i < birds.Count; i++)
        {
            if (i == 0)
            {
                birds[i].transform.position = originPos;
                birds[i].enabled = true;
                birds[i].sp.enabled = true;
                birds[i].canMove = true;
            }
            else
            {
                birds[i].enabled = false;
                birds[i].sp.enabled = false;
                birds[i].canMove = false;
            }
        }
    }

    public void NextBird()
    {
        if (pig.Count > 0)
        {
            if (birds.Count > 0)
            {
                Initialized();
            }
            else
            {
                lose.SetActive(true);
            }
        }
        else
        {
            win.SetActive(true);
        }
    }

    public void ShowStars()
    {
        StartCoroutine("show");
    }

    IEnumerator show()
    {
        for (; starsNum < birds.Count + 1; starsNum++)
        {
            if (starsNum >= stars.Length)
            {
                break;
            }
            yield return new WaitForSeconds(0.2f);
            stars[starsNum].SetActive(true);
        }
    }

    public void Replay()
    {
        SaveDate();
        SceneManager.LoadScene(2);
    }

    public void Home()
    {
        SaveDate();
        SceneManager.LoadScene(1);
    }

    public void SaveDate()
    {
        if(starsNum > PlayerPrefs.GetInt(PlayerPrefs.GetString("nowLevel"))){
           PlayerPrefs.SetInt(PlayerPrefs.GetString("nowLevel"),starsNum);
        }

        int sum = 0;
        for(int i = 1; i <= totalNum; i++)
        {
            sum += PlayerPrefs.GetInt("level" + i.ToString());
        }


        PlayerPrefs.SetInt("totalNum", sum);
        print(PlayerPrefs.GetInt("totalNum")); 
    }
}
