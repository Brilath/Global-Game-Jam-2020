using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public GameManager Instance { get; private set; }
    public List<Repairable> Repairables = new List<Repairable>();
    [SerializeField] private bool win;
    [SerializeField] private float totalTime;
    [SerializeField] private int loseScene;
    [SerializeField] private int nextScene;
    [SerializeField] private int chillTime;

    public float TimeLeft { get; set; }

    private void Awake()
    {
        Instance = this;
        TimeLeft = totalTime;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        CheckRepairables();
        if (win)
            WinScreen();
        else
        {
            CheckLose();
            TimeLeft -= Time.deltaTime;
        }

    }

    private void CheckLose()
    {
        if (TimeLeft <= 0)
            SceneManager.LoadScene(loseScene);
    }

    private void WinScreen()
    {
        StartCoroutine(ChillOut());
    }

    private IEnumerator ChillOut()
    {
        yield return new WaitForSeconds(chillTime);
        SceneManager.LoadScene(nextScene);
    }

    private void CheckRepairables()
    {
        bool check = true;
        foreach (Repairable repairable in Repairables)
        {
            if (!repairable.Repaired)
                check = false;
        }
        win = check;
    }

}
