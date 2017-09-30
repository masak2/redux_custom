using UnityEngine;
using UnityEngine.Assertions;


namespace masak.common
{
    public class ActionCreater : MonoBehaviour
    {
        [SerializeField]
        private string _actionName = null;
        private ActionBlackboard _actionBlackboard = null;

        public void Initialize(ActionBlackboard ab)
        {
            Assert.IsNotNull(ab);
            _actionBlackboard = ab;
        }


        public void OnClickButton()
        {
            Assert.IsNotNull(_actionName, "action登録しろ");
            Assert.IsNotNull(_actionBlackboard);

            _actionBlackboard.Register(_actionName);
        }
    }
}