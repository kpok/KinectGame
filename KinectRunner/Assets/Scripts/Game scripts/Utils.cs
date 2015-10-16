using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Utils
{

	// Use this for initialization
	public static float PLAYER_SPEED = 6.0f;
	public static float JUMP_SPEED = 8.0f;
	public static float GRAVITY = 10.0f;
	public static float baseZ = 2.0f;
    public static float GROUND_SPEED = 0.5f;
    public static float ITEM_SPEED = -0.5f;
    public static float SPAWN_CYCLE = 0.8f;
	public static List<Field> FIELDS = new List<Field>
	{
		new Field{start = -50.0f, end = -0.3f, position = -2.5f, name = "VeryLeft"},
		new Field{start = -0.3f, end = -0.1f, position = -1.25f, name = "Left"},
		new Field{start = -0.1f, end = 0.1f, position =  0.0f, name = "Center"},
		new Field{start = 0.1f, end = 0.3f, position = 1.25f, name="Right"},
		new Field{start = 0.3f, end = 50.0f, position =  2.5f, name="VeryRight"},
	};

	public static Field DynamicRealField = 
	new Field{start = -0.4f, end = 0.4f, position =  0.0f, name = "Center"};
	public static Field DynamicGameField = 
	new Field{start = -3.0f, end = 3.0f, position =  0.0f, name = "Center"};

	public static float jumpDistance = 0.15f;
	public static float SQUAT_DISTANCE = 0.2f;
	public static float GroundPosition = 1.3f;

	public static float PlayerYPostionSquatDiff = 1.0f;
}

public class Field
{
	public float start;
	public float end;
	public float position;
	public string name;
}
