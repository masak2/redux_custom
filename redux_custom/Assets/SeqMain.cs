using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using masak.common;

public class SeqMain : MonoBehaviour {

    private static readonly string kST_Idle = "idle";
    private static readonly string kST_Http = "http";
    private static readonly string kST_Wait = "wait";

    private static readonly string kActionHttp = "actionhttp";
    private static readonly string kActionWait = "actionwait";

    private const float kWaitTime = 3f;// sec

    [SerializeField]
    private Text _textState = null;
    [SerializeField]
    private Text _textMsg = null;

    private StateFuncs _statemachine = null;
    private ActionBlackboard _actionBlackboard = new ActionBlackboard();
    private UnityWebRequest _webrequest = null;
    private float _waitDuration = 0f;
	// Use this for initialization
	void Start () {
        _statemachine = new StateFuncs();
        _statemachine.Add(new StateFuncs.CStateElement(kST_Idle, ST_Idle, ST_Idle_Enter));
        _statemachine.Add(new StateFuncs.CStateElement(kST_Http, ST_Http, ST_Http_Enter));
        _statemachine.Add(new StateFuncs.CStateElement(kST_Wait, ST_Wait, ST_Wait_Enter));
        _statemachine.SetState(kST_Idle);

        ActionCreater[] arr = GameObject.FindObjectsOfType<ActionCreater>();
        foreach(ActionCreater ac in arr)
        {
            ac.Initialize(_actionBlackboard);
        }
	}
	
	// Update is called once per frame
	void Update () {
        _statemachine.Update();
	}

    private void ResetFlg()
    {
        _actionBlackboard.Clear();
    }

    private void ST_Idle_Enter()
    {
        ResetFlg();
        _textState.text = kST_Idle;
    }
    private void ST_Idle()
    {
        if( _actionBlackboard.IsRegistered(kActionHttp))
        {
            _statemachine.SetState(kST_Http);
            return;
        }

        if( _actionBlackboard.IsRegistered(kActionWait))
        {
            _statemachine.SetState(kST_Wait);
            return;
        }
    }

    private void ST_Http_Enter()
    {
        ResetFlg();
        _textState.text = kST_Http;
        _webrequest = UnityWebRequest.Get("https://www.google.co.jp/");
        _webrequest.Send();
    }
    private void ST_Http()
    {
        _textMsg.text = _webrequest.downloadProgress.ToString("F3");
        if( _webrequest.isDone)
        {
            if( _webrequest.isHttpError)
            {
                _textMsg.text = _webrequest.error;
            }
            else
            {
                _textMsg.text = _webrequest.downloadHandler.text;
            }
            _statemachine.SetState(kST_Idle);
            return;

        }
    }

    private void ST_Wait_Enter()
    {
        ResetFlg();
        _textState.text = kST_Wait;
        _waitDuration = kWaitTime;
    }
    private void ST_Wait()
    {
        _waitDuration -= Time.deltaTime;
        _textMsg.text = _waitDuration.ToString("F3");
        if( _waitDuration <= 0f)
        {
            _statemachine.SetState(kST_Idle);
            return;
        }
    }
}
