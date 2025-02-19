using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Sprite CurrentRoom;
    public Sprite BRoom;
    public Sprite EmptyRoom;
    public Sprite ShopRoom;
    public Sprite TreasureRoom;
    public Sprite UnexploredRoom;


    void DrawRoomOnMap(Room R)
    {
        GameObject MapTile = new GameObject("MapTile");
        Image RoomImage = MapTile.AddComponent<Image>();
        RoomImage.sprite = R.RoomImage;
        RectTransform rectTransform = RoomImage.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(Level.Height, Level.Width) * Level.IconScale;
        rectTransform.position = R.Location * (Level.IconScale * Level.Height * Level.Scale + (Level.padding * Level.Height * Level.Scale));
        RoomImage.transform.SetParent(transform, false);

        Level.Rooms.Add(R);
    }

    bool CheckIfRoomExists(Vector2 v)
    {
        return(Level.Rooms.Exists(x => x.Location == v));
    }

    bool CheckIfRoomsAroundGeneratedRoom(Vector2 v, string direction)
    {
        switch (direction)
        {
            case "Right":
                {
                    // Check down, left and up
                    if (Level.Rooms.Exists(x => x.Location == new Vector2(v.x - 1, v.y)) || // Left
                        Level.Rooms.Exists(x => x.Location == new Vector2(v.x, v.y - 1)) || // Down
                        Level.Rooms.Exists(x => x.Location == new Vector2(v.x, v.y + 1))) // Up
                    {
                        return true;
                    }

                    break;
                }

            case "Left":
                {
                    // Check down, right and up
                    if (Level.Rooms.Exists(x => x.Location == new Vector2(v.x + 1, v.y)) || // Right
                        Level.Rooms.Exists(x => x.Location == new Vector2(v.x, v.y - 1)) || // Down
                        Level.Rooms.Exists(x => x.Location == new Vector2(v.x, v.y + 1))) // Up
                    {
                        return true;
                    }
                    break;
                }

            case "Up":
                {
                    // Check down, right and left
                    if (Level.Rooms.Exists(x => x.Location == new Vector2(v.x + 1, v.y)) || // Right
                        Level.Rooms.Exists(x => x.Location == new Vector2(v.x - 1, v.y)) || // Left
                        Level.Rooms.Exists(x => x.Location == new Vector2(v.x, v.y - 1))) // Down
                    {
                        return true;
                    }
                    break;
                }

            case "Down":
                {
                    // Check up, right and left
                    if (Level.Rooms.Exists(x => x.Location == new Vector2(v.x + 1, v.y)) || // Right
                        Level.Rooms.Exists(x => x.Location == new Vector2(v.x - 1, v.y)) || // Left
                        Level.Rooms.Exists(x => x.Location == new Vector2(v.x, v.y + 1))) // Up
                    {
                        return true;
                    }
                    break;
                }

        }

        return false;
    }

    int failsafe = 0;
    void Generate(Room room)
    {
        failsafe++;
        if (failsafe > 50) // Failsafe to prevent infinite loop
        {
            return;
        }

        DrawRoomOnMap(room); // Draw the room on the map

        // Left
        if (Random.value > Level.RoomGenerationChance) // Check if a room should be generated
        {
            Room newRoom = new Room(); // Create a new room
            newRoom.Location = new Vector2(-1, 0) + room.Location; //   Set the location of the new room
            newRoom.RoomImage = Level.DefaultRoomIcon; // Set the image of the new room

            if (!CheckIfRoomExists(newRoom.Location)) // Check if the room already exists
            {
                if (!CheckIfRoomsAroundGeneratedRoom(newRoom.Location, "Right")) // Check if there is a room to the right of the new room
                {
                    if (Mathf.Abs(newRoom.Location.x) < Level.RoomLimit && Mathf.Abs(newRoom.Location.y) < Level.RoomLimit) // Check if the new room is within the room limit
                    {
                        Generate(newRoom);
                    }
                }
            }

        }

        // Right
        if (Random.value > Level.RoomGenerationChance)
        {
            Room newRoom = new Room();
            newRoom.Location = new Vector2(1, 0) + room.Location;
            newRoom.RoomImage = Level.DefaultRoomIcon;

            if (!CheckIfRoomExists(newRoom.Location))
            {
                if (!CheckIfRoomsAroundGeneratedRoom(newRoom.Location, "Left"))
                {
                    if (Mathf.Abs(newRoom.Location.x) < Level.RoomLimit && Mathf.Abs(newRoom.Location.y) < Level.RoomLimit) // Check if the new room is within the room limit
                    {
                        Generate(newRoom);
                    }
                }
            }

        }

        // Up
        if (Random.value > Level.RoomGenerationChance)
        {
            Room newRoom = new Room();
            newRoom.Location = new Vector2(0, 1) + room.Location;
            newRoom.RoomImage = Level.DefaultRoomIcon;

            if (!CheckIfRoomExists(newRoom.Location))
            {
                if (!CheckIfRoomsAroundGeneratedRoom(newRoom.Location, "Down"))
                {
                    if (Mathf.Abs(newRoom.Location.x) < Level.RoomLimit && Mathf.Abs(newRoom.Location.y) < Level.RoomLimit) // Check if the new room is within the room limit
                    {
                        Generate(newRoom);
                    }
                }
            }
        }

        // Down
        if (Random.value > Level.RoomGenerationChance)
        {
            Room newRoom = new Room();
            newRoom.Location = new Vector2(0, -1) + room.Location;
            newRoom.RoomImage = Level.DefaultRoomIcon;

            if (!CheckIfRoomExists(newRoom.Location))
            {
                if (!CheckIfRoomsAroundGeneratedRoom(newRoom.Location, "Up"))
                {
                    if (Mathf.Abs(newRoom.Location.x) < Level.RoomLimit && Mathf.Abs(newRoom.Location.y) < Level.RoomLimit) // Check if the new room is within the room limit
                    {
                        Generate(newRoom);
                    }
                }
            }
        }


    }

    private void Awake()
    {
        Level.DefaultRoomIcon = EmptyRoom;
        Level.BossRoomIcon = BRoom;
        Level.CurrentRoomIcon = CurrentRoom;
        Level.ShopRoomIcon = ShopRoom;
        Level.TreasureRoomIcon = TreasureRoom;
        Level.UnexploredRoom = UnexploredRoom;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Level.DefaultRoomIcon = EmptyRoom;
        //Level.BossRoomIcon = BRoom;
        //Level.CurrentRoomIcon = CurrentRoom;
        //Level.ShopRoomIcon = ShopRoom;
        //Level.TreasureRoomIcon = TreasureRoom;
        //Level.UnexploredRoom = UnexploredRoom;

        Room StartRoom = new Room();
        StartRoom.Location = new Vector2(0, 0);
        StartRoom.RoomImage = Level.CurrentRoomIcon;

        // Draw starting room
        DrawRoomOnMap(StartRoom);
        //DrawRoomOnMap(Level.UnexploredRoom, new Vector2(1, 0));

        // Left
        if (Random.value > .5f)
        {
            Room newRoom = new Room();
            newRoom.Location = new Vector2(-1, 0);
            newRoom.RoomImage = Level.DefaultRoomIcon;
            if (!CheckIfRoomsAroundGeneratedRoom(newRoom.Location, "Right"))
            {
                Generate(newRoom);
            }
        }

        // Right
        if (Random.value > .5f)
        {
            Room newRoom = new Room();
            newRoom.Location = new Vector2(1, 0);
            newRoom.RoomImage = Level.DefaultRoomIcon;
            if (!CheckIfRoomsAroundGeneratedRoom(newRoom.Location, "Left"))
            {
                Generate(newRoom);
            }
        }

        // Up
        if (Random.value > .5f)
        {
            Room newRoom = new Room();
            newRoom.Location = new Vector2(0, 1);
            newRoom.RoomImage = Level.DefaultRoomIcon;
            if (!CheckIfRoomsAroundGeneratedRoom(newRoom.Location, "Down"))
            {
                Generate(newRoom);
            }
        }

        // Down
        if (Random.value > .5f)
        {
            Room newRoom = new Room();
            newRoom.Location = new Vector2(0, -1);
            newRoom.RoomImage = Level.DefaultRoomIcon;
            if (!CheckIfRoomsAroundGeneratedRoom(newRoom.Location, "Up"))
            {
                Generate(newRoom);
            }
        }
    }


    bool regenerating = false;
    void StopRegenerating()
    {
        regenerating = false;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Tab) && !regenerating)
        {
            regenerating = true;
            Invoke(nameof(StopRegenerating), 1);
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                Destroy(child.gameObject);
            }

            Level.Rooms.Clear();

            Start();
        }
    }

}
