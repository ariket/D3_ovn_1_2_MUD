﻿

using System;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;
using static D3_ovn_1_2_MUD.Program;

namespace D3_ovn_1_2_MUD

{
    internal class Program
    {
        public class Room
        {
            int number; //ett unikt nummer på rummet
            string name; // name ett valfritt namn för rummet, t.ex.ingångsrum, 
            string presentation; // en presentationstext för rummet, t.ex.du är i ett mörkt rum med
            int north;   //nummer på nästa rum åt norr, om det inte finns något -1
            int east;   //nummer på nästa rum åt öster, om det inte finns något -1
            int south; //nummer på nästa rum åt söder, om det inte finns något -1
            int west; // nummer på nästa rum åt väster, om det inte finns något -1
            public Room(int number, string name, string presentation, int north = -1, int east = -1, int south = -1, int west = -1)
            {
                this.number = number;
                this.name = name;
                this.presentation = presentation;
                this.north = north;
                this.east = east;
                this.south = south;
                this.west = west;
            }

            public int Getnumber() { return number; }
            public int Getnorth() { return north; }
            public int Geteast() { return east; } 
            public int Getsouth() { return south; } 
            public int Getwest() { return west; }
            public void Print()
            {
                Console.WriteLine($"Du är i {name}, ett rum som har {presentation}");
                if (north != -1) Console.WriteLine($"Norrut har du en dörr");
                if (east != -1) Console.WriteLine($"Österut har du en dörr");
                if (south != -1) Console.WriteLine($"Söderut har du en dörr");
                if (west != -1) Console.WriteLine($"Västerut har du en dörr");
                Console.WriteLine("Övriga väggar är helmurade");
            }
        }
        public class Game
        {
            List<Room> rooms = new List<Room>();

            public void Start()
            {
                bool quiz = true;
                int nr = 0;
                string commando = null;

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
               
               


                Console.WriteLine("n - gå norrut, s - gå söderut, v - gå västerut, ö - gå österut");
                rooms[0].Print();
                

                do {
                    Console.Write(">  ");
                    commando = Console.ReadLine();

                    if (commando == "n" && rooms[nr].Getnorth() != -1 && quiz == true)
                    {
                        nr = rooms[nr].Getnorth();
                        rooms[nr].Print();
                        quiz = Slump();
                    }

                    else if (commando == "s" && rooms[nr].Getsouth() != -1 && quiz == true)
                    {
                        nr = rooms[nr].Getsouth();
                        rooms[nr].Print();
                        quiz = Slump();
                    }

                    else if (commando == "e" && rooms[nr].Geteast() != -1 && quiz == true || commando == "ö" && rooms[nr].Geteast() != -1 && quiz == true)
                    {
                        nr = rooms[nr].Geteast();
                        rooms[nr].Print();
                        quiz = Slump();
                    }

                    else if (commando == "v" && rooms[nr].Getwest() != -1 && quiz == true || commando == "w" && rooms[nr].Getwest() != -1 && quiz == true)
                    {
                        nr = rooms[nr].Getwest();
                        rooms[nr].Print();
                        quiz = Slump();
                    }
                    else if(commando == "n" || commando == "s" || commando == "e" || commando == "ö" || commando == "v" || commando == "w")
                    { Console.WriteLine("Du gick in i väggen!"); }

                    else if (commando == "q") { Console.WriteLine("skriv " +
                        "sluta för att avsluta programmet"); break; }

                    else { Console.WriteLine($"Okänt kommando {commando}");
                           Console.WriteLine("n - gå norrut, s - gå söderut, v - gå västerut, ö - gå österut, q - quit");
                    }
                    if (nr == 26) { stopWatch.Stop(); TimeSpan ts = stopWatch.Elapsed; Console.WriteLine($"Din tid: {ts}"); commando = "q"; break; }
                    if (quiz == false) { Console.WriteLine("Du måste svara rätt på en fråga innan du kan lämna rummet!"); quiz = StällFråga(); }
                 
                } while (commando != "q");
            }

            public int SlumpFråga()
            {Random random = new Random(); return random.Next(0,9);}

            public bool Slump()
            {Random random = new Random(); if(1== random.Next(1, 3)) return false; else return true;}

            public bool StällFråga()
            {
                bool test = false;
                do {
                    int number = SlumpFråga();
                    string[] frågan = frågor[number].Split(";"); //NYI kontrollera längden på arrayen frågor, och skicka med detta till SlumpFråga
                    Console.WriteLine($"{frågan[0]} :");
                    Console.Write($"a: {frågan[1]} \nb: {frågan[2]} \nc: {frågan[3]} \nd: {frågan[4]} \n> ");
                    if (Console.ReadLine() == frågan[5]) { Console.WriteLine($"Bra rätt svar!"); test = true; }
                    else Console.WriteLine($"Tyvärr, fel svar, försök igen");
                } while (!test);
                return test;
            }



            public void Load(String file)
            {
                String[] lines = File.ReadAllLines(file);
                foreach (String line in lines)
                {
                    string[] parts = line.Split(";");
                    rooms.Add(new Room(int.Parse(parts[0]), parts[1], parts[2], int.Parse(parts[3]), int.Parse(parts[4]), int.Parse(parts[5]), int.Parse(parts[6])));
                }
            }
            public void Save(String file)
            { }//TODO
        }
 
       
        static readonly string rootFolder = @"C:\Temp\Data\";// Default folder
        static string RoomFile = @"C:\Temp\Data\rooms.txt"; // Default file
        static string Frågor = @"C:\Temp\Data\Frågor.txt"; // Default file
        public static String[] frågor = File.ReadAllLines(Frågor);
        public static String[] command;
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, Welcome to a MUD like game ");
            Game game = new Game();

            do
            {   Console.Write("> ");
                command = Console.ReadLine().Split(' ');
                //TODO Console.WriteLine($"test {command[0]}    ");if (command.Length > 1)
                if (command[0] == "hjälp"); //TODO commandhelp();
                else if (command[0] == "ladda" || command[0] == "l")
                    game.Load(RoomFile);
                else if (command[0] == "spara")
                    game.Save(RoomFile);
                else if (command[0] == "starta" || command[0] == "s")
                    game.Start();
                else if (command[0] == "sluta");
                else Console.WriteLine($"Okänt kommando {command[0]}");
            } while (command[0] != "sluta");
                Console.WriteLine("Adjö");
        }
    }
}