using System.Collections.Generic;
using UnityEngine;

namespace TWS.Utils
{
    public class BillboardController
    {
        public static BillboardController Instance;

        private readonly List<Billboard> billboards = new List<Billboard>();

        public static void Reset()
        {
            if (Instance == null) return;
            Instance.billboards.Clear();
        }

        public static void Add(Billboard obj)
        {
            if (Instance == null) Instance = new BillboardController();
            Instance.billboards.Add(obj);
        }

        public static void Remove(Billboard obj)
        {
            Instance.SwapRemove(obj);
        }

        private void SwapRemove(Billboard obj)
        {
            var index = billboards.IndexOf(obj);
            if (index == -1) return;

            if (index < billboards.Count - 1)
                billboards[index] = billboards[billboards.Count - 1];


            billboards.RemoveAt(index);
        }

        public void Update(Camera cam)
        {
            var angle = cam.transform.rotation.eulerAngles.y;
            foreach (Billboard bb in billboards)
            {
                bb.transform.rotation = Quaternion.Euler(0, angle, 0);
            }
        }
    }
}
