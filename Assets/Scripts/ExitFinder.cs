using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitFinder : MonoBehaviour
{
    //Need to specify the start and end positions on the grid for the algorithm
    [SerializeField] GridSection mazeStart;
    [SerializeField] GridSection mazeExit;

    Dictionary<Vector2Int, GridSection> maze = new Dictionary<Vector2Int, GridSection>(); //holds all the grid sections that make up the maze

    Queue<GridSection> queue = new Queue<GridSection>(); //the queue required for the BFS algorithm

    bool pathFindingInProgress = true; //boolean used to determine when to stop searching

    GridSection checkingFrom; //keeping track of the section the algorithm is exploring from (at each iteration)
    private List<GridSection> path = new List<GridSection>(); //will hold the path based on backtracking through the sections explored from

    //used for specifying neighbours of each section. In this case they are the four immediate section connected to the current one (excluding diaganol neighbours but it is possible to make this system work with diaganol searching)
    Vector2Int[] neighbours = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left }; 

    //main method for handling the grid for the algorithm and processing the path as well
    public List<GridSection> ProcessMazeAndGetPath() //will return the path for the follower object in the scene to take
    {
        LoadGrid(); 
        SpecifyStartAndEndOnGrid();
        FindShortestPathToExit();
        ProcessFoundPath();
        return path;
    }

    private void LoadGrid() //setting up the grid system for the maze
    {
        var gridSections = FindObjectsOfType<GridSection>(); //find all sections
        //for each of the sections
        foreach (GridSection section in gridSections)
        {
            bool sectionsOverlapping = maze.ContainsKey(section.GetPositionOfSectionInGrid()); //check if more than one section occupy the same position (has the same coordinate values) 
            if (sectionsOverlapping) //if two sections share the same location
            {
                print("Sections overlapping not added to dictionary more than once -> " + section); //notify user on console
            }
            else //otherwise add it to the maze grid and change its color
            {
                maze.Add(section.GetPositionOfSectionInGrid(), section);
                section.setSectionColor(Color.green);
            }
        }
    }

    private void SpecifyStartAndEndOnGrid() //change the color of the start and end positions for demsontration purposes
    {
        mazeStart.setSectionColor(Color.blue);
        mazeExit.setSectionColor(Color.red);
    }


    private void FindShortestPathToExit() //Main BFS algorithm implementation (fragmented into smaller functions for easier editing and undertsanding)
    {
        queue.Enqueue(mazeStart); //start by adding the starting section to the queue

        while (queue.Count > 0 && pathFindingInProgress) //while the queue is not empty and finding algorithm has not been halted
        {
            checkingFrom = queue.Dequeue(); //dequeue the section the current one was reached/checked from 
            CheckIfGoalFound(); //check if the goal was reached
            CheckNeighbouringSections(); //explore the neighbouring sections 
            checkingFrom.alreadyChecked = true; //mark the current section as checked/visisted

        }

    }

    private void CheckIfGoalFound() //check if the goal was founding during searching
    {
        if (checkingFrom == mazeExit)
        {
            print("EXIT FOUND"); //bebug line to notify user on console when exit was found during path finding
            pathFindingInProgress = false; //set to false to halt further path finding 
        }
    }

    private void CheckNeighbouringSections() //checking neighbours for BFS algorithm
    {
        if (!pathFindingInProgress) { return; } //stop checking neighbours when path finding is done

        //for every neighbouring section 
        foreach (Vector2Int section in neighbours)
        {
            Vector2Int neighbouringPosition = checkingFrom.GetPositionOfSectionInGrid() + section; //get the neighbour (section)

            if (maze.ContainsKey(neighbouringPosition)) //check if the neighbour exists in the grid then queue the new neighbour
            {
                GridSection neighbouringSection = maze[neighbouringPosition]; //assign the neighbouring section
                
                if (neighbouringSection.alreadyChecked || queue.Contains(neighbouringSection))  //skip section if it has already been checked or is already in the queue (requires checking both for edge and end cases)
                {
                    //skip
                }
                else //otherwise
                {
                    queue.Enqueue(neighbouringSection); //queue the neighbour
                    neighbouringSection.checkedFrom = checkingFrom; //keep track of section the neighbour was checked from
                }
            }

        }
    }

    private void ProcessFoundPath() //with the path obtained from the main part of the BFS algorithm, process the list for the follower to use
    {
        path.Add(mazeExit); //add the maze exit as it is not in the path due to the way the BFS is implemented here (same for the start later)

        GridSection sectionReachedFrom = mazeExit.checkedFrom; //start the backtracking process

        while(sectionReachedFrom != mazeStart) //while the start has not been reached during the backtracking process
        {
            path.Add(sectionReachedFrom); //add successive waypoints 
            sectionReachedFrom = sectionReachedFrom.checkedFrom; //moving backwards through the path from start to finish to record and construct the path in a list
        }

        path.Add(mazeStart); //add the start position as it is not recorded the way the start of the algorithm is implemnted

        path.Reverse(); //reverse the list from the backtracking process to get a list of sections from the maze start to the maze exit (in correct order)
    }

}
