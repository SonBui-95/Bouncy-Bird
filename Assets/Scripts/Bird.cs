using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Bird : MonoBehaviour
{
    Vector3 startPosBird;

    [SerializeField] float lauchPower = 350;
    [SerializeField] float maxDragRange = 3;

    bool isReloading = false;
    int bounCount = 0;
    bool isOnGround = false;
    float groundTime = 0;

    LineRenderer lineRenderer;

    [SerializeField] TMP_Text retryAttempsTxt;
    [SerializeField] TMP_Text levelTxt;
    [SerializeField] TMP_Text nextScreenTxt;

    static int retryAttemps = 2;

    void Start()
    {
        RetryAttempsPrintOut();

        startPosBird = transform.position;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.SetPosition(0, transform.position);

        levelTxt.text = "Level: " + SceneManager.GetActiveScene().buildIndex.ToString();

        nextScreenTxt.enabled = false;
    }

    void Update()
    {
        if(FindAnyObjectByType<Monster>(FindObjectsInactive.Exclude) == null)
        {
            isReloading = true;
            retryAttemps = 2;
            nextScreenTxt.text = "Congratulations! You've advanced to the next level";
            nextScreenTxt.enabled = true;
            Invoke(nameof(NextLevelToLoad), 3);
        }

        if(isOnGround)
        {
            groundTime += Time.deltaTime;
            if (!isReloading && groundTime >= 2) 
            {
                LoadWhenOutOfView();
            }
        }
    }

    void OnMouseDrag()
    {
        Vector3 mousePosScene = Input.mousePosition;
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(mousePosScene);
        mousePosWorld.z = 0;

        if(Vector2.Distance(startPosBird, mousePosWorld) > maxDragRange)
            mousePosWorld = Vector3.MoveTowards(startPosBird, mousePosWorld, maxDragRange);

        transform.position = mousePosWorld;
        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.enabled = true;
    }

    void OnMouseUp()
    {
        Vector3 directionAndMagnitude = startPosBird - transform.position;
        GetComponent<Rigidbody2D>().AddForce(directionAndMagnitude * lauchPower);
        GetComponent<Rigidbody2D>().gravityScale = 1;
        lineRenderer.enabled = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            bounCount++;
            isOnGround = true;
            groundTime = 0;
        }

        if(!isReloading && bounCount >= 15)
        {
            LoadWhenOutOfView();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        isOnGround = false;
        groundTime = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isReloading && other.CompareTag("Boundary"))
        {
            LoadWhenOutOfView();
        }
    }

    void NextLevelToLoad()
    {
        int nextLevelToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextLevelToLoad);
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void RetryAttempsPrintOut()
    {
        retryAttempsTxt.text = "Retry Attemps: " + retryAttemps.ToString();
    }

    void LoseSceneLoad()
    {
        retryAttemps = 2;
        SceneManager.LoadScene("Lose");
    }

    void LoadWhenOutOfView()
    {
        isReloading = true;
        retryAttemps--;
        if (retryAttemps != 0)
        {
            nextScreenTxt.text = "You didn't succeed. Let's try this level again";
            nextScreenTxt.enabled = true;
            Invoke(nameof(ReloadScene), 5);
        }
        else
        {
            Invoke(nameof(LoseSceneLoad), 5);
        }
    }
}
