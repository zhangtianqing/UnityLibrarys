using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Client Ping ������
/// </summary>
public enum ActionCode
{
    None = -1,
    /// <summary>
    /// ��ѯ����
    /// </summary>
    Ping = 0,
    Pong = -2,
    /// <summary>
    /// ��ݰ�ר��
    /// </summary>
    BindIdentity = 10,
    TestMsg = 11,
    /// <summary>
    /// ����ҳ��
    /// </summary>
    Standby,
    /// <summary>
    /// ����
    /// </summary>
    Unlock,
    /// <summary>
    /// ����
    /// </summary>
    Lock,
    /// <summary>
    /// �������湦��
    /// </summary>
    Shot,
    /// <summary>
    /// �������湦��
    /// </summary>
    G2C_Heartbeat
}
