using UnityEngine;

namespace MiddleAges.Database
{
    [CreateAssetMenu(fileName = "Items", menuName = "Database/Items/Item", order = 0)]
    public class Item : ScriptableObject
    {
        public ItemData ItemInfo;
    }
}