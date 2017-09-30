using System.Collections.Generic;
using UnityEngine.Assertions;


namespace masak.common
{
    public class ActionBlackboard
    {
        private Dictionary<string, int> _hash = new Dictionary<string, int>();

        public void Register(string key, int param = 0)
        {
            if (!_hash.ContainsKey(key))
            {
                _hash.Add(key, param);
            }
            else
            {
                _hash[key] = param;
            }
        }
        public bool IsRegistered(string key)
        {
            return _hash.ContainsKey(key);
        }

        public int GetParam(string key)
        {
            Assert.IsTrue(_hash.ContainsKey(key), "なんで登録されてないキーのパラメータにアクセスすんの？=" + key);
            return _hash[key];
        }

        public void Clear()
        {
            _hash.Clear();
        }
    }
}