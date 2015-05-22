using System.Collections.Generic;
using System.Collections;
using UnityEngine;

class MapGenerator{

    private List<List<Cell>> matrix;
    private List<Room> rooms;
    private Map map = null;
    private bool[,] visited;
    private List<Vector2> neighboursList = null;
    private Vector2 initialPosition = Vector2.zero;


    public MapGenerator(Vector2 size, List<Room> rooms = null) {
        this.rooms = rooms;


        map = new Map(size, rooms);
        this.generateMap();
        
      
        foreach (Room room in rooms)
        {
            Vector2 bridge = makeBridgeBetweenRoomAndMap(room.position, room.size);
            map[(int)bridge.x, (int)bridge.y] = Cell.Block;
        }

        this.makeDungeonNicer();
        this.makeDungeonNicer();

    }

    private void makeDungeonNicer() {
        visited = new bool[(int)map.size.x, (int)map.size.y];
        for (int i = 0; i < map.size.x; i++)
        {
            for (int j = 0; j < map.size.y; j++)
            {
                visited[i, j] = false;
            }
        }
        this.visited[(int)initialPosition.x, (int)initialPosition.y] = true;
        this.map[(int)this.initialPosition.x, (int)this.initialPosition.y] = Cell.Block;
        this.DFS(initialPosition);

        this.reduceMap(1);
    }


    private Vector2 makeBridgeBetweenRoomAndMap(Vector2 poz, Vector2 size)
    {

        Vector2 bridge = checkNeightbours(new Vector2(poz.x - 1, poz.y - 1), new Vector2(size.x + 2, size.y + 2));
        if (bridge == new Vector2(-1, -1))
        {
            bridge = makeBridgeBetweenRoomAndMap(new Vector2(poz.x - 1, poz.y - 1), new Vector2(size.x + 2, size.y + 2));
        }

        Vector2 dir = new Vector2(poz.x + size.x / 2, poz.y + size.y / 2) - bridge;
		dir = dir.normalized;
        Vector2 newPoz = Vector2.zero;
        float cos = Vector2.Dot(dir, new Vector2(1, 0));
        float delta = Mathf.Acos(cos) * Mathf.Rad2Deg;


        if ((delta >= 0 && delta < 45) || (delta < 360 && delta >= 315)){ 
            newPoz = new Vector2((int)bridge.x + 1, (int)bridge.y);
        } else if (delta >= 45 && delta < 135){
            newPoz = new Vector2((int)bridge.x, (int)bridge.y + 1);
        }
        else if (delta >= 135 && delta < 225)
        {
            newPoz = new Vector2((int)bridge.x - 1, (int)bridge.y);
        }
        else {
            newPoz = new Vector2((int)bridge.x, (int)bridge.y - 1);
        }

        map[(int)newPoz.x, (int)newPoz.y] = Cell.Block;


        if (Mathf.Abs(newPoz.x - bridge.x) + Mathf.Abs(newPoz.y - bridge.y) == 2)
        {
            int rand = Random.Range(0, 1);
            if (rand == 0)
            {
                bridge = new Vector2(newPoz.x, bridge.y);
            }
            else
            {
                bridge = new Vector2(bridge.x, newPoz.y);
            }
            map[(int)bridge.x, (int)bridge.y] = Cell.Block;
        }
        bridge = newPoz;

        return bridge;
    }
    private Vector2 checkNeightbours(Vector2 poz, Vector2 size)
    {
        Vector2 bridge = new Vector2(-1 , -1);

        List<Vector2> list = new List<Vector2>();
        for (int i = 0; i < (int)(size.x); i++)
        {
            if (map[(int)poz.x + i, (int)poz.y] == Cell.Block)
            {
                list.Add(new Vector2(poz.x + i, poz.y));
            }
            else if (map[(int)poz.x + i, (int)(poz.y + size.y) - 1] == Cell.Block)
            {
                list.Add(new Vector2((int)poz.x + i, (int)(poz.y + size.y) - 1));
            }
        }

        for (int j = 0; j < (int)(size.y); j++)
        {
            if (map[(int)poz.x, j + (int)poz.y] == Cell.Block)
            {
                list.Add(new Vector2((int)poz.x, j + (int)poz.y));
            }
            else if (map[(int)(poz.x + size.x) - 1, j + (int)poz.y] == Cell.Block)
            {
                list.Add(new Vector2((int)(poz.x + size.x) - 1, j + (int)poz.y));
            }
        }

        if (list.Count == 0)
            return bridge;


        int rand = Random.Range(0, list.Count - 1);

        return list[rand];
    }


    private void DFS(Vector2 v) {
        List<Vector2> neighbours = getNeighbours(v, false);
        bool isBridge = false;


        foreach (Vector2 x in neighbours)
        {
            this.visited[(int)x.x, (int)x.y] = true;
        }


        for (int i = neighbours.Count - 1; i >= 0; i--)
        {
            if (this.isXInRooms(neighbours[i]))
            {
                isBridge = true;
                neighbours.RemoveAt(i);
            }
        }

        foreach (Vector2 x in neighbours) {
            DFS(x);
        }

        if (isBridge == true) {
            return;
        }
        if (neighbours.Count > 0)
        {
            neighbours = getNeighbours(v, true);
            if (neighbours.Count <= 1)
            {
                this.map[(int)v.x, (int)v.y] = Cell.None;
            }
        }
        else {
            this.map[(int)v.x, (int)v.y] = Cell.None;
        }

        return;
    }


    private List<Vector2> getNeighbours(Vector2 poz, bool visitedStatus){
        List<Vector2> list = new List<Vector2>();
        int x, y;
        x = (int)poz.x - 1;
        y = (int)poz.y;
        if (x >= 0 && this.map[x, y] == Cell.Block && this.visited[x, y] == visitedStatus) 
        {
            list.Add(new Vector2(x, y));
        }
        x = (int)poz.x + 1;

        if (x >= 0 && this.map[x, y] == Cell.Block && this.visited[x, y] == visitedStatus) 
        {
            list.Add(new Vector2(x, y));
        }
        x = (int)poz.x;
        y = (int)poz.y - 1;
        if (x >= 0 && this.map[x, y] == Cell.Block && this.visited[x, y] == visitedStatus) 
        {
            list.Add(new Vector2(x, y));
        }
        y = (int)poz.y + 1;
        if (x >= 0 && this.map[x, y] == Cell.Block && this.visited[x, y] == visitedStatus) 
        {
            list.Add(new Vector2(x, y));
        }

        return list;
    }

    private bool isXInRooms(Vector2 v) {
        bool ok = false;
        foreach(Room room in rooms){
            if (room.position.x <= v.x && v.x < room.position.x + room.size.x) {
                if (room.position.y <= v.y && v.y < room.position.y + room.size.y)
                {
                    ok = true;
                    break;
                }
                    
            }
        }
        return ok;
    }

	private void reduceMap(int k1){
        
            for (int k = 0; k < k1; k++ )
                for (int i = 0; i < this.map.size.x; i++)
                {
                    for (int j = 0; j < this.map.size.y; j++)
                    {
                        int count = countNeighbours(new Vector2(i, j));
                        if (count == 1 && i != (int)this.initialPosition.x && j != this.initialPosition.y) 
                        {
                            this.map[(int)i, (int)j] = Cell.None;

                        }
                    }
                }
            
        
	}

	private int countNeighbours(Vector2 poz){
		int count = 0;
		if (poz.x - 1 >= 0 && this.map[(int)poz.x - 1, (int)poz.y] == Cell.Block) {
			count += 1;
		}
		if (poz.x + 1 < this.map.size.x && this.map[(int)poz.x + 1, (int)poz.y] == Cell.Block) {
			count += 1;
		}
		if (poz.y - 1 >= 0 && this.map[(int)poz.x, (int)poz.y - 1] == Cell.Block) {
			count += 1;
		}
		if (poz.y + 1 < this.map.size.y && this.map[(int)poz.x, (int)poz.y + 1] == Cell.Block) {
			count += 1;
		}

        return count;
	}

    private void generateMap(){
        //Vector2 currentPoz = new Vector2(this.map.size.x - 1, this.map.size.y - 1);
        Vector2 currentPoz = new Vector2(0, 0);
        
        for (int i = 0; i < map.size.x; i++)
            for (int j = 0; j < map.size.y; j++) {
                if (this.map[i, j] == Cell.None) {
                    currentPoz = new Vector2(i, j);
                    break;
                }
            }
        
        initialPosition = currentPoz;

        this.map[0, 0] = Cell.Block;
        neighboursList = new List<Vector2>();
        markNeighbours(currentPoz);
        while (neighboursList.Count > 0){
            currentPoz = chooseNeighbours();
            markNeighbours(currentPoz);
        }
    }

    private Vector2 chooseNeighbours() {

        int n = Random.Range(0, neighboursList.Count - 1);
        Vector2 neighbour = neighboursList[n];
        this.map[(int)neighbour.x, (int)neighbour.y] = Cell.Block;
        this.neighboursList.Remove(neighbour);
        return neighbour;
    }

    private void markNeighbours(Vector2 poz) {
        if (poz.x - 1 >= 0) {
            changeCellInMatrix(new Vector2(poz.x - 1, poz.y));
        }
        if (poz.x + 1 < this.map.size.x) {
            changeCellInMatrix(new Vector2(poz.x + 1, poz.y));
        }
        if (poz.y - 1 >= 0) {
            changeCellInMatrix(new Vector2(poz.x, poz.y - 1));
        }
        if (poz.y + 1 < this.map.size.y) {
            changeCellInMatrix(new Vector2(poz.x, poz.y + 1));
        }
    }

    private void changeCellInMatrix(Vector2 v) {
           
        if (this.map[(int)v.x, (int)v.y] == Cell.Avalible) {
            this.map[(int)v.x, (int)v.y] = Cell.Unavalible;
			this.neighboursList.Remove(v);
        }else if (this.map[(int)v.x, (int)v.y] == Cell.None) {
            this.neighboursList.Add(v);
            this.map[(int)v.x, (int)v.y] = Cell.Avalible;
        }

        
    }

    public Map getMap() {
        if (map == null) {
            Debug.Log("MapGenerator just returned a null map");
        }
        return map;
    }

    public Vector2 getStartPosition() {
        return this.initialPosition;
    }

}
