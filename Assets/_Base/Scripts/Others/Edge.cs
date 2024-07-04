using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [System.Serializable]
    public class Edge
    {
        public Vector2 point1;
        public Vector2 point2;
        public Transform pointArea1;
        public Transform pointArea2;

        public Edge(Transform point1, Transform point2)
        {
            this.point1 = point1.position;
            this.point2 = point2.position;
            pointArea1 = point1;
            pointArea2 = point2;
        }
        public Edge(Vector2 point1, Vector2 point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }
    }
}