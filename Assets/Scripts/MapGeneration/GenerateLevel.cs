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


    void DrawRoomOnMap(Room R) // Draw a room on the map
    {
        GameObject MapTile = new GameObject("MapTile"); //  Create a new game object
        Image RoomImage = MapTile.AddComponent<Image>(); // Add an image component to the game object
        RoomImage.sprite = R.RoomImage; // Set the sprite of the image component
        RectTransform rectTransform = RoomImage.GetComponent<RectTransform>(); // Get the rect transform of the image component
        rectTransform.sizeDelta = new Vector2(Level.Height, Level.Width) * Level.IconScale; // Set the size of the image
        rectTransform.position = R.Location * (Level.IconScale * Level.Height * Level.Scale + (Level.padding * Level.Height * Level.Scale)); // Set the position of the image
        RoomImage.transform.SetParent(transform, false); // Set the parent of the image to the map

        Level.Rooms.Add(R); // Add the room to the list of rooms
    }

    int RandomRoomNumber()
    {
        return 6; // Replace later
    }

    bool CheckIfRoomExists(Vector2 v)
    {
        return (Level.Rooms.Exists(x => x.Location == v)); // Check if a room exists at the location
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
            newRoom.RoomNumber = RandomRoomNumber(); // Set the room number

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
            newRoom.RoomNumber = RandomRoomNumber();

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
            newRoom.RoomNumber = RandomRoomNumber();

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
            newRoom.RoomNumber = RandomRoomNumber();

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

    private void GenerateBossRoom()
    {
        float MaxNumber = 0;
        Vector2 FathestRoom = Vector2.zero;

        foreach (Room R in Level.Rooms)
        {
            if (Mathf.Abs(R.Location.x) + Mathf.Abs(R.Location.y) >= MaxNumber)
            {
                MaxNumber = Mathf.Abs(R.Location.x) + Mathf.Abs(R.Location.y);
                FathestRoom = R.Location;
            }
        }

        Room BossRoom = new Room();
        BossRoom.RoomImage = Level.BossRoomIcon;
        BossRoom.RoomNumber = 1; // Vid 6 Changes

        // Left
        if (!CheckIfRoomExists(FathestRoom + new Vector2(-1, 0)))
        {
            if (!CheckIfRoomsAroundGeneratedRoom(new Vector2(-1, 0) + FathestRoom, "Right"))
            {
                BossRoom.Location = new Vector2(-1, 0) + FathestRoom;
            }
        }

        // Right
        else if (!CheckIfRoomExists(FathestRoom + new Vector2(1, 0)))
        {
            if (!CheckIfRoomsAroundGeneratedRoom(new Vector2(1, 0) + FathestRoom, "Left"))
            {
                BossRoom.Location = new Vector2(1, 0) + FathestRoom;
            }
        }

        // Up
        else if (!CheckIfRoomExists(FathestRoom + new Vector2(0, 1)))
        {
            if (!CheckIfRoomsAroundGeneratedRoom(new Vector2(0, 1) + FathestRoom, "Down"))
            {
                BossRoom.Location = new Vector2(0, 1) + FathestRoom;
            }
        }

        // Down
        else if (!CheckIfRoomExists(FathestRoom + new Vector2(0, -1)))
        {
            if (!CheckIfRoomsAroundGeneratedRoom(new Vector2(0, -1) + FathestRoom, "Up"))
            {
                BossRoom.Location = new Vector2(0, -1) + FathestRoom;
            }
        }

        DrawRoomOnMap(BossRoom);

    }

    void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        System.Random rng = new System.Random();

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

    }

    private bool GenerateSpecialRoom(Sprite MapIcon, int RoomNumber)
    {
        List<Room> ShuffledList = new List<Room>(Level.Rooms);

        Room SpecialRoom = new Room();
        SpecialRoom.RoomImage = MapIcon;
        SpecialRoom.RoomNumber = RoomNumber;
        bool FoundAvailableLocation = false; // Check if an available location has been found

        foreach (Room R in ShuffledList)
        {
            Vector2 SpecialRoomLocation = R.Location;

            if (R.RoomNumber < 6)
            {
                continue;
            }

            // Left
            if (!CheckIfRoomExists(SpecialRoomLocation + new Vector2(-1, 0)))
            {
                if (!CheckIfRoomsAroundGeneratedRoom(new Vector2(-1, 0) + SpecialRoomLocation, "Right"))
                {
                    SpecialRoom.Location = new Vector2(-1, 0) + SpecialRoomLocation;
                    FoundAvailableLocation = true;
                }
            }

            // Right
            else if (!CheckIfRoomExists(SpecialRoomLocation + new Vector2(1, 0)))
            {
                if (!CheckIfRoomsAroundGeneratedRoom(new Vector2(1, 0) + SpecialRoomLocation, "Left"))
                {
                    SpecialRoom.Location = new Vector2(1, 0) + SpecialRoomLocation;
                    FoundAvailableLocation = true;
                }
            }

            // Up
            else if (!CheckIfRoomExists(SpecialRoomLocation + new Vector2(0, 1)))
            {
                if (!CheckIfRoomsAroundGeneratedRoom(new Vector2(0, 1) + SpecialRoomLocation, "Down"))
                {
                    SpecialRoom.Location = new Vector2(0, 1) + SpecialRoomLocation;
                    FoundAvailableLocation = true;
                }
            }

            // Down
            else if (!CheckIfRoomExists(SpecialRoomLocation + new Vector2(0, -1)))
            {
                if (!CheckIfRoomsAroundGeneratedRoom(new Vector2(0, -1) + SpecialRoomLocation, "Up"))
                {
                    SpecialRoom.Location = SpecialRoomLocation + new Vector2(0, -1);
                    FoundAvailableLocation = true;
                }
            }

            if(FoundAvailableLocation)
            {
                DrawRoomOnMap(SpecialRoom);
                return true;
            }


        }

        return false;
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

        // Create starting room
        Room StartRoom = new Room(); // Create a new room
        StartRoom.Location = new Vector2(0, 0); // Set the location of the room
        StartRoom.RoomImage = Level.CurrentRoomIcon; // Set the room image
        StartRoom.RoomNumber = 0; // Set the room number

        // Draw starting room
        DrawRoomOnMap(StartRoom);
        //DrawRoomOnMap(Level.UnexploredRoom, new Vector2(1, 0));

        // Left
        if (Random.value > .5f)
        {
            Room newRoom = new Room();
            newRoom.Location = new Vector2(-1, 0);
            newRoom.RoomImage = Level.DefaultRoomIcon;
            newRoom.RoomNumber = RandomRoomNumber();
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
            newRoom.RoomNumber = RandomRoomNumber();
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
            newRoom.RoomNumber = RandomRoomNumber();
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
            newRoom.RoomNumber = RandomRoomNumber();
            if (!CheckIfRoomsAroundGeneratedRoom(newRoom.Location, "Up"))
            {
                Generate(newRoom);
            }
        }


        GenerateBossRoom(); // Generate boss room

        // ADD AS MUCH ROOM WE WANT (V6)
        bool treasure = GenerateSpecialRoom(Level.TreasureRoomIcon, 3); // Generate treasure room
        bool shop = GenerateSpecialRoom(Level.ShopRoomIcon, 2); // Generate shop room

        if (!treasure || !shop)
        {
            Regenerate();
        }
    }


    bool regenerating = false;
    void StopRegenerating()
    {
        regenerating = false;
    }

    void Regenerate()
    {
        regenerating = true;
        failsafe = 0;
        Level.Rooms.Clear(); // Clear the list of rooms
        Invoke(nameof(StopRegenerating), 1); // Stop regenerating after 1 second
        for (int i = transform.childCount - 1; i >= 0; i--) // Destroy all children of the map
        {
            Transform child = transform.GetChild(i); // Get the child
            Destroy(child.gameObject);
        }

        Start(); // Regenerate the level
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Tab) && !regenerating) // Regenerate the level
        {
            Regenerate();
        }

        // Debug
        //if (Input.GetKey(KeyCode.p) && !regenerating) // Regenerate the level
        //{
        //    regenerating = true;
        //    Invoke(nameof(StopRegenerating), 1); // Stop regenerating after 1 second

        //    for (Room R in Level.Rooms) // Destroy all children of the map
        //    {
        //        Transform child = transform.GetChild(i); // Get the child
        //        Destroy(child.gameObject);
        //    }
        //    Start(); // Regenerate the level
    }

}
