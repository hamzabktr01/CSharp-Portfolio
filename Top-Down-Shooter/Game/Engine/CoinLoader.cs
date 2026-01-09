using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Game.Gameplay;

namespace Game.Engine
{
    public class CoinLoader
    {
        public List<Coin> ItemList = new List<Coin>();


        public Coin testItem = new Coin(5, 200, 135, 20, 20, 100);
        public Coin testItem2 = new Coin(5, 300 , 135, 20, 20, 100);
        public Coin testItem3 = new Coin(5, 400, 135, 20, 20, 100);



        public CoinLoader()
        {

            ItemList.Add(testItem);
            ItemList.Add(testItem2);
            ItemList.Add(testItem3);


        }
        public CoinLoader(List<Coin> itemList)
        {
            this.ItemList = itemList;
        }
        public void UpdateItems(ref Player player)
        {
            foreach (Coin coin in ItemList)
            {
                coin.Update_Item(ref player);
  

            }
        }
        public void DrawItems()
        {
            foreach (Coin coin in ItemList)
            {             
                coin.DrawItemAnimated();
            }
        }
    }
}
