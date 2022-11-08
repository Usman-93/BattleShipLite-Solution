using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShipLite_Library.Models
{
    public class PlayerInfoModel
    {
        public string UsersName { get; set; }
        public List<GridSpotModel> ShipLocations { get; set; } = new List<GridSpotModel>();
        public List<GridSpotModel> ShotGridLocations { get; set; } = new List<GridSpotModel>();
    }
}
