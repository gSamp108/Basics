using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basics
{
    public class UnitPrototype
    {
        public class WeaponPrototype
        {
            public int Range;
            public int Damage;
            public int AreaOfEffect;
            public WeaponTargetTypes TargetType;
        }
        public class MovementPrototype
        {
            public MovementTypes Type;
            public int Range;
        }
        public class AbilitiesPrototype
        {
            public int Extraction;
            public int PowerGeneration;
            public int GroundUnitConstruction;
            public int AirUnitConstruction;
            public int StructureConstruction;
            public int AirUnitFuelStation;
            public int MineralStorage;
            public int PowerStorage;
        }

        public string Name { get; private set; }
        public decimal Structure;
        public decimal Armor;
        public UnitLayers Layer;
        public MovementPrototype Movement;
        public WeaponPrototype Weapon;
    }
}
