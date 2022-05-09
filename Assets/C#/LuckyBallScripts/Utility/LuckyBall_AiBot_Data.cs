using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LuckyBall.Utility
{
    [CreateAssetMenu]
    public class LuckyBall_AiBot_Data : ScriptableObject
    {
        public List<Bot> bots;
        public void AddData(Bot data)
        {
            bots.Add(data);
        }
        public List<Bot> GetData()
        {
            return bots;
        }
    }
}
