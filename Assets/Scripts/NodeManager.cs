using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NodeManager : MonoBehaviour
{

    public GameObject levelNodeText;
    public AssetBundle bundle;

    [SerializeField]
    private bool playerHere;

    [SerializeField]
    private string levelToLoad;

    // Update is called once per frame
    void Update()
    {
        if ((/*Input.GetButton("Start") ||*/ Input.GetKey(KeyCode.Return)) && playerHere)
        {
            SceneManager.LoadScene(levelToLoad);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
            playerHere = true;
            levelNodeText.SetActive(true);
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerHere = true;
        levelNodeText.SetActive(false);
    }
}
