using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMover : MonoBehaviour //used to move an object for demonstration purposes
{    
    // Start is called before the first frame update
    void Start()
    {
        ExitFinder pathFinder = FindObjectOfType<ExitFinder>(); //get the path finder class -> this will only work if one of these exists in the scene at a time
        List<GridSection> retrievedPath = pathFinder.ProcessMazeAndGetPath(); //get the path to follow from the path finder
        StartCoroutine(followFoundPath(retrievedPath));  //calls the method to move the follower one section at a time per second (achieved using Unity's Coroutine funtion)
    }

    //move the follower across the path one section at a time
    private IEnumerator followFoundPath(List<GridSection> retrievedPath) //using IEnumerator to be able to wait a second before getting the next grid section 
    {
        //follower moves to position of each section on the path towards the exit
        foreach (GridSection section in retrievedPath)
        {
            transform.position = section.transform.position; //translation
            yield return new WaitForSeconds(1f); //wait one second per movement
        }
        
    }
}
