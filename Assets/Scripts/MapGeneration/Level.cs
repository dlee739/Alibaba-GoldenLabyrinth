using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static float Height = 500;
    public static float Width = 500;

    public static float Scale = 1f;
    public static float IconScale = 0.07f;
    public static float padding = .01f;

    public static float RoomGenerationChance = .5f; // Chance of a room being generated

    // public static int RoomLimit = 6; // Maximum number of rooms
    // in the fifth video it is changed to 5
    public static int RoomLimit = 5;


    public static Sprite TreasureRoomIcon;
    public static Sprite BossRoomIcon;
    public static Sprite ShopRoomIcon;
    public static Sprite UnexploredRoom;
    public static Sprite DefaultRoomIcon;
    public static Sprite CurrentRoomIcon;

    public static List<Room> Rooms = new List<Room>();
    public static Room CurrentRoom;

}

public class Room
{
    public int RoomNumber = 6;
    public Vector2 Location;
    public Sprite RoomImage;

}
