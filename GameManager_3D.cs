using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_3D : MonoBehaviour
{
    public WallMover wall;
    float wallSpeed;

    public bool moveToNextLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        wallSpeed = wall.speed;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            wall.speed = 25;
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            wall.speed = wallSpeed;
        }

        if(moveToNextLevel)
        {
            // Load next level
            LoadNextLevel();
        }

        if(Input.GetKey(KeyCode.R))
        {
            RestartLevel();
        }

        if(Input.GetKey(KeyCode.Q))
        {
            Application.Quit();
        }

        if(Input.GetKey(KeyCode.N))
        {
            SceneManager.LoadScene(1);
        }
    }
    private void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Rotator r = FindAnyObjectByType<Rotator>();
        if (r != null)
        {
            r.RandomizeAllRotators();
            moveToNextLevel = false;
        }
    }

    private void RestartLevel()
    {
        string currentScenename = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScenename);
    }
}
