using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleShipLite_Library;
using BattleShipLite_Library.Models;

namespace BattleShipLite_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WelcomeMessage();

            PlayerInfoModel activePlayer = CreatePlayer("Player 1");
            PlayerInfoModel opponent = CreatePlayer("Player 2");
            PlayerInfoModel winner = null;

            do
            {


                // Determine is the game is over or not.
                // If over, set the activePlayer else switch the players

                // Display the grid from the activePlayer
                DisplayShotGrid(activePlayer);

                // Ask the activePlayer for a shot (determine if the shot is valid. If not ask again).
                // Determine the shot results. (You would need activePlayer (shot) and opponent (ship) to determine the shot results).
                RecordPlayerShot(activePlayer, opponent);

                bool doesGameContinue = GameLogic.PlayerStillActive(opponent);

                if (doesGameContinue == true)
                {
                    (activePlayer, opponent) = (opponent, activePlayer);
                }
                else
                {
                    winner = activePlayer;
                }

            } while (winner == null);

            IdentifyWInner(winner);

            Console.ReadLine();
        }

        private static void IdentifyWInner(PlayerInfoModel winner)
        {
            Console.WriteLine($"Congratulations to {winner.UsersName} for winning!");
            Console.WriteLine($"{winner.UsersName} took {GameLogic.GetShotCount(winner)} shots.");
        }

        private static void RecordPlayerShot(PlayerInfoModel activePlayer, PlayerInfoModel opponent)
        {
            bool isValidShot = false;

            string row = "";
            int column = 0;

            do
            {
                string shot = AskForShot(activePlayer);

                try
                {
                    (row, column) = GameLogic.SplitShotIntoRowsAndColumn(shot);
                    isValidShot = GameLogic.ValidateShot(activePlayer, row, column);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    isValidShot = false;
                }
                
                
                if (isValidShot == false)
                {
                    Console.WriteLine("Invalid shot location. Please try again.");
                }

            } while (isValidShot == false);

            //Determine the shot results
            bool isAHit = GameLogic.IdentifyShotResult(opponent, row, column);

            //Update the ShotGridLocations for the activePlayer. Since he just played the shot.
            GameLogic.MarkShotResult(activePlayer, row, column, isAHit);

            DisplayShotResults(row, column, isAHit);
        }

        private static void DisplayShotResults(string row, int column, bool isAHit)
        {
            if (isAHit)
            {
                Console.WriteLine($"{row}{column} is a Hit.");
            }
            else
            {
                Console.WriteLine($"{row}{column} is a Miss.");
            }
            Console.WriteLine();
        }

        private static string AskForShot(PlayerInfoModel activePlayer)
        {
            string shot;
            Console.Write($"{activePlayer.UsersName} please enter your shot selection: ");
            shot = Console.ReadLine();
            return shot;
        }

        private static void DisplayShotGrid(PlayerInfoModel activePlayer)
        {
            Console.WriteLine($"{activePlayer.UsersName} Grid:");
            string currentRow = activePlayer.ShotGridLocations[0].SpotLetter;

            foreach (var ShotGridLocation in activePlayer.ShotGridLocations)
            {
                if (currentRow != ShotGridLocation.SpotLetter)
                {
                    Console.WriteLine();
                    currentRow = ShotGridLocation.SpotLetter;
                }

                if (ShotGridLocation.Status == GridSpotStatus.Empty )
                {
                    Console.Write($" {ShotGridLocation.SpotLetter}{ShotGridLocation.SpotNumber} ");
                }
                else if (ShotGridLocation.Status == GridSpotStatus.Hit)
                {
                    Console.Write($" XX ");
                }
                else if (ShotGridLocation.Status == GridSpotStatus.Miss)
                {
                    Console.Write($" OO ");
                }
                else
                {
                    Console.WriteLine(" ? ");
                }

            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void WelcomeMessage()
        {
            Console.WriteLine("Welcome to the BattleShip Lite Game");
            Console.WriteLine("created by developer: Usman Rahat");
            Console.WriteLine();
        }

        private static PlayerInfoModel CreatePlayer(string playerTitle)
        {
            PlayerInfoModel player = new PlayerInfoModel();

            Console.WriteLine($"Player information for {playerTitle}");

            // Now fill in the details for player.
            // We need to have atleast following to create player.
            // Initize the SpotGridLocations with all the spots empty. 
            // UsersName and ShipLocations to create the player.

            player.UsersName = AskForUsersName();
            GameLogic.InitializeGrid(player);
            PlaceShips(player);

            Console.Clear();

            return player;
        }

        private static string AskForUsersName()
        {
            string userName;
            Console.Write("What is your name: ");
            userName = Console.ReadLine();
            return userName;
        }

        private static void PlaceShips(PlayerInfoModel model)
        {
            do
            {
                Console.Write($"Where do you want to place the ship number {model.ShipLocations.Count + 1}: ");
                string location = Console.ReadLine();

                bool isValidLocation = false;

                try
                {
                    isValidLocation = GameLogic.PlaceShip(model, location);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                

                if (isValidLocation == false)
                {
                    Console.WriteLine("That was not a valid location. Please try again.");
                }
                else
                {
                    Console.WriteLine($"Ship number {model.ShipLocations.Count + 1} placed successfully");
                }
            } while (model.ShipLocations.Count < 2);
        }
    }
}
