using UnityEngine;

namespace Utils.Visual
{
    public class AssociatedGameObjectActivator : MonoBehaviour
    {
        [SerializeField] private GameObject[] _associatedObjects;

        private void OnEnable()
        {
            for (var i = 0; i < _associatedObjects.Length; i++)
            {
                _associatedObjects[i].SetActive(true);
            }
        }

        private void OnDisable()
        {
            for (var i = 0; i < _associatedObjects.Length; i++)
            {
                _associatedObjects[i].SetActive(false);
            }
        }
    }
}
