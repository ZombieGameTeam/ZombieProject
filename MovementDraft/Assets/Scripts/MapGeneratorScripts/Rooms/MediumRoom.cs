using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class MediumRoom: Room{

	private List<Object> objects = new List<Object>();
	public MediumRoom(Vector2 position){
		this.position = position;
        this.size = new Vector2(4, 4);
	}
	
	public override void addObject(Object obj){
		objects.Add (obj);
	}

    public override void removeObject(Object obj)
    {
		objects.Remove (obj);
	}
}
