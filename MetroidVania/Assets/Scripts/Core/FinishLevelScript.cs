using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLevelScript : MonoBehaviour
{
    [SerializeField] GameObject finishScreen;
    [SerializeField] float finishLevelDelay = 2f;

    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            finishScreen.SetActive(true);
            StartCoroutine(FinishLevelCoroutine());
        }
    }

    IEnumerator FinishLevelCoroutine()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(finishLevelDelay);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
