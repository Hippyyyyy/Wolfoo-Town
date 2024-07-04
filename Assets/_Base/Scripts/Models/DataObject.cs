using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Base
{
    public class DataObject
    {
        public Vector3 _position;
        public Vector3 _rotation;
        public Vector3 _scale;
        public string _parentName;
        public int _instanceID;

        public DataObject(int instanceID, Vector3 position, Vector3 rotation, Vector3 scale, string parentName)
        {
            _position = position;
            _rotation = rotation;
            _scale = scale;
            _parentName = parentName;
            _instanceID = instanceID;
        }

        public void UpdateData(Vector3 position, Vector3 rotation, Vector3 scale, string parentName)
        {
            _position = position;
            _rotation = rotation;
            _scale = scale;
            _parentName = parentName;
        }
    }
}