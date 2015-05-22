using System.Collections.Generic;
using System.Collections;
using UnityEngine;

class RoomsPlacer {

    private List<Room> rooms = null;
    private Vector2 mapSize;

    public RoomsPlacer(List<Room> rooms, Vector2 mapSize) {
        this.rooms = rooms;
        this.mapSize = mapSize;
        
        this.findAPlaceForEveryRoom();
    }

    private void findAPlaceForEveryRoom() {
        float omega = 360f / this.rooms.Count;
        float longR = mapSize.x / 2f;
        float smallR = mapSize.y / 2f;
        Vector2 origin = new Vector2(longR, smallR);


        for (float r = 0f, k = 0; r < 360f; r += omega, k += 1) {
            Vector2 vel = new Vector2(longR * Mathf.Cos(r * Mathf.Rad2Deg), smallR * Mathf.Sin(r * Mathf.Rad2Deg));
            vel = vel.normalized;


            for (int i = 0; i < 4; i++) {
				float rand;
                do
                {
                    rand = Random.Range(0, Mathf.Max(longR, smallR));
                } while (Mathf.Abs(origin.x + rand * vel.x) > mapSize.x || Mathf.Abs(origin.y + rand * vel.y) > mapSize.y);

                if (this.checkIfRoomFits(this.rooms[(int)k], origin + rand * vel)) {
                    this.placeRoom(this.rooms[(int)k], origin + rand * vel);
                    break;
                }

            }
        }

    }

    private void placeRoom(Room room, Vector2 poz)
    {
        room.position = new Vector2((int)poz.x - room.size.x / 2, (int)poz.y - room.size.y / 2);
    }

    private bool checkIfRoomFits(Room room, Vector2 poz) {
		Vector2 pozRoom = new Vector2(poz.x - room.size.x / 2, poz.y -  room.size.y / 2);

        if (pozRoom.x < 0 || pozRoom.y < 0) {
            return false;
        }
        if (pozRoom.x >= mapSize.x || pozRoom.y >= mapSize.y) {
            return false;
        }

        if (checkCollisionWithOtherRooms(room, poz)) {
            return false;
        }

        return true;
    }


    private bool checkCollisionWithOtherRooms(Room r1, Vector2 poz) {
        Rect r11 = new Rect(poz.x - r1.size.x / 2 - 2, poz.y - r1.size.y / 2 - 2, r1.size.x + 4, r1.size.y + 4);
        foreach (Room r2 in rooms) {
			if (r2.Equals(r1))
				continue;
            Rect r22 = new Rect(r2.position.x - 2, r2.position.y - 2, r2.size.x + 4, r2.size.y + 4);
            if (r11.Overlaps(r22)) {
                return true;
            }
        }


        return false;
    }
}