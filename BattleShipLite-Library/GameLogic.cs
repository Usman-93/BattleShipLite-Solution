using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleShipLite_Library.Models;

namespace BattleShipLite_Library
{
    public class GameLogic
    {
        public static void InitializeGrid(PlayerInfoModel model)
        {
            List<string> letters = new List<string>()
            {
                 "A"
                ,"B"
                ,"C"
                ,"D"
                ,"E"
            };

            List<int> numbers = new List<int>()
            {
                 1
                ,2
                ,3
                ,4
                ,5

            };

            foreach (var letter in letters)
            {
                foreach (var number in numbers)
                {
                    AddGridSpot(model, letter, number);
                }

            }
        }

        private static void AddGridSpot(PlayerInfoModel model, string letter, int number)
        {
            GridSpotModel spot = new GridSpotModel()
            {
                 SpotLetter = letter
                ,SpotNumber = number
                ,Status = GridSpotStatus.Empty
            };

            model.ShotGridLocations.Add(spot);

        }

        public static bool PlayerStillActive(PlayerInfoModel player)
        {
            bool isActive = false;

            foreach (var ShipLocation in player.ShipLocations)
            {
                if (ShipLocation.Status != GridSpotStatus.Sunk)
                {
                    isActive = true;
                }
            }

            return isActive;
        }

        public static bool PlaceShip(PlayerInfoModel model, string location)
        {
            bool output = false;
            (string row, int column) = SplitShotIntoRowsAndColumn(location);

            bool isValidLocation = ValidateGridLocation(model, row, column);
            bool isSpotOpen = ValidateShipLocation(model, row, column);

            if (isValidLocation && isSpotOpen)
            {
                model.ShipLocations.Add(

                new GridSpotModel()
                {
                    SpotLetter = row.ToUpper(),
                    SpotNumber = column,
                    Status = GridSpotStatus.Ship
                }
                );

                output = true;
            }

            return output;
        }

        private static bool ValidateShipLocation(PlayerInfoModel model, string row, int column)
        {
            bool isValidateLocation = true;

            foreach (var shipLocation in model.ShipLocations)
            {
                if (shipLocation.SpotLetter == row && shipLocation.SpotNumber == column)
                {
                    isValidateLocation = false;
                }
            }

            return isValidateLocation;
        }

        private static bool ValidateGridLocation(PlayerInfoModel model, string row, int column)
        {
            bool isValidateLocation = false;

            foreach (var shotLocation in model.ShotGridLocations)
            {
                if (shotLocation.SpotLetter == row && shotLocation.SpotNumber == column)
                {
                    isValidateLocation = true;
                }
            }

            return isValidateLocation;
        }

        public static int GetShotCount(PlayerInfoModel player)
        {
            int shotCount = 0;
            foreach (var shotLocation in player.ShotGridLocations)
            {
                if (shotLocation.Status != GridSpotStatus.Empty)
                {
                    shotCount += 1;
                }
            }
            return shotCount;
        }

        public static (string row, int column) SplitShotIntoRowsAndColumn(string shot)
        {
            string row = "";
            int column = 0;

            if (shot.Length !=2)
            {
                throw new ArgumentException("This was an invalid shot type.", "shot");
            }

            char[] shotArray = shot.ToArray();

            row = shotArray[0].ToString();
            column = int.Parse(shotArray[1].ToString());

            return (row, column);
        }

        public static bool ValidateShot(PlayerInfoModel player, string row, int column)
        {
            bool isValidate = false;

            foreach (var ShotGridLocation in player.ShotGridLocations)
            {
                if (ShotGridLocation.SpotLetter == row.ToUpper() && ShotGridLocation.SpotNumber == column)
                {
                    if (ShotGridLocation.Status == GridSpotStatus.Empty)
                    {
                        isValidate = true;
                    }
                }
            }
            return isValidate;
        }

        public static bool IdentifyShotResult(PlayerInfoModel opponent, string row, int column)
        {
            bool isAHit = false;

            foreach (var ShipLocation in opponent.ShipLocations)
            {
                if (ShipLocation.SpotLetter == row && ShipLocation.SpotNumber == column)
                {
                    isAHit = true;
                    ShipLocation.Status = GridSpotStatus.Sunk; 
                }
            }
            return isAHit;
        }

        public static void MarkShotResult(PlayerInfoModel player, string row, int column, bool isAHit)
        {
            foreach (var ShotGridLocation in player.ShotGridLocations)
            {
                if (ShotGridLocation.SpotLetter == row.ToUpper() && ShotGridLocation.SpotNumber == column)
                {
                    if (isAHit)
                    {
                        ShotGridLocation.Status = GridSpotStatus.Hit;
                    }
                    else
                    {
                        ShotGridLocation.Status = GridSpotStatus.Miss;
                    }
                }

            }
        }
    }
}
