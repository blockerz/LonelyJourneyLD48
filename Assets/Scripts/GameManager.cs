using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public SpaceProbe Probe;

    public List<Message> Messages;

    public Director GameDirector;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;

        GameDirector = GetComponent<Director>();

        //sceneManager = GetComponentInChildren<SceneManager>();
        //GameMapPrefab = (GameObject)Resources.Load("prefabs/GameMap", typeof(GameObject));
        //playerPrefab = (GameObject)Resources.Load("prefabs/Player", typeof(GameObject));

        Messages = new List<Message>();
        //Messages.Add(new Message("System", "System Online"));
        Messages.Add(new Message("System", "All Systems <color=green>Green</color>."));
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
