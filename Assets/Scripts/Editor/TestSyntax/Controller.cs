using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// using flash.display.DisplayObjectContainer;
using /*<com>*/Finegamedesign.Utils/*<DataUtil>*/;
namespace Monster
{
    public class Controller
    {
        public delegate /*<var>*/void ChildKeyChangeDelegate(string _key, string _change);
        
        public static void ListenToChildren(Dictionary<string, object> view, ArrayList childNames, string methodName, /*<var>*/object owner)
        {
            for (int c = 0; c < DataUtil.Length(childNames); c++)
            {
                string name = (string)(childNames[c]);
                var child = view[name];
                // View.listenToOverAndDown(child, methodName, owner);
            }
        }
        
        public static bool IsObject(/*<var>*/object value)
        {
            return !(value is int || value is string || value is float || value is bool);
        }
        
        /**
         * @param   changes     What is different as nested hashes.
         */
        public static void Visit(Dictionary<string, object> parent, Dictionary<string, object> changes)
        {
            foreach(KeyValuePair<string, object> _entry in changes){
                string key = _entry.Key;
                var change = changes[key];
                var child = parent[key];
                if (IsObject(change))
                {
                    Visit((Dictionary<string, object>)(child), (Dictionary<string, object>)(change));
                }
                else
                {
                    if (object.ReferenceEquals("x", key))
                    {
                        //+ ViewUtil.setPositionX(parent, change);
                    }
                    else if (object.ReferenceEquals("y", key))
                    {
                        //+ ViewUtil.setPositionY(parent, change);
                    }
                    else if (object.ReferenceEquals("visible", key))
                    {
                        ViewUtil.SetVisible((GameObject)(parent["view"]), (bool)(change));
                    }
                }
            }
        }
    }
}
