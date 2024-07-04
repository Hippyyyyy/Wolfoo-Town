using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace _WolfooShoppingMall
{
    public class Table : BackItem
    {
        [SerializeField] List<Transform> frontDeskTrans;
        [SerializeField] bool isBlockCharacter;
        private float distance_;
        private float xRange;
        private Transform compareItem;

        public bool IsEnable { get; set; } = true;
        public Edge[] Edges { get; private set; }

        private void Start()
        {
            Edges = GetEdges();
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);

            if (!IsEnable) return;

            if (item.backitem == null) return;
            if (item.shoppingBasket != null) return;
            if (isBlockCharacter && item.character != null) return;
            if (isBlockCharacter && item.newCharacter != null) return;

            compareItem = item.backitem.StandZone == null ? item.backitem.transform : item.backitem.StandZone;

            if (compareItem == null) return;

            if (compareItem.position.y < frontDeskTrans[0].position.y ||
                compareItem.position.x < frontDeskTrans[0].position.x ||
                compareItem.position.y > frontDeskTrans[1].position.y + 1 ||
                compareItem.position.x > frontDeskTrans[1].position.x)
            {
                return;
            }

            if (item.backitem.IsAssigned) return;
            OnDistanceVerified(item.backitem);
        }

        protected override void InitItem()
        {
        }

        public bool Is_inside(Vector2 itemPos)
        {
            var cnt = 0;
            Edges = GetEdges();
            foreach (var edge in Edges)
            {
                var x1 = edge.point1.x;
                var y1 = edge.point1.y;
                var x2 = edge.point2.x;
                var y2 = edge.point2.y;

                if ((itemPos.y < y1) != (itemPos.y < y2) && itemPos.x < x1 + ((itemPos.y - y1) / (y2 - y1)) * (x2 - x1))
                {
                    cnt += 1;
                }
            }
            Debug.Log("IS_Inside: " + (cnt % 2 == 1));
            return cnt % 2 == 1;
        }
        private Edge[] GetEdges()
        {
            //    if (edges == null || limitZones.Length > edges.Length)
            var edges = new Edge[4];
            var leftDownPos = frontDeskTrans[0].position;
            var rightDownPos = new Vector2(frontDeskTrans[1].position.x, frontDeskTrans[0].position.y);
            var rightUpPos = frontDeskTrans[1].position;
            var leftUpPos = new Vector2(frontDeskTrans[0].position.x, frontDeskTrans[1].position.y);

            var edge = new Edge(leftDownPos, rightDownPos);
            edges[0] = edge;

            edge = new Edge(rightDownPos, rightUpPos);
            edges[1] = edge;

            edge = new Edge(rightUpPos, leftUpPos);
            edges[2] = edge;

            edge = new Edge(leftUpPos, leftDownPos);
            edges[3] = edge;

            return edges;
        }

        protected virtual void OnDistanceVerified(BackItem backitem)
        {
            isJumpingTable = true;
            if (SoundManager.instance == null) { SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.FallingDown); }
            else { SoundManager.instance.PlayOtherSfx(SfxOtherType.FallingDown, 0.2f); }

            if (compareItem.position.y > frontDeskTrans[1].position.y)
            {
         //       backitem.transform.SetParent(transform);
                backitem.JumpToEndPos(new Vector3(backitem.transform.position.x, frontDeskTrans[1].position.y, 0),
                    transform, null, assignPriorityy);
                //backitem.JumpToEndLocalPos(
                //    new Vector3(backitem.transform.localPosition.x, frontDeskTrans[1].localPosition.y, 0),
                //    null, DG.Tweening.Ease.OutBounce, 70, true, assignPriorityy);
            }
            else
            {
                backitem.JumpToEndPos(backitem.transform.position,
                    transform, null, assignPriorityy);
             //   backitem.transform.SetParent(transform);
                //backitem.JumpToEndLocalPos(backitem.transform.localPosition,
                //    null, DG.Tweening.Ease.OutBounce, 70, true, assignPriorityy);
            }
        }

#if UNITY_EDITOR
        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.yellow;
        //    Gizmos.DrawLine(((RectTransform)frontDeskTrans[0].transform).position, ((RectTransform)frontDeskTrans[1].transform).position);
        //    if (compareItem != null)
        //    {
        //        var min = frontDeskTrans[1].transform.position;
        //        var max = frontDeskTrans[0].transform.position;
        //        Gizmos.color = compareItem.transform.position.y < max.y &&
        //            compareItem.transform.position.y > min.y ?
        //            Color.green : Color.red;
        //        Gizmos.DrawLine(compareItem.transform.position, frontDeskTrans[0].transform.position);
        //        Gizmos.color = compareItem.transform.position.y < max.y &&
        //         compareItem.transform.position.y > min.y ?
        //         Color.green : Color.red;
        //        Gizmos.DrawLine(compareItem.transform.position, frontDeskTrans[1].transform.position);

        //        if (compareItem.transform.position.y < frontDeskTrans[0].position.y ||
        //             compareItem.transform.position.x > frontDeskTrans[0].position.x ||
        //             compareItem.transform.position.x < frontDeskTrans[1].position.x)
        //        {
        //            Gizmos.color = Color.white;
        //            Gizmos.DrawLine(compareItem.transform.position, frontDeskTrans[1].transform.position);
        //        }
        //        else
        //        {
        //            Gizmos.color = Color.black;
        //            Gizmos.DrawLine(compareItem.transform.position, frontDeskTrans[1].transform.position);
        //        }
        //    }
        //}
#endif
    }

}