using Ambush.Components;
using Ambush.CustomEventArgs;
using Ambush.Profiles;
using Ambush.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambush.Utils
{
    public class Play
    {
        public static List<CPX> cPXes;
        public static List<MiniController> nods;
        public static bool[] cpxStates;
        public static string gameMode;
        public static int score;
        public static Dictionary<string,string> xTriggeredY;
        public static Dictionary<string, string> triggeredToState;
        public static List<Component> defaultValues;
        public Play()
        {
            
            cPXes = new List<CPX>();
            nods = new List<MiniController>();
            cpxStates  = new bool[6];
            triggeredToState = new Dictionary<string, string>();
            defaultValues = new List<Component>();
        }

        public static void setTriggeredToState(Dictionary<string,string> dict)
        {
            triggeredToState = dict;
        }

        public static void setxTriggeredY(Dictionary<string,string> dict)
        {
            xTriggeredY = dict;
        }
        public static void setScore(int _score)
        {
            score = _score;
        }

        public static void setGameMode(string GameMode)
        {
            gameMode = GameMode;
        }

        public static void setCpxStates(bool[] cpxstates)
        {
            cpxStates = cpxstates;
        }
        public static void setCPXes(List<CPX> cPxes)
        {
            cPXes = cPxes;
        }

        public static MiniController getControllerById(string id)
        {
            foreach (MiniController con in nods)
            {
                if (con.id == id)
                    return con;
            }
            return null;
        }

        public static CPX getCpxByPhysicalId(int physicalId)
        {
            foreach(CPX cpx in cPXes)
            {
                foreach(Component cmp in cpx.components )
                {
                    if (cmp.physicalID.ToString() == physicalId.ToString())
                        return cpx;
                }
            }
            return null;
        
        }

        public static CPX getCpxByRpiId(string rpid)
        {
            foreach( CPX cpx in cPXes)
            {
                if (cpx.id == Int32.Parse(rpid))
                    return cpx;
            }
            return null;
        }

        public static void setCpxComponents(string rpid, List<Component> comps)
        {
            foreach ( CPX cpx in cPXes)
            {
                if(cpx.id == Int32.Parse(rpid))
                {
                    cpx.components = comps;
                }
            }
        }
        
        public static Component getComponentByPhysicalID(string id)
        {
            foreach (CPX cpx in cPXes)
            {
                foreach(Component comp in cpx.components)
                {
                    if (comp.physicalID == Int32.Parse(id))
                        return comp;
                }
            }
            return null;
        }

        public static MiniController getMiniControllerById(string id)
        {
            foreach( MiniController con in nods)
            {
                if (con.id == id)
                    return con;
            }
            return null;
        }
        
        public static void invokeTrigger(object sender, InvokeTriggerEventArgs e)
        {
            var result = GetKey(xTriggeredY, e.physicalID);
            if (result != "")
            {
                Component cmp = getComponentByPhysicalID(result);
                if(cmp == null)
                {
                    cmp = GetControllerComponentByPhysicalId(Int32.Parse(result));
                    if(cmp is Laser)
                    {
                        cmp = (Laser)cmp;
                    }
                    else if (cmp is Detector)
                    {
                        cmp = (Detector)cmp;
                    }
                    ((IProfile)cmp).Invoke(GetKey(triggeredToState, e.physicalID));


                }
                else
                {
                    if (cmp is Target)
                    {
                        cmp = (Target)cmp;
                        ((IProfile)cmp).Invoke(GetKey(triggeredToState, e.physicalID));

                    }
                    else if (cmp is Components.Door)
                    {
                        cmp = (Components.Door)cmp;
                        string nextState = GetKey(triggeredToState, result);
                        ((IProfile)cmp).Invoke(nextState);
                    }
                    
                }

            }
        }
        public static string GetKey(IReadOnlyDictionary<string, string> dictValues, string keyValue)
        {
            return dictValues.ContainsKey(keyValue) ? dictValues[keyValue] : "";
        }

        public static void addController(MiniController con)
        {
            nods.Add(con);
        }
        public static void setControllers(List<MiniController> cons)
        {
            nods = cons;
        }

        public static void SetComponentsToDefault()
        {
            foreach(CPX cpx in cPXes)
            {
                foreach(Component comp in cpx.components)
                {
                    if(comp.source == Constants.DR )
                    {
                        Components.Door door = (Components.Door)comp;
                        door.setDoorState(Conversions.stringToDirection(door.defaultState));
                    }
                    else if ( comp.source == Constants.TR )
                    {
                        Target target = (Target)comp;
                        target.setTargetState(Conversions.stringToDirection(target.defaultState));
                    }
                }

                //TODO --> Set Lasers to default values
            }
        }

        public static Component GetControllerComponentByPhysicalId(int id)
        {
            foreach(MiniController con in nods)
            {
                if(con.type == Constants.LS)
                {
                    foreach (Laser laser in con.lasers)
                        if (laser.physicalID == id)
                            return laser;
                }
                else if (con.type == Constants.DT)
                {
                    foreach (Detector dt in con.detectors)
                        if (dt.physicalID == id)
                            return dt;
                }
            }
            return null;
        }
    }
}
