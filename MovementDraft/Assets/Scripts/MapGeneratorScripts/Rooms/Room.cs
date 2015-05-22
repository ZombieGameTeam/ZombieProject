using UnityEngine;
using System.Collections;

public abstract class Room
{
    public Vector2 size;
    public Vector2 position;
	public abstract void addObject(Object obj);
	public abstract void removeObject(Object obj);
}