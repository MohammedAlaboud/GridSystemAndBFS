using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase] //makes it easier to select the whole cube rather than the quads that make up the cube
[ExecuteInEditMode] //allows the code to be executed and remaing running within the editor
[RequireComponent(typeof(GridSection))] //to access data from grid section script/component on this object (added automatically if it does not exist on the game object)

public class GridEditingHelper : MonoBehaviour //the goal of the script is to make it easier to edit and visualize the grid, but the calculations for position locking is dependent on each grid section
{
    GridSection gridSection; //establish the section of the grid this script belongs to

    private void Awake()
    {
        gridSection = GetComponent<GridSection>(); //get the grid section component on this object before executing anything elsd
    }

    // Update is called once per frame
    void Update()
    {
        LockPositionToGrid(); //snapping to positions to make up a uniform grid system
        UpdateLabelAndObjectName(); //update label and object name based on position in scene world
    }

    private void LockPositionToGrid() //ensures that sections are lock to specific postitions to follow a grid like structure when moved
    {
        int gridIncrement = gridSection.GetGridIncrement(); //get the increment value
        //lock to specific positions using calculations obtain from the sections. The sections are only allowed to be moved in the X and Z axis in the scene and are completely locked in the Y axis as the grid will be represented in two dimensions
        transform.position = new Vector3(gridSection.GetPositionOfSectionInGrid().x * gridIncrement, 0f, gridSection.GetPositionOfSectionInGrid().y * gridIncrement);
    }

    private void UpdateLabelAndObjectName() //modify the section name and labels on top of them based on their location in the grid relative to their location in the world 
    {
        TextMesh coordinateLabel; //the text mesh used as a label placed on the top face of the sections
        string coordinatePosition = gridSection.GetPositionOfSectionInGrid().x + "," + gridSection.GetPositionOfSectionInGrid().y; //hold the coordinate position based on the world position of the object (set increment taken into account)
        coordinateLabel = GetComponentInChildren<TextMesh>(); //find the text mesh component or label that will display the coordinate position
        coordinateLabel.text = coordinatePosition; //update the label text to display the coordinate position
        gameObject.name = "GridSection: " + coordinatePosition; //update the name of the object to the coordinate position
    }
}
