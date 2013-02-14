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
            public String BaseType;
            //public bool identified;
            //public List<Property> properties;
            public List<String> PropertyNames;
            public List<SkillTypes> SkillGemTypes;
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
                PropertyNames = new List<string>();
                SkillGemTypes = new List<SkillTypes>();

                JSONDefinition = JsonConvert.SerializeObject(pItem);
                Name = pItem.name;
                Type = pItem.typeLine;
                Description = pItem.descrText;
                BaseType = String.Empty;
                
                //Parse through properties
                if (pItem.properties != null)
                {
                    foreach (ExiledToolkit.Models.PathOfExileObjects.Property fProp in pItem.properties)
                    {
                        EvaluateProperty(fProp);
                    }
                }
                if (BaseType == String.Empty)
                {
                    if (Type.Contains("Belt"))
                    {
                        BaseType = "Belt";
                    }
                    else if (Type.Contains("Ring"))
                    {
                        BaseType = "Ring";
                    }
                    else if (Type.Contains("Amulet"))
                    {
                        BaseType = "Amulet";
                    }
                    else if (Type.Contains("Flask"))
                    {
                        BaseType = "Flask";
                    }
                    else if (Description != null && Description.Length > 0)
                    {
                        BaseType = "Currency";
                    }
                    else
                    {
                        BaseType = "Armor";
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
                if (lPropType == PropertyNameType.Weapon)
                {
                    BaseType = pProp.name;
                }
                else if (lPropType == PropertyNameType.SkillGem)
                {
                    BaseType = "Skill Gem";
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
                        lType = PropertyNameType.Weapon;
                    }
                    else
                    {
                        // maybe it's a spell
                        if (pName.Contains(','))
                        {
                            lType = PropertyNameType.SkillGem;
                            String[] tokens = pName.Split(',');
                            foreach (String s in tokens)
                            {
                                SkillTypes type = SkillTypes.Unknown;
                                if (Enum.TryParse(s, out type))
                                {
                                    SkillGemTypes.Add(type);
                                }
                            }
                        }
                        else
                        {
                            // It may still be a spell..
                            SkillTypes type = SkillTypes.Unknown;
                            if (Enum.TryParse(pName, out type))
                            {
                                lType = PropertyNameType.SkillGem;
                                SkillGemTypes.Add(type);
                            }
                            else
                            {
                                lType = PropertyNameType.Unknown;
                            }
                        }
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
                Weapon,
                SkillGem
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

            public enum SkillTypes
            {
                Unknown,
                Cold,
                Fire,
                Lightning,
                Chaos,
                Spell,
                Projectile,
                Trap,
                Attack,
                Curse,
                AoE,
                Melee,
                Duration,
                Support,
                Minion,
                Totem,
                Bow
            }
        }
    }
}