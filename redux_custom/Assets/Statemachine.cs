using System.Collections.Generic;
namespace masak.common
{
    public class StateFuncs
    {
        public delegate void StateDelegate();

        private List<CStateElement> _stateElementList = new List<CStateElement>();
        private CStateElement _activeStateElem = null;

        public class CStateElement
        {
            private StateDelegate _FuncRun { get; set; }
            private StateDelegate _FuncInit { get; set; }
            private StateDelegate _FuncEnd { get; set; }
            public readonly string _name = null;

            public CStateElement(string szName, StateDelegate run, StateDelegate init, StateDelegate end = null)
            {
                _name = szName;
                _FuncRun = run;
                _FuncInit = init;
                _FuncEnd = end;
            }

            public void Start()
            {
                if (_FuncInit != null)
                {
                    _FuncInit();
                }
            }

            public void Update()
            {
                if (_FuncRun != null)
                {
                    _FuncRun();
                }
            }
            public void End()
            {
                if (_FuncEnd != null)
                {
                    _FuncEnd();
                }
            }
        }
        public void DestroyObject()
        {
            _stateElementList.Clear();
            _stateElementList = null;
            _activeStateElem = null;
        }

        public void Add(CStateElement oElem)
        {
            _stateElementList.Add(oElem);
        }

        public void SetState(string szName)
        {
            CStateElement oFindElem = _stateElementList.Find(delegate (CStateElement oElem)
            {
                return oElem._name == szName;
            });


            if (oFindElem != null)
            {
                CStateElement pOldEelm = _activeStateElem;
                if (pOldEelm != null)
                {
                    pOldEelm.End();
                }
                _activeStateElem = oFindElem;

                oFindElem.Start();

            }
        }

        public string GetStateName()
        {
            if (_activeStateElem != null)
            {
                return _activeStateElem._name;
            }
            else
            {
                return null;
            }
        }

        public void Update()
        {
            if (_activeStateElem == null)
            {
                return;
            }

            _activeStateElem.Update();

        }

    }
}