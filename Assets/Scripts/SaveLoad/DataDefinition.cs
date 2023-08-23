using System;
using UnityEngine;
using Utilities;

namespace SaveLoad
{
    public class DataDefinition : MonoBehaviour
    {
        public PersistentType Type;
        public string ID;

        private void OnValidate()
        {
            if (Type == PersistentType.ReadWrite)
            {
                if (ID == string.Empty) 
                    ID = Guid.NewGuid().ToString();
            }
            else
            {
                ID = string.Empty;
            }
            
        }
    }
}
