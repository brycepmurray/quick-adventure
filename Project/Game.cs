using System;
using System.Collections.Generic;

namespace CastleGrimtol.Project
{
    public class Game : IGame
    {
        public Room currentRoom { get; set; }
        public Player Player { get; set; }
        public bool Playing { get; private set; }
        public List<Room> Rooms { get; set; } = new List<Room>();

        public void InitializeGame()
        {
            Player = new Player();

            var room1 = new Room("Room 1", "An open room with a nice chandelier in the center");
            var room2 = new Room("Room 2", "The second room in this so called home.");
            var room3 = new Room("Room 3", "The Room with the use for a key in this home");
            var room4 = new Room("Room 4", "The End Game, Once you use the key you are free from here");

            room1.Items.Add(ReadyItems.Key.Name, ReadyItems.Key);

            room1.Exits.Add("forward", room2);
            room2.Exits.Add("forward", room3);
            room2.Exits.Add("back", room1);
            // room3.Exits.Add("forward", room4);
            room3.Exits.Add("back", room2);
            room4.Exits.Add("back", room3);

            Rooms.Add(room1);
            Rooms.Add(room2);
            Rooms.Add(room3);
            Rooms.Add(room4);

            currentRoom = room1;

            Console.Clear();
            System.Console.WriteLine("Welcome to the Shack");
            System.Console.WriteLine("Write 'help' at any time to be given controls");

            System.Console.WriteLine("You wake up in a shack in a very wide and open room, there isnt much around save a key sitting on a table next to you");
            Playing = true;
            showControls();
        }
        void showControls()
        {
            while (Playing)
            {
                //Quick guide
                // Room One is where the Key is
                //Room 2 is possible death when using the key  in room two you will die
                //Room three unlocks the door to Room 4
                //Room 4 is the end Room and Win

                showRoom();
                System.Console.WriteLine($@"
             ------------------------------
             You can go forward, or go back
             You can take an item, or use an item
             Or type help at any time to get these instructions,
             or enter q or quit to exit the game 
             ");
                if (Player.Inventory.ContainsKey("key"))
                {
                    System.Console.WriteLine($@"
                 4. Use Key
                 ");
                }
                System.Console.WriteLine($@"
             ------------------------------
             ");
                userChoice();
            }
            System.Console.WriteLine("Goodbye...");
        }
        void showRoom()
        {
            System.Console.WriteLine($@"
                ------------------------------------------
                            {currentRoom.Name}
                {currentRoom.Description}
                ------------------------------------------
                ");
        }
        void userChoice()
        {
            if (currentRoom.Name == "Room 4")
            {
                Win();
            }
            string choice = Console.ReadLine();
            choice = choice != null ? choice.ToLower() : "";
            if (choice == "help" || choice == "h")
            {
                System.Console.WriteLine("Help is a fickle thing...");
                return;
            }
            else if (choice == "take" || choice =="take key" || choice == "1" || choice == "take item")
            {
                TakeItem();
            }
            else if (choice == "forward" ||choice == "go forward" || choice == "2" || choice == "f")
            {
                Move("forward");
            }
            else if (choice == "back" || choice == "go back" || choice == "3" || choice == "b")
            {
                Move("back");
            }
            else if (choice == "use" || choice == "use item" || choice == "use key" || choice == "4" && Player.Inventory.ContainsKey("key"))
            {
                UseKey();
            }
            else if(choice == "q" || choice == "quit" || choice == "exit"){
                Playing = false;
            }
            else
            {
                System.Console.WriteLine("That is not a valid response, please try again");
            }

        }

        private void Move(string direction)
        {
            if (currentRoom.Exits.ContainsKey(direction))
            {
                currentRoom = currentRoom.Exits[direction];
            }
            else
            {
                System.Console.WriteLine("That is not a valid direction");
            }
        }

        private void TakeItem()
        {
            if (currentRoom.Items.ContainsKey("Key"))
            {
                Console.Clear();
                Player.Inventory.Add("key", currentRoom.Items["Key"]);
                currentRoom.Items.Remove("Key");
                System.Console.WriteLine("You take the Key");
                Player.Points++;
                System.Console.WriteLine("Points: " + Player.Points);
            }
            else
            {
                System.Console.WriteLine("There is nothing to take...");
            }
        }

        public void UseKey()
        {
            if (currentRoom.Name == "Room 2")
            {
                Playing = false;
                System.Console.WriteLine($@"
                            There is a stone pillar with a key slot on it,
                            as you insert the key you hear doors, and gears sliding into place.
                            The ground beneath you opens up and swallows you whole...
                            
                                                   YOU LOSE.
                    ");
            }
            else if (currentRoom.Name == "Room 3" && Player.Inventory.ContainsKey("key"))
            {
                if (currentRoom.Exits.ContainsKey("forward"))
                {
                    System.Console.WriteLine("You have already unlocked the door");
                    return;
                }
                System.Console.WriteLine($@"
                            There is a Large Door, and the end is on the other side,
                            The door is locked...You pull out your key and insert it into the door it was
                            made for...you hear the lock click as it unlocks
                        "); ;
                Player.Points++;
                currentRoom.Exits.Add("forward", Rooms.Find(r => r.Name == "Room 4"));
                System.Console.WriteLine("Points: " + Player.Points);
            }
        }
        public void Win()
        {
            System.Console.WriteLine("Congrats you have won the Game and made it out of the House");
            System.Console.WriteLine("Points: " + Player.Points);
            Playing = false;
        }
    }
}