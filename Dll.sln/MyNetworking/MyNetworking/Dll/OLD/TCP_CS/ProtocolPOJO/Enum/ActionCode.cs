using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Client Ping 服务器
/// </summary>
public enum ActionCode
{
    None = -1,
    /// <summary>
    /// 轮询心跳
    /// </summary>
    Ping = 0,
    Pong = -2,
    /// <summary>
    /// 身份绑定专用
    /// </summary>
    BindIdentity = 10,
    TestMsg = 11,
    /// <summary>
    /// 待机页面
    /// </summary>
    Standby,
    /// <summary>
    /// 解锁
    /// </summary>
    Unlock,
    /// <summary>
    /// 锁定
    /// </summary>
    Lock,
    /// <summary>
    /// 发射增益功能
    /// </summary>
    Shot,
    /// <summary>
    /// 发射增益功能
    /// </summary>
    G2C_Heartbeat
}
