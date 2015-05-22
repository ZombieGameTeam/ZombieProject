using System.Collections.Generic;
using System.Collections;
using UnityEngine;

class Map
{

    private List<List<Cell>> matrix;
    private List<Room> rooms;

    public Vector2 size;

    public Cell this[int i, int j] {
        get {
            if (0 <= i && i < this.size.x && 0 <= j && j < this.size.y)
                return this.matrix[i][j];
            return Cell.Null;
        }
        set {
            if (0 <= i && i < this.size.x && 0 <= j && j < this.size.y)
                this.matrix[i][j] = value;
        }
    }

    public Map(Vector2 size, List<Room> rooms) {
        this.size = size;
        this.rooms = rooms;

        RoomsPlacer roomPlacer = new RoomsPlacer(rooms, this.size);

        this.createMap();
        this.addRoomsToMap();        
        // TODO: trebuie sa unesc labirintul cu camerele
        
        // return;
       
        
    }


    private void addRoomsToMap(){
         // TODO: de marcat celulele camerelor
        foreach (Room room in rooms)
        {
            // Debug.Log("Mark room");
            markRoom(room);
        }
        
    }

    private void markRoom(Room room) {

        Vector2 poz = room.position;
        
        for (int i = 0; i < room.size.x; i++)
            for (int j = 0; j < room.size.y; j++)
            {
                if (poz.x + i < this.size.x && j + poz.y < this.size.y && i + poz.x >= 0 && j + poz.y >= 0){
					matrix[i + (int)poz.x][j + (int)poz.y] = Cell.Block;
                	markNeightbours(new Vector2(i + (int)poz.x, j + (int)poz.y));
				}
            }
    }

    private void markNeightbours(Vector2 poz) {
        int i = (int)poz.x;
        int j = (int)poz.y;

        if (poz.x - 1 >= 0 && matrix[i - 1][j] != Cell.Block)
        {
            matrix[i - 1][j] = Cell.Avalible;
        }
        if (poz.x + 1 < size.x && matrix[i + 1][j] != Cell.Block)
        {
            matrix[i + 1][j] = Cell.Avalible;
        }
        if (poz.y - 1 >= 0 && matrix[i][j - 1] != Cell.Block)
        {
            matrix[i][j - 1] = Cell.Avalible;
        }
        if (poz.y + 1 < size.y && matrix[i][j + 1] != Cell.Block)
        {
            matrix[i][j + 1] = Cell.Avalible;
        }
    }

    private void createMap(){
        matrix = new List<List<Cell>>();

        for (int i = 0; i < size.x; i++)
        {

            // add elements to row
            List<Cell> list = new List<Cell>();
            for (int j = 0; j < size.y; j++)
            {
                list.Add(Cell.None);
            }

            // add row to matrix
            matrix.Add(list);

        }
        
    }


}
