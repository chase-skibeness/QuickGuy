using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class NodeManager : MonoBehaviour
{

    public GameObject levelNodeText;

    [SerializeField]
    private bool playerHere = false;


    // Update is called once per frame
    void Update()
    {
        
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
