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
            public Dictionary<Stats, Double> Statistics;

            double Speed;
            Dictionary<DamageTypes, int> MinimumDamage;
            Dictionary<DamageTypes, int> MaximumDamage;

            int Quantity;
            int MaxQuanity;

            int CurrentGemLevel;

            public Item()
            {
            }

            public Item(ExiledToolkit.Models.PathOfExileObjects.Item pItem)
            {
                PropertyNames = new List<string>();
                SkillGemTypes = new List<SkillTypes>();
                Statistics = new Dictionary<Stats, double>();
                MinimumDamage = new Dictionary<DamageTypes, int>();
                MaximumDamage = new Dictionary<DamageTypes, int>();

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
                if (pItem.requirements != null)
                {
                    // parse them requirements
                }
                if (pItem.implicitMods != null)
                {
                    foreach (String mod in pItem.implicitMods)
                    {
                        EvaluateMod(mod);
                    }
                }
                if (pItem.explicitMods != null)
                {
                    foreach (String mod in pItem.explicitMods)
                    {
                        EvaluateMod(mod);
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

            public String getStatistic(Stats pStat)
            {
                String ret = String.Empty;
                try
                {
                    ret = Statistics[pStat].ToString();
                }
                catch (Exception)
                {
                    ret = "N/A";
                }
                return ret;
            }

            private void EvaluateProperty(ExiledToolkit.Models.PathOfExileObjects.Property pProp)
            {
                PropertyNameType lPropType = EvaluatePropertyName(pProp.name);
                //do something  with that type
                switch (lPropType)
                {
                    case PropertyNameType.Stack_Size:
                        {
                            // Items in this stack
                            String[] lValues = pProp.values[0];
                            String lStackSize = lValues[0];
                            String[] lQuantities = lStackSize.Split('/');
                            int.TryParse(lQuantities[0], out Quantity);
                            int.TryParse(lQuantities[1], out MaxQuanity);
                            break;
                        }
                    case PropertyNameType.Level:
                        {
                            // Current Gem Level
                            String[] lValues = pProp.values[0];
                            int.TryParse(lValues[0], out CurrentGemLevel);
                            break;
                        }
                    case PropertyNameType.Mana_Cost_Multiplier:
                        {
                            // Gems
                            break;
                        }
                    case PropertyNameType.Mana_Reserved:
                        {
                            // Gems
                            break;
                        }
                    case PropertyNameType.Cooldown_Time:
                        {
                            // Gems
                            break;
                        }
                    case PropertyNameType.Cast_Time:
                        {
                            // Gems
                            break;
                        }
                    case PropertyNameType.Mana_Cost:
                        {
                            // Gems
                            break;
                        }
                    case PropertyNameType.Critical_Strike_Chance:
                        {
                            break;
                        }
                    case PropertyNameType.Quality:
                        {
                            break;
                        }
                    case PropertyNameType.Damage_Effectiveness:
                        {
                            // Gems
                            break;
                        }
                    case PropertyNameType.Physical_Damage:
                        {
                            String[] lValues = pProp.values[0];
                            String lRange = lValues[0];
                            int lIncludesMods;
                            int.TryParse(lValues[1], out lIncludesMods);

                            String[] lDamages = lRange.Split('-');
                            int lMinimum, lMaximum;
                            int.TryParse(lDamages[0], out lMinimum);
                            int.TryParse(lDamages[1], out lMaximum);

                            MinimumDamage[DamageTypes.Physical] = lMinimum;
                            MaximumDamage[DamageTypes.Physical] = lMaximum;

                            break;
                        }
                    case PropertyNameType.Elemental_Damage:
                        {
                            String[] lValues = pProp.values[0];
                            String lRange = lValues[0];
                            int lTypeInt;
                            int.TryParse(lValues[1], out lTypeInt);

                            DamageTypes lType = (DamageTypes)lTypeInt;
                            String[] lDamages = lRange.Split('-');
                            int lMinimum, lMaximum;
                            int.TryParse(lDamages[0], out lMinimum);
                            int.TryParse(lDamages[1], out lMaximum);

                            MinimumDamage[lType] = lMinimum;
                            MaximumDamage[lType] = lMaximum;

                            break;
                        }
                    case PropertyNameType.Attacks_per_Second:
                        {
                            // Current Gem Level
                            String[] lValues = pProp.values[0];
                            double.TryParse(lValues[0], out Speed);
                            break;
                        }
                    case PropertyNameType.Chance_to_Block:
                        {
                            break;
                        }
                    case PropertyNameType.Energy_Shield:
                        {
                            String[] lValues = pProp.values[0];
                            int lEnergyShield;
                            int lContainsMods;
                            if (int.TryParse(lValues[0], out lEnergyShield))
                            {
                                Statistics[Stats.Energy_Shield] = lEnergyShield;
                            }
                            if (int.TryParse(lValues[1], out lContainsMods))
                            {
                                if (lContainsMods == 1)
                                {
                                    // The armour total is blue, and contains the mods on the item.
                                }
                            }
                            break;
                        }
                    case PropertyNameType.Evasion:
                        {
                            String[] lValues = pProp.values[0];
                            int lEvasion;
                            int lContainsMods;
                            if (int.TryParse(lValues[0], out lEvasion))
                            {
                                Statistics[Stats.Armour] = lEvasion;
                            }
                            if (int.TryParse(lValues[1], out lContainsMods))
                            {
                                if (lContainsMods == 1)
                                {
                                    // The armour total is blue, and contains the mods on the item.
                                }
                            }
                            break;
                        }
                    case PropertyNameType.Armour:
                        {
                            String[] lValues = pProp.values[0];
                            int lArmour;
                            int lContainsMods;
                            if (int.TryParse(lValues[0], out lArmour))
                            {
                                Statistics[Stats.Armour] = lArmour;
                            }
                            if (int.TryParse(lValues[1], out lContainsMods))
                            {
                                if (lContainsMods == 1)
                                {
                                    // The armour total is blue, and contains the mods on the item.
                                }
                            }
                            break;
                        }
                    case PropertyNameType.Weapon:
                        BaseType = pProp.name;
                        break;
                    case PropertyNameType.SkillGem:
                        BaseType = "Skill Gem";
                        break;
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

            private void EvaluateMod(String lMod)
            {
                if (lMod.StartsWith("Adds"))
                {
                    // Probably Damage Mod
                }
                else if (lMod.StartsWith("+"))
                {
                    // Flat bonus to something or Life/Mana on hit/kill
                }
                else if (lMod.Contains("%"))
                {
                    // Percent bonus to something
                }
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

            public enum Stats
            {
                Armour,
                Evasion,
                Energy_Shield,
            }

            public enum DamageTypes
            {
                Physical = 1,
                Fire = 4,
                Cold = 5,
                Lightning = 6
            }
        }
    }
}