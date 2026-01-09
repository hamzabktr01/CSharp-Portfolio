using Game.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Engine
{
    public class PowerUpLoader
    {
        public List<PowerUp> ItemList = new List<PowerUp>();

        public PowerUp testPowerUp = new PowerUp(200, 300, 20, 20, 20);
   
        public PowerUpLoader()
        {

            ItemList.Add(testPowerUp);
        
        }
        public PowerUpLoader(List<PowerUp> itemList)
        {
            this.ItemList = itemList;

        }
        public void UpdateItems(ref Player player)
        {
            foreach (PowerUp powerUp in ItemList)
            {
                powerUp.Update_Item(ref player);


            }
        }
        public void DrawItems()
        {
            foreach (PowerUp powerUp in ItemList)
            {
                powerUp.DrawItemAnimated();
            }
        }
    }
}
