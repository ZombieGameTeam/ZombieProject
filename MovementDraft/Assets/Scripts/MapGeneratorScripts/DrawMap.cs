using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawMap : MonoBehaviour {

    private int sizeOfTile = 10;
   
    private Map map = null;
    private Vector2 size = new Vector2(50, 50);
    private MapGenerator mapGenerator = null;


    public List<GameObject> walls, floors;

    private int angle = 0;

    void Awake() {


        List<Room> rooms = new List<Room>();
        for (int i = 0; i < 10; i++)
            rooms.Add(new MediumRoom(Vector2.zero));
        
        mapGenerator = new MapGenerator(size, rooms);

        map = mapGenerator.getMap();
        this.drawMap();

    }

    public Vector3 getStartPosition() {
        return mapGenerator.getStartPosition();
    }

    private void drawMap() {
        if (map == null)
        {
            Debug.Log("map variable is null");
            return;
        }
        
        
        for (int i = 0; i < size.x; i++) {
        
            for (int j = 0; j < size.y; j++) {
        
                if (map[i, j] == Cell.Block){
        
                    GameObject plane = Instantiate(floors[0], Vector3.zero, Quaternion.identity) as GameObject;
                    plane.transform.position = new Vector3(i * sizeOfTile, 0, j * sizeOfTile);
                    plane.transform.localScale = new Vector3(sizeOfTile / 10, 1f, sizeOfTile / 10);
                    plane.transform.parent = gameObject.transform;
                    drawWalls(new Vector2(i, j));
                    
                }
            }
        }
    
    }
    private void drawWalls(Vector2 poz) {
        GameObject wall = null;
        int typeOfWall = 0;
        float wallWidth = sizeOfTile / 10f;
        float wallHeight = sizeOfTile / 2f;
        int x, y;
        x = (int)poz.x - 1;
        y = (int)poz.y;
        if (this.map[x, y] != Cell.Block) {
            wall = Instantiate(walls[typeOfWall], Vector3.zero, Quaternion.identity) as GameObject;
            wall.transform.localScale = new Vector3(wallWidth, wallHeight, sizeOfTile);
            wall.transform.position = new Vector3(x * sizeOfTile + sizeOfTile / 2, wallHeight / 2 + 0.5f, y * sizeOfTile);
            wall.transform.parent = gameObject.transform;
        }
        x = (int)poz.x + 1;
        y = (int)poz.y;
        if (this.map[x, y] != Cell.Block) {
            wall = Instantiate(walls[typeOfWall], Vector3.zero, Quaternion.identity) as GameObject;
            wall.transform.localScale = new Vector3(wallWidth, wallHeight, sizeOfTile);
            wall.transform.position = new Vector3(x * sizeOfTile - sizeOfTile / 2, wallHeight / 2 + 0.5f, y * sizeOfTile);
            wall.transform.parent = gameObject.transform;
        }
        x = (int)poz.x;
        y = (int)poz.y - 1;
        if (this.map[x, y] != Cell.Block) {
            wall = Instantiate(walls[typeOfWall], Vector3.zero, Quaternion.identity) as GameObject;
            wall.transform.localScale = new Vector3(wallWidth, wallHeight, sizeOfTile);
            wall.transform.rotation = Quaternion.Euler(0, 90, 0);
            wall.transform.position = new Vector3(x * sizeOfTile, wallHeight / 2 + 0.5f, y * sizeOfTile + sizeOfTile / 2);
            wall.transform.parent = gameObject.transform;
        }
        x = (int)poz.x;
        y = (int)poz.y + 1;
        if (this.map[x, y] != Cell.Block) {
            wall = Instantiate(walls[typeOfWall], Vector3.zero, Quaternion.identity) as GameObject;
            wall.transform.localScale = new Vector3(wallWidth, wallHeight, sizeOfTile);
            wall.transform.rotation = Quaternion.Euler(0, 90, 0);
            wall.transform.position = new Vector3(x * sizeOfTile, wallHeight / 2 + 0.5f, y * sizeOfTile - sizeOfTile / 2);
            wall.transform.parent = gameObject.transform;
        }
    }


}
