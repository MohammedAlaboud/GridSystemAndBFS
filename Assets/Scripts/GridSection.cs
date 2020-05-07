using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSection : MonoBehaviour
{
    public bool alreadyChecked = false; //need to determine if a section has already been visited or checked previosuly so as to not check it again. None of the sections have been checked at first by default 
    public GridSection checkedFrom; //to keep track of which section each of them were checked from. This will help keep track of the path taken to reach each section

    const int gridIncrement = 10; //chose this value as a design choice, it is possible to serialize this field for further experimenting. Ultimately has no effect on the pathfinding/searching algorithm which is why it is set to const

    public int GetGridIncrement() //getter method to acess the increments to lock to and will determines the size of the sections
    {
        return gridIncrement; //getting it rather than making it public ensures it is not modified unintentionally
    }

    public Vector2Int GetPositionOfSectionInGrid() //computes the position of the section in the grid using a coordinate system and based on the grid increment 
    {
        return new Vector2Int( //dividing by the increment to ensure the grid values are seprated by increments of 1 rather than the world translations between the sections
            Mathf.RoundToInt(transform.position.x / gridIncrement),
            Mathf.RoundToInt(transform.position.z / gridIncrement)
        );
    }

    public void setSectionColor(Color c) //Used to be able to have different colors for specific sections
    {
        MeshRenderer topFaceRenderer = transform.Find("TopFace").GetComponent<MeshRenderer>(); //access the top quad that makes up the cube for the section
        topFaceRenderer.material.color = c; //change the color using the given paramter
    }
}
