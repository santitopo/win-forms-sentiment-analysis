﻿using System;

namespace BusinessLogic
{
    public class Entity
    {
        public String Name { get; set; }

        public Entity()
        {

        }
        public Entity(String name)
        {
            Name = name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is Entity))
            {
                return false;
            }
            return Name == ((Entity)obj).Name;
        }

        public override String ToString()
        {
            return Name;
        }

        public Entity Clone()
        {
            return new Entity(Name);
        }
    }
}