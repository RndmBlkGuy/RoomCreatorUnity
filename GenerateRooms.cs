using UnityEngine;
using UnityEditor;

public class GenerateRooms : EditorWindow
{
    [SerializeField] private GameObject tile;
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject ceiling;

    [SerializeField] private int roomWidth;

    [SerializeField] private int roomLength;

    [SerializeField] private float wallEdgePlacement;

    [MenuItem("Tools/Generate Rooms")]
    static void CreateGenerateRooms()
    {
        EditorWindow.GetWindow<GenerateRooms>();
    }
    private void OnGUI()
    {
        tile = (GameObject)EditorGUILayout.ObjectField("Prefab tile", tile, typeof(GameObject), false);
        wall = (GameObject)EditorGUILayout.ObjectField("Prefab wall", wall, typeof(GameObject), false);
        ceiling = (GameObject)EditorGUILayout.ObjectField("Prefab Ceiling", ceiling, typeof(GameObject), false);

        

         //EditorGUILayout.LabelField("width: ", roomWidth );
         

        roomWidth = EditorGUILayout.IntField("Room Width", roomWidth);
        roomLength = EditorGUILayout.IntField("Room length", roomLength);

        wallEdgePlacement = EditorGUILayout.FloatField("Place Wall on edge pos", wallEdgePlacement );

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
                        Vector3 pos = new Vector3(i, 0, j);
                        Instantiate(tile, pos, tile.transform.rotation);

                        Vector3 pos1 = new Vector3(i, 1, j);
                        Quaternion rot2 = Quaternion.Euler(180.0f, 0.0f, 0.0f);
                        Instantiate(ceiling, pos1, ceiling.transform.rotation);

                        //If corner, create two walls to make corner
                        //if((i == 0)|| (j == 0)){
                            CreateCorner(i, j, roomWidth, roomLength);
                        //}

                        //If roomLength is larger that 2 but on edge, create walls on the long side edges
                        //else if((i > 0) || (i == roomLength)){
                            Debug.Log("Printing Wall Length: " + i);
                            CreateWallsLength(i, j, roomWidth, roomLength);
                        //}

                        //if roomWidth is larger than 2, create walls on the horizontial edges
                        //else if((j > 0) || (j <= roomLength)){
                            Debug.Log("Printing Wall Width: " + j);
                            CreateWallsWidth(i, j, roomWidth, roomLength);
                            
                       // }
                    }

                }
            }
         
        }
        GUI.enabled = false;
        //EditorGUILayout.LabelField("Selection count: " + Selection.objects.Length);
    }

//create corner
    void CreateCorner(int i, int j, float wallSideW, float wallSideL){
        if (((i == 0) && (j == 0)) )
        {
            Vector3 pos2 = new Vector3(0, i + wallEdgePlacement, j - wallEdgePlacement);
            Instantiate(wall, pos2, wall.transform.rotation);

            Vector3 pos3 = new Vector3(i - wallEdgePlacement, j + wallEdgePlacement, 0);
            Quaternion rot2 = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            Instantiate(wall, pos3, rot2);
        }

        if ((i == 0) && (j == roomWidth))
        {
            Vector3 pos2 = new Vector3(0, i + wallEdgePlacement, j + wallEdgePlacement);
            Instantiate(wall, pos2, wall.transform.rotation);

            Vector3 pos3 = new Vector3(i - wallEdgePlacement, wallEdgePlacement, j);
            Quaternion rot2 = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            Instantiate(wall, pos3, rot2);

        }

        if ((i == wallSideL) && (j == 0)){
            Vector3 pos2 = new Vector3(i, wallEdgePlacement, j - wallEdgePlacement);
            Instantiate(wall, pos2, wall.transform.rotation);

            Vector3 pos3 = new Vector3(i+wallEdgePlacement, wallEdgePlacement, j);
            Quaternion rot2 = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            Instantiate(wall, pos3, rot2);
        }

        if ((i == wallSideL) && (j == wallSideW)){
            Vector3 pos2 = new Vector3(i, wallEdgePlacement, j + wallEdgePlacement);
            Instantiate(wall, pos2, wall.transform.rotation);

            Vector3 pos3 = new Vector3(i+wallEdgePlacement, wallEdgePlacement, j);
            Quaternion rot2 = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            Instantiate(wall, pos3, rot2);
        }

    }
    //Width = Z direction
    void CreateWallsWidth(int i, int j, float wallSideW, float wallSideL){


//top
        if ((i == 0) && ((j > 0) && (j < wallSideW)))
        {

            Vector3 pos3 = new Vector3(i - wallEdgePlacement, wallEdgePlacement, j);
            Quaternion rot2 = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            Instantiate(wall, pos3, rot2);

        }
//bottom
        else if ((i == wallSideL) && ((j > 0) && (j < wallSideW)))
        {

            Vector3 pos3 = new Vector3(i + wallEdgePlacement, wallEdgePlacement, j);
            Quaternion rot2 = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            Instantiate(wall, pos3, rot2);

        }



    }

//Length = x direction
    void CreateWallsLength(int i, int j, float wallSideW, float wallSideL){


        if (((i > 0) && (i < wallSideL)) && ((j == 0) || (j == wallSideW)))
        {

            if ((j == wallSideW))
            {
                Vector3 pos1 = new Vector3(i, wallEdgePlacement, j + wallEdgePlacement);
                Instantiate(wall, pos1, wall.transform.rotation);
            }
            else
            {
                Vector3 pos2 = new Vector3(i, wallEdgePlacement, j - wallEdgePlacement);
                Instantiate(wall, pos2, wall.transform.rotation);
            }


        }

    }
}