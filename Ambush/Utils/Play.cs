using Ambush.Client;
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
    public sealed class Play
    {
        public  List<CPX> cPXes;
        public  List<MiniController> nods;
        public  bool[] cpxStates;
        public  string gameMode;
        public int score;
        public  Dictionary<string, string> xTriggeredY; //X Triggered Y
        public  Dictionary<string, string> triggeredToState; //Y change to state 
        public  List<Component> defaultValues;
        public  Dictionary<string, string> DetectorToLaserController; //Detector and laser controller dependencies

        private static Play instance = new Play();

        private Play()
        {
            cPXes = new List<CPX>();
            nods = new List<MiniController>();
            cpxStates = new bool[6];
            triggeredToState = new Dictionary<string, string>();
            defaultValues = new List<Component>();
        }


        public static Play Instance
        {
            get
            {
                return instance;
            }
        }


        public static void setTriggeredToState(Dictionary<string, string> dict)
        {
            instance.triggeredToState = dict;
        }

        public void AddToDetectorToLaserController(string detectorControllerID, string laserControllerID)
        {
            DetectorToLaserController.Add(detectorControllerID, laserControllerID);
            DetectorToLaserController.Add(laserControllerID, detectorControllerID);
        }

        public static void setxTriggeredY(Dictionary<string, string> dict)
        {
            instance.xTriggeredY = dict;
        }
        public static void setScore(int _score)
        {
            instance.score = _score;
        }

        public static void setGameMode(string GameMode)
        {
            instance.gameMode = GameMode;
        }

        public static void setCpxStates(bool[] cpxstates)
        {
            instance.cpxStates = cpxstates;
        }
        public void setCPXes(List<CPX> cPxes)
        {
            cPXes = cPxes;
        }

        public  MiniController getControllerById(string id)
        {
            foreach (MiniController con in nods)
            {
                if (con.id == id)
                    return con;
            }
            return null;
        }

        public  CPX getCpxByPhysicalId(int physicalId)
        {
            foreach (CPX cpx in cPXes)
            {
                foreach (Component cmp in cpx.components)
                {
                    if (cmp.physicalID.ToString() == physicalId.ToString())
                        return cpx;
                }
            }
            return null;

        }

        public static CPX getCpxByRpiId(string rpid)
        {
            foreach (CPX cpx in instance.cPXes)
            {
                if (cpx.id == Int32.Parse(rpid))
                    return cpx;
            }
            return null;
        }

        public static void setCpxComponents(string rpid, List<Component> comps)
        {
            foreach (CPX cpx in instance.cPXes)
            {
                if (cpx.id == Int32.Parse(rpid))
                {
                    cpx.components = comps;
                }
            }
        }

        public Component getComponentByPhysicalID(string id)
        {
            foreach (CPX cpx in cPXes)
            {
                foreach (Component comp in cpx.components)
                {
                    if (comp.physicalID == Int32.Parse(id))
                        return comp;
                }
            }
            return null;
        }

        public static MiniController getMiniControllerById(string id)
        {
            foreach (MiniController con in instance.nods)
            {
                if (con.id == id)
                    return con;
            }
            return null;
        }

        public static void invokeTrigger(object sender, InvokeTriggerEventArgs e)
        {

            var result = instance.GetKey(instance.xTriggeredY, e.physicalID);
            if (result != "")
            {
                Component cmp = instance.getComponentByPhysicalID(result);
                if (cmp == null)
                {
                    cmp = GetControllerComponentByPhysicalId(Int32.Parse(result));
                    if (cmp is Laser)
                    {
                        cmp = (Laser)cmp;
                    }
                    else if (cmp is Detector)
                    {
                        cmp = (Detector)cmp;
                    }
                    ((IProfile)cmp).Invoke(instance.GetKey(instance.triggeredToState, e.physicalID));


                }
                else
                {
                    if (cmp is Target)
                    {
                        cmp = (Target)cmp;
                        ((IProfile)cmp).Invoke(instance.GetKey(instance.triggeredToState, e.physicalID));

                    }
                    else if (cmp is Components.Door)
                    {
                        cmp = (Components.Door)cmp;
                        string nextState = instance.GetKey(instance.triggeredToState, result);
                        ((IProfile)cmp).Invoke(nextState);
                    }

                }

            }
        }
        public  string GetKey(IReadOnlyDictionary<string, string> dictValues, string keyValue)
        {
            return dictValues.ContainsKey(keyValue) ? dictValues[keyValue] : "";
        }

        public void addController(MiniController con)
        {
            nods.Add(con);
        }
        public void setControllers(List<MiniController> cons)
        {
            nods = cons;
        }

        public static void SetComponentsToDefault()
        {
            foreach (CPX cpx in instance.cPXes)
            {
                foreach (Component comp in cpx.components)
                {
                    if (comp.source == Constants.DR)
                    {
                        Components.Door door = (Components.Door)comp;
                        door.setDoorState(Conversions.stringToDirection(door.defaultState));
                    }
                    else if (comp.source == Constants.TR)
                    {
                        Target target = (Target)comp;
                        target.setTargetState(Conversions.stringToDirection(target.defaultState));
                    }
                }
            }

            StringBuilder builder = new StringBuilder();
            foreach (MiniController con in instance.nods)
            {
                con.setType();
                builder.Clear();
                builder.Append("AR:SRE:");
                builder.Append(con.type);
                builder.Append(con.id.ToString());
                builder.Append(":");
                builder.Append(con.default_state_string);
                using (TCPClient client = new TCPClient(builder.ToString(), con.ip, con.port)) { }
            }
        }

        public static Component GetControllerComponentByPhysicalId(int id)
        {
            foreach (MiniController con in instance.nods)
            {
                if (con.type == Constants.LS)
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

