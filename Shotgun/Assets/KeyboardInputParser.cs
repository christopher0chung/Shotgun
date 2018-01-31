using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KeyboardInputParser : MonoBehaviour {

    private List<BufferedInput> _bufferedInputs;
    private List<Flag_Tap> _tapFlags;
    private List<Flag_Hold> _holdFlags;

    private float _howLongToHold = .25f;

    public Reins reins;

    void Start()
    {
        _bufferedInputs = new List<BufferedInput>();
        _tapFlags = new List<Flag_Tap>();
        _holdFlags = new List<Flag_Hold>();
    }

    void Update()
    {
        _HouseKeeping();
        _CheckNewInputs();
        _UpdateBufferedInputs();

        _UpdateAllFlags();

        _AssessTapFlags();
        _AssessHoldFlags();
    }

    private void _HouseKeeping()
    {
        if (_bufferedInputs.Count > 0)
        {
            for (int i = _bufferedInputs.Count - 1; i >= 0; i--)
            {
                if (_bufferedInputs[i].markedForCleanup)
                    _bufferedInputs.Remove(_bufferedInputs[i]);
            }
        }
        if (_tapFlags.Count > 0)
        {
            for (int i = _tapFlags.Count - 1; i >= 0; i--)
            {
                if (_tapFlags[i].markedForCleanup)
                    _tapFlags.Remove(_tapFlags[i]);
            }
        }
        if (_holdFlags.Count > 0)
        {
            for (int i = _holdFlags.Count - 1; i >= 0; i--)
            {
                if (_holdFlags[i].markedForCleanup)
                    _holdFlags.Remove(_holdFlags[i]);
            }
        }
    }

    private void _CheckNewInputs()
    {
        if (Input.GetKeyDown(KeyCode.A))
            _bufferedInputs.Add(new BufferedInput(KeyCode.A, _howLongToHold, ref _tapFlags, ref _holdFlags));
        if (Input.GetKeyDown(KeyCode.S))
            _bufferedInputs.Add(new BufferedInput(KeyCode.S, _howLongToHold, ref _tapFlags, ref _holdFlags));
        if (Input.GetKeyDown(KeyCode.D))
            _bufferedInputs.Add(new BufferedInput(KeyCode.D, _howLongToHold, ref _tapFlags, ref _holdFlags));
        if (Input.GetKeyDown(KeyCode.F))
            _bufferedInputs.Add(new BufferedInput(KeyCode.F, _howLongToHold, ref _tapFlags, ref _holdFlags));
    }

    private void _UpdateBufferedInputs()
    {
        for (int i = 0; i < _bufferedInputs.Count; i++)
        {
            _bufferedInputs[i].Update();
        }
    }

    private void _UpdateAllFlags()
    {
        for (int i = 0; i < _tapFlags.Count; i++)
            _tapFlags[i].Update();
        for (int i = 0; i < _holdFlags.Count; i++)
            _holdFlags[i].Update();
    }

    private void _AssessTapFlags()
    {
        if (_tapFlags.Count < 2)
            return;
        else
        {
            if (_FTContainsType(_tapFlags, typeof(Flag_Tap_A)) && _FTContainsType(_tapFlags, typeof(Flag_Tap_F)))
            {
                reins.BigUp();
                _holdFlags.Clear();
                _tapFlags.Clear();
            }
            else if (_FTContainsType(_tapFlags, typeof(Flag_Tap_S)) && _FTContainsType(_tapFlags, typeof(Flag_Tap_D)))
            {
                reins.SmallUp();
                _holdFlags.Clear();
                _tapFlags.Clear();
            }
        }
    }

    private void _AssessHoldFlags()
    {
        if (_holdFlags.Count == 0 || _holdFlags.Count >= 4)
            return;
        else if (_holdFlags.Count == 1)
        {
            if (_FHContainsType(_holdFlags, typeof(Flag_Hold_A)))
                reins.BigLeft();
            else if (_FHContainsType(_holdFlags, typeof(Flag_Hold_S)))
                reins.SmallLeft();
            else if (_FHContainsType(_holdFlags, typeof(Flag_Hold_D)))
                reins.SmallRight();
            else if (_FHContainsType(_holdFlags, typeof(Flag_Hold_F)))
                reins.BigRight();
        }
        else if (_holdFlags.Count == 2)
        {
            if (_FHContainsType(_holdFlags, typeof(Flag_Hold_A)) && _FHContainsType(_holdFlags, typeof(Flag_Hold_F)))
                reins.BigDown();
            else if (_FHContainsType(_holdFlags, typeof(Flag_Hold_S)) && _FHContainsType(_holdFlags, typeof(Flag_Hold_D)))
                reins.SmallDown();
            else if (_FHContainsType(_holdFlags, typeof(Flag_Hold_A)) && _FHContainsType(_holdFlags, typeof(Flag_Hold_S)))
                reins.BigLeft();
            else if (_FHContainsType(_holdFlags, typeof(Flag_Hold_D)) && _FHContainsType(_holdFlags, typeof(Flag_Hold_F)))
                reins.BigRight();
            else if (_FHContainsType(_holdFlags, typeof(Flag_Hold_A)) && _FHContainsType(_holdFlags, typeof(Flag_Hold_D)))
                reins.BigLeftDown();
            else if (_FHContainsType(_holdFlags, typeof(Flag_Hold_S)) && _FHContainsType(_holdFlags, typeof(Flag_Hold_F)))
                reins.BigRightDown();
        }
        else if (_holdFlags.Count == 3)
        {
            if (!_FHContainsType(_holdFlags, typeof(Flag_Hold_A)))
                reins.BigRightDown();
            else if (!_FHContainsType(_holdFlags, typeof(Flag_Hold_S)))
                reins.BigRightDown();
            else if (!_FHContainsType(_holdFlags, typeof(Flag_Hold_D)))
                reins.BigLeftDown();
            else if (!_FHContainsType(_holdFlags, typeof(Flag_Hold_F)))
                reins.BigLeftDown();
        }
    }

    private bool _FTContainsType(List<Flag_Tap> l, Type t)
    {
        for (int i = 0; i < l.Count; i++)
        {
            if (l[i].GetType() == t)
                return true;
        }
        return false;
    }

    private bool _FHContainsType(List<Flag_Hold> l, Type t)
    {
        for (int i = 0; i < l.Count; i++)
        {
            if (l[i].GetType() == t)
                return true;
        }
        return false;
    }
}

public class BufferedInput
{
    private KeyCode _bufferedKey;
    private float _holdTime;
    private float _holdThreshold;

    private bool __nH;
    private bool _nowHold
    {
        get
        {
            return __nH;
        }
        set
        {
            if (value != __nH)
            {
                __nH = value;
                if (_bufferedKey == KeyCode.A)
                    _myHold = new Flag_Hold_A(_flagDuration);
                else if (_bufferedKey == KeyCode.S)
                    _myHold = new Flag_Hold_S(_flagDuration);
                else if (_bufferedKey == KeyCode.D)
                    _myHold = new Flag_Hold_D(_flagDuration);
                else if (_bufferedKey == KeyCode.F)
                    _myHold = new Flag_Hold_F(_flagDuration);
            }
        }
    }

    private List<Flag_Tap> _taps;
    private List<Flag_Hold> _holds;

    private Flag_Tap _myTap;
    private Flag_Hold _myHold;

    private float _flagDuration = .1f;

    public bool markedForCleanup { get; private set; }

    public BufferedInput(KeyCode kC, float holdThreshold, ref List<Flag_Tap> tapList, ref List<Flag_Hold> holdList)
    {
        _bufferedKey = kC;
        _holdThreshold = holdThreshold;
        _taps = tapList;
        _holds = holdList;
    }

    public void Update()
    {
        if (markedForCleanup)
        {
            return;
        }

        if (!Input.GetKey(_bufferedKey))
        {
            if (!_nowHold)
            {
                if (_bufferedKey == KeyCode.A)
                    _myTap = new Flag_Tap_A(_holdThreshold);
                else if (_bufferedKey == KeyCode.S)
                    _myTap = new Flag_Tap_S(_holdThreshold);
                else if (_bufferedKey == KeyCode.D)
                    _myTap = new Flag_Tap_D(_holdThreshold);
                else if (_bufferedKey == KeyCode.F)
                    _myTap = new Flag_Tap_F(_holdThreshold);

                _taps.Add(_myTap);
            }
            markedForCleanup = true;
        }

        if (Input.GetKey(_bufferedKey))
            _holdTime += Time.deltaTime;

        if (Input.GetKey(_bufferedKey) && _holdTime >= _holdThreshold)
        {
            _nowHold = true;

            if (_holds.Contains(_myHold))
            {
                _myHold.Refresh();
            }
            else
            {
                _holds.Add(_myHold);
            }
        }
    }
}

public class Flag_Tap
{
    protected float _flagDuration;

    protected float _timer;
    public bool markedForCleanup { get; private set; }

    public Flag_Tap (float duration)
    {
        _flagDuration = duration;
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _flagDuration)
            markedForCleanup = true;
    }
}

public class Flag_Tap_A : Flag_Tap
{
    public Flag_Tap_A (float duration) : base (duration) { }
}
public class Flag_Tap_S : Flag_Tap
{
    public Flag_Tap_S(float duration) : base(duration) { }
}
public class Flag_Tap_D : Flag_Tap
{
    public Flag_Tap_D(float duration) : base(duration) { }
}
public class Flag_Tap_F : Flag_Tap
{
    public Flag_Tap_F(float duration) : base(duration) { }
}

public class Flag_Hold
{
    protected float _flagDuration;

    protected float _timer;
    public bool markedForCleanup { get; private set; }

    public Flag_Hold(float duration)
    {
        _flagDuration = duration;
    }

    public void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _flagDuration)
            markedForCleanup = true;
    }

    public void Refresh()
    {
        _timer = 0;
    }
}

public class Flag_Hold_A : Flag_Hold
{
    public Flag_Hold_A(float duration) : base(duration) { }
}
public class Flag_Hold_S : Flag_Hold
{
    public Flag_Hold_S(float duration) : base(duration) { }
}
public class Flag_Hold_D : Flag_Hold
{
    public Flag_Hold_D(float duration) : base(duration) { }
}
public class Flag_Hold_F : Flag_Hold
{
    public Flag_Hold_F(float duration) : base(duration) { }
}
