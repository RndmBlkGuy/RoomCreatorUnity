using UnityEngine;
using UnityEditor;

public class GenerateRooms : EditorWindow
{
    [SerializeField] private GameObject tile;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject ceiling;

    [SerializeField] private int roomWidth;

    [SerializeField] private int roomLength;

    [SerializeField] private int tileSizeWorldViewX;
    [SerializeField] private int tileSizeWorldViewY;

    [SerializeField] private float wallEdgePlacement;

    [SerializeField] private int hallWayLength;

    [MenuItem("Tools/Generate Rooms")]
    static void CreateGenerateRooms()
    {
        EditorWindow.GetWindow<GenerateRooms>();
    }
    private void OnGUI()
    {
        //All the prefabs used to create the room and the offset if there is one.
        //Note: will Change offset to calculate WallOffset = y/2
        tile = (GameObject)EditorGUILayout.ObjectField("Prefab tile", tile, typeof(GameObject), false);
        wall = (GameObject)EditorGUILayout.ObjectField("Prefab wall", wall, typeof(GameObject), false);
        ceiling = (GameObject)EditorGUILayout.ObjectField("Prefab Ceiling", ceiling, typeof(GameObject), false);
        wallEdgePlacement = EditorGUILayout.FloatField("Place Wall on edge pos", wallEdgePlacement );
        

         EditorGUILayout.LabelField("Create Room" );
         

        roomWidth = EditorGUILayout.IntField("Room Width", roomWidth);
        roomLength = EditorGUILayout.IntField("Room length", roomLength);

        tileSizeWorldViewX = EditorGUILayout.IntField("Size of Prefab Width (World Space)", tileSizeWorldViewX);
        tileSizeWorldViewY = EditorGUILayout.IntField("Size of Prefab Height(world Space", tileSizeWorldViewY);

        EditorGUILayout.LabelField("Create Hallway" );

        hallWayLength = EditorGUILayout.IntField("Hallway length", hallWayLength);


        //roomLength = EditorGUILayout.ObjectField("Room Height", roomLength, typeof(int), false);

        if (GUILayout.Button("Create room"))
        {
            //Can not be negative Note: need to throw exception
            if((roomLength < 0)|| (roomWidth < 0)){

            }

            else
            {
                //For Loop for now
                for (var i = 0; i <= roomLength; i++)
                {
                    for (var j = 0; j <= roomWidth; j++)
                    {
                        //Place floor piece in place
                        CreateSingleFloor(i, 0, j*tileSizeWorldViewY);
                        
                        //CreateSingleCeiling(i, 0, j);
                        

                        //If corner, create two walls to make corner
                        //if((i == 0)|| (j == 0)){
                            CreateCorners(i, j * tileSizeWorldViewY, roomWidth, roomLength);
                        //}

                        //If roomLength is larger that 2 but on edge, create walls on the long side edges
                        //else if((i > 0) || (i == roomLength)){
                            Debug.Log("Printing Wall Length: " + i);
                            CreateWallsLength(i , j * tileSizeWorldViewY, roomWidth, roomLength);
                        //}

                        //if roomWidth is larger than 2, create walls on the horizontial edges
                        //else if((j > 0) || (j <= roomLength)){
                            Debug.Log("Printing Wall Width: " + j);
                            CreateWallsWidth(i, j * tileSizeWorldViewY, roomWidth, roomLength);
                            
                       // }
                    }

                }
            }
         
        }

        if (GUILayout.Button("Create hallway"))
        {
            for(int i = 0; i < hallWayLength; i++){

                CreateSingleFloor(0, 0, i * tileSizeWorldViewX);
                CreateWallWidthTop(0, 0, i * tileSizeWorldViewX, wallEdgePlacement);
                CreateWallWidthBottom(0, 0, i * tileSizeWorldViewX, wallEdgePlacement);
            }
        }
        GUI.enabled = false;
        //EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
    }

//create corner
    void CreateCorners(int i, int j, float wallSideW, float wallSideL){
        if (((i == 0) && (j == 0)) )
        {
            CreateTopLeftCorner(i, 0, j, wallEdgePlacement);
        }

        else if ((i == 0) && (j == roomWidth))
        {
            CreateTopRightCorner(i, 0, j * tileSizeWorldViewY, wallEdgePlacement);


        }

        else if ((i == wallSideL) && (j == 0)){
            CreateBottomLeftCorner(i * tileSizeWorldViewX, 0, j, wallEdgePlacement);
        }

        else if ((i == wallSideL) && (j == wallSideW)){
            CreateBottomRightCorner(i * tileSizeWorldViewX, 0, j * tileSizeWorldViewY, wallEdgePlacement);
        }

    }
    //Width = Z direction
    void CreateWallsWidth(int i, int j, float wallSideW, float wallSideL){


//top
        if ((i == 0) && ((j > 0) && (j < wallSideW)))
        {

            CreateWallWidthTop(i, 0, j * tileSizeWorldViewY, wallEdgePlacement);

        }
//bottom
        else if ((i == wallSideL) && ((j > 0) && (j < wallSideW)))
        {

            CreateWallWidthBottom(i * tileSizeWorldViewX, 0, j * tileSizeWorldViewY, wallEdgePlacement);

        }

    }

//Length = x direction
    void CreateWallsLength(int i, int j, float wallSideW, float wallSideL){


        if (((i > 0) && (i < wallSideL)) && ((j == 0)))
        {

        CreateWallLengthLeft(i * tileSizeWorldViewX, 0, j, wallEdgePlacement);
            
        }

        else if ( ((i > 0) && (i < wallSideL)) && (j == wallSideW)){

         CreateWallLengthRight(i * tileSizeWorldViewX, 0, j * tileSizeWorldViewY, wallEdgePlacement);
        }

    }

    //Create Top Left Coner Wall
    void CreateTopLeftCorner(float x, float y, float z, float wallOffset)
    {

        Vector3 pos = new Vector3(x, y + wallOffset, z - wallOffset);
        Instantiate(wall, pos, wall.transform.rotation);

        pos = new Vector3(x - wallOffset, y + wallOffset, 0);
        Quaternion rot = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        Instantiate(wall, pos, rot);
    }

    //Create Top Right Coner Wall
    void CreateTopRightCorner(float x, float y, float z, float wallOffset)
    {

        Vector3 pos = new Vector3(x, y + wallOffset, z + wallOffset);
        Instantiate(wall, pos, wall.transform.rotation);

        pos = new Vector3(x - wallOffset, wallOffset, z);
        Quaternion rot = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        Instantiate(wall, pos, rot);
    }

    //Create Bottom Left Coner Wall
    void CreateBottomLeftCorner(float x, float y, float z, float wallOffset)
    {

        Vector3 pos = new Vector3(x, wallOffset, z - wallOffset);
        Instantiate(wall, pos, wall.transform.rotation);

        pos = new Vector3(x + wallOffset, wallOffset, z);
        Quaternion rot = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        Instantiate(wall, pos, rot);
    }

    //Create Bottom Right Coner Wall
    void CreateBottomRightCorner(float x, float y, float z, float wallOffset)
    {

        Vector3 pos = new Vector3(x, wallOffset, z + wallOffset);
        Instantiate(wall, pos, wall.transform.rotation);

        pos = new Vector3(x + wallOffset, wallOffset, z);
        Quaternion rot = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        Instantiate(wall, pos, rot);
    }

    //Create single tile floor
    void CreateSingleFloor(float x, float y, float z)
    {
        Vector3 pos = new Vector3(x, y, z);
        Instantiate(tile, pos, tile.transform.rotation);
    }

    //Create single tile ceiling
    void CreateSingleCeiling(float x, float y, float z)
    {
        Vector3 pos1 = new Vector3(x, 1, z);
        Quaternion rot2 = Quaternion.Euler(180.0f, 0.0f, 0.0f);
        Instantiate(ceiling, pos1, ceiling.transform.rotation);
    }

    //Create wall to the top of the space
    void CreateWallWidthTop(float x, float y, float z, float wallOffset)
    {
        Vector3 pos = new Vector3(x - wallOffset, wallOffset, z);
        Quaternion rot = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        Instantiate(wall, pos, rot);
    }

    //Create wall to the bottom of the space
    void CreateWallWidthBottom(float x, float y, float z, float wallOffset)
    {
        Vector3 pos = new Vector3(x + wallOffset, wallOffset, z);
        Quaternion rot = Quaternion.Euler(0.0f, 90.0f, 0.0f);
        Instantiate(wall, pos, rot);
    }

    //Create left wall
    void CreateWallLengthLeft(float x, float y, float z, float wallOffset)
    {
        Vector3 pos2 = new Vector3(x, wallOffset, z - wallOffset);
        Instantiate(wall, pos2, wall.transform.rotation);
    }

    //Create right wall
    void CreateWallLengthRight(float x, float y, float z, float wallOffset)
    {
        Vector3 pos = new Vector3(x, wallOffset, z + wallOffset);
        Instantiate(wall, pos, wall.transform.rotation);
    }
}