using UnityEngine;

namespace TWS.Utils
{
    public class Billboard : MonoBehaviour
    {
        private void OnEnable()
        {
            BillboardController.Add(this);
        }

        private void OnDisable()
        {
            BillboardController.Remove(this);
        }
    }
}
