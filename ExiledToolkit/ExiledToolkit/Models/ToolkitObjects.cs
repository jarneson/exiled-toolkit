using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ExiledToolkit.Models
{
    namespace ToolkitObjects
    {
        public class Item
        {
            public String JSONDefinition;
            //public bool verified;
            //public int w;
            //public int h;
            //public String icon;
            //public bool support;
            //public String league;
            //public List<Object> sockets;
            public String Name; //public String name;
            public String Type; //public String typeLine;
            public String WeaponType;
            //public bool identified;
            //public List<Property> properties;
            public List<String> PropertyNames;
            //public List<Property> requirements;
            //public List<String> implicitMods;
            //public List<String> explicitMods;
            public String Description;//public String descrText;
            //public int frameType;
            //public int x;
            //public int y;
            //public String inventoryId;
            //public List<Item> socketedItems;

            public Item()
            {
            }

            public Item(ExiledToolkit.Models.PathOfExileObjects.Item pItem)
            {
                JSONDefinition = JsonConvert.SerializeObject(pItem);
                Name = pItem.name;
                Type = pItem.typeLine;
                Description = pItem.descrText;
                PropertyNames = new List<string>();
                
                //Parse through properties
                if (pItem.properties != null)
                {
                    foreach (ExiledToolkit.Models.PathOfExileObjects.Property fProp in pItem.properties)
                    {
                        EvaluateProperty(fProp);
                    }
                }
            }

            public String toString()
            {
                return String.Format("{0} {1} {2}", Name, Type, Description);
            }

            private void EvaluateProperty(ExiledToolkit.Models.PathOfExileObjects.Property pProp)
            {
                PropertyNameType lPropType = EvaluatePropertyName(pProp.name);
                if (lPropType == PropertyNameType.WeaponName)
                {
                    WeaponType = pProp.name;
                }
            } 

            PropertyNameType EvaluatePropertyName(String pName)
            {
                PropertyNameType lType = PropertyNameType.Unknown;
                if (!Enum.TryParse(pName.Replace(" ", "_"), true, out lType))
                {
                    // Not a basic Type..
                    WeaponTypes temp;
                    if (Enum.TryParse(pName.Replace(" ", "_"), true, out temp))
                    {
                        // weapon..
                        lType = PropertyNameType.WeaponName;
                    }
                    else
                    {
                        lType = PropertyNameType.Unknown;
                    }
                }
                return lType;
            }

            public enum PropertyNameType
            {
                Unknown,
                Stack_Size,
                Level,
                Mana_Cost_Multiplier,
                Mana_Reserved,
                Cooldown_Time,
                Cast_Time,
                Mana_Cost,
                Critical_Strike_Chance,
                Quality,
                Damage_Effectiveness,
                Physical_Damage,
                Elemental_Damage,
                Attacks_per_Second,
                Chance_to_Block,
                Energy_Shield,
                Evasion,
                Armour,
                WeaponName
            }

            public enum WeaponTypes
            {
                Dagger,
                One_Handed_Sword,
                Claw,
                Wand,
                One_Handed_Mace,
                Two_Handed_Mace,
                Two_Handed_Axe,
                Staff,
                Two_Handed_Sword,
                One_Handed_Axe,
                Bow
            }
        }
    }
}