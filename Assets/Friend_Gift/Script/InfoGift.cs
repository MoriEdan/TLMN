using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;

namespace Assets.Script
{
   
    class InfoGift
    {
        public int ID;
        public string name;
        public int money;
        public Texture imgImage;
        public InfoGift(int id, string name, int money, Texture imgGift)
        {
            this.ID = id;
            this.name = name;
            this.money = money;
            this.imgImage = imgGift;
        }
    }
}
