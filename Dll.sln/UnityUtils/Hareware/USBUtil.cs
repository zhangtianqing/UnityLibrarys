using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;

namespace Dll.UnityUtils.Hareware { 
public class USBUtil
{

    /// <summary>
    /// Get SerialPort Name By vid and pid
    /// Only Win10 Test
    /// Default ArduinoUno Vid 2341 Pid 0043
    /// Default Ch340   Vid 1A86 Pid 7523
    /// </summary>
    /// <param name="VID">Serial USB VID</param>
    /// <param name="PID">Serial USB PID</param>
    /// <returns></returns>
    public static List<string> ComPortNamesByRegedit(String VID = "2341", String PID = "0043")
    {
        String pattern = String.Format("^VID_{0}.PID_{1}", VID, PID);
        Regex _rx = new Regex(pattern, RegexOptions.IgnoreCase);
        List<string> comports = new List<string>();

        RegistryKey rk1 = Registry.LocalMachine;
        RegistryKey rk2 = rk1.OpenSubKey(@"SYSTEM\CurrentControlSet\Enum");

        foreach (String s3 in rk2.GetSubKeyNames())
        {
            RegistryKey rk3 = rk2.OpenSubKey(s3);
            foreach (String s in rk3.GetSubKeyNames())
            {
                if (_rx.Match(s).Success)
                {
                    RegistryKey rk4 = rk3.OpenSubKey(s);
                    foreach (String s2 in rk4.GetSubKeyNames())
                    {
                        RegistryKey rk5 = rk4.OpenSubKey(s2);
                        string location = (string)rk5.GetValue("LocationInformation");
                        RegistryKey rk6 = rk5.OpenSubKey("Device Parameters");
                        string portName = (string)rk6.GetValue("PortName");
                        if (!String.IsNullOrEmpty(portName) && SerialPort.GetPortNames().Contains(portName))
                            comports.Add((string)rk6.GetValue("PortName"));
                    }
                }
            }
        }
        return comports;
    }

    #region UsbDevice
    /// <summary>
    /// ��ȡ���е�USB�豸ʵ�壨����û��VID��PID���豸��
    /// </summary>
    public static PnPEntityInfo[] AllUsbDevices
    {
        get
        {
            return WhoUsbDevice(UInt16.MinValue, UInt16.MinValue, Guid.Empty);
        }
    }

    /// <summary>
    /// ��ѯUSB�豸ʵ�壨�豸Ҫ����VID��PID��
    /// </summary>
    /// <param name="VendorID">��Ӧ�̱�ʶ��MinValue����</param>
    /// <param name="ProductID">��Ʒ��ţ�MinValue����</param>
    /// <param name="ClassGuid">�豸��װ��Guid��Empty����</param>
    /// <returns>�豸�б�</returns>
    public static PnPEntityInfo[] WhoUsbDevice(UInt16 VendorID, UInt16 ProductID, Guid ClassGuid)
    {
        List<PnPEntityInfo> UsbDevices = new List<PnPEntityInfo>();

        // ��ȡUSB������������������豸ʵ��
        ManagementObjectCollection USBControllerDeviceCollection = new ManagementObjectSearcher("SELECT * FROM Win32_USBControllerDevice").Get();
        if (USBControllerDeviceCollection != null)
        {
            foreach (ManagementObject USBControllerDevice in USBControllerDeviceCollection)
            {   // ��ȡ�豸ʵ���DeviceID
                String Dependent = (USBControllerDevice["Dependent"] as String).Split(new Char[] { '=' })[1];

                // ���˵�û��VID��PID��USB�豸
                Match match = Regex.Match(Dependent, "VID_[0-9|A-F]{4}&PID_[0-9|A-F]{4}");
                if (match.Success)
                {
                    UInt16 theVendorID = Convert.ToUInt16(match.Value.Substring(4, 4), 16);   // ��Ӧ�̱�ʶ
                    if (VendorID != UInt16.MinValue && VendorID != theVendorID) continue;

                    UInt16 theProductID = Convert.ToUInt16(match.Value.Substring(13, 4), 16); // ��Ʒ���
                    if (ProductID != UInt16.MinValue && ProductID != theProductID) continue;

                    ManagementObjectCollection PnPEntityCollection = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE DeviceID=" + Dependent).Get();
                    if (PnPEntityCollection != null)
                    {
                        foreach (ManagementObject Entity in PnPEntityCollection)
                        {
                            Guid theClassGuid = new Guid(Entity["ClassGuid"] as String);    // �豸��װ��GUID
                            if (ClassGuid != Guid.Empty && ClassGuid != theClassGuid) continue;

                            PnPEntityInfo Element;
                            Element.PNPDeviceID = Entity["PNPDeviceID"] as String;  // �豸ID
                            Element.Name = Entity["Name"] as String;                // �豸����
                            Element.Description = Entity["Description"] as String;  // �豸����
                            Element.Service = Entity["Service"] as String;          // ����
                            Element.Status = Entity["Status"] as String;            // �豸״̬
                            Element.VendorID = theVendorID;     // ��Ӧ�̱�ʶ
                            Element.ProductID = theProductID;   // ��Ʒ���
                            Element.ClassGuid = theClassGuid;   // �豸��װ��GUID

                            UsbDevices.Add(Element);
                        }
                    }
                }
            }
        }

        if (UsbDevices.Count == 0) return null; else return UsbDevices.ToArray();
    }

    /// <summary>
    /// ��ѯUSB�豸ʵ�壨�豸Ҫ����VID��PID��
    /// </summary>
    /// <param name="VendorID">��Ӧ�̱�ʶ��MinValue����</param>
    /// <param name="ProductID">��Ʒ��ţ�MinValue����</param>
    /// <returns>�豸�б�</returns>
    public static PnPEntityInfo[] WhoUsbDevice(UInt16 VendorID, UInt16 ProductID)
    {
        return WhoUsbDevice(VendorID, ProductID, Guid.Empty);
    }

    /// <summary>
    /// ��ѯUSB�豸ʵ�壨�豸Ҫ����VID��PID��
    /// </summary>
    /// <param name="ClassGuid">�豸��װ��Guid��Empty����</param>
    /// <returns>�豸�б�</returns>
    public static PnPEntityInfo[] WhoUsbDevice(Guid ClassGuid)
    {
        return WhoUsbDevice(UInt16.MinValue, UInt16.MinValue, ClassGuid);
    }

    /// <summary>
    /// ��ѯUSB�豸ʵ�壨�豸Ҫ����VID��PID��
    /// </summary>
    /// <param name="PNPDeviceID">�豸ID�������ǲ�������Ϣ</param>
    /// <returns>�豸�б�</returns>        
    public static PnPEntityInfo[] WhoUsbDevice(String PNPDeviceID)
    {
        List<PnPEntityInfo> UsbDevices = new List<PnPEntityInfo>();

        // ��ȡUSB������������������豸ʵ��
        ManagementObjectCollection USBControllerDeviceCollection = new ManagementObjectSearcher("SELECT * FROM Win32_USBControllerDevice").Get();
        if (USBControllerDeviceCollection != null)
        {
            foreach (ManagementObject USBControllerDevice in USBControllerDeviceCollection)
            {   // ��ȡ�豸ʵ���DeviceID
                String Dependent = (USBControllerDevice["Dependent"] as String).Split(new Char[] { '=' })[1];
                if (!String.IsNullOrEmpty(PNPDeviceID))
                {   // ע�⣺���Ӵ�Сд
                    if (Dependent.IndexOf(PNPDeviceID, 1, PNPDeviceID.Length - 2, StringComparison.OrdinalIgnoreCase) == -1) continue;
                }

                // ���˵�û��VID��PID��USB�豸
                Match match = Regex.Match(Dependent, "VID_[0-9|A-F]{4}&PID_[0-9|A-F]{4}");
                if (match.Success)
                {
                    ManagementObjectCollection PnPEntityCollection = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE DeviceID=" + Dependent).Get();
                    if (PnPEntityCollection != null)
                    {
                        foreach (ManagementObject Entity in PnPEntityCollection)
                        {
                            PnPEntityInfo Element;
                            Element.PNPDeviceID = Entity["PNPDeviceID"] as String;  // �豸ID
                            Element.Name = Entity["Name"] as String;                // �豸����
                            Element.Description = Entity["Description"] as String;  // �豸����
                            Element.Service = Entity["Service"] as String;          // ����
                            Element.Status = Entity["Status"] as String;            // �豸״̬
                            Element.VendorID = Convert.ToUInt16(match.Value.Substring(4, 4), 16);   // ��Ӧ�̱�ʶ   
                            Element.ProductID = Convert.ToUInt16(match.Value.Substring(13, 4), 16); // ��Ʒ���                         // ��Ʒ���
                            Element.ClassGuid = new Guid(Entity["ClassGuid"] as String);            // �豸��װ��GUID

                            UsbDevices.Add(Element);
                        }
                    }
                }
            }
        }

        if (UsbDevices.Count == 0) return null; else return UsbDevices.ToArray();
    }

    /// <summary>
    /// ���ݷ���λUSB�豸
    /// </summary>
    /// <param name="ServiceCollection">Ҫ��ѯ�ķ��񼯺�</param>
    /// <returns>�豸�б�</returns>
    public static PnPEntityInfo[] WhoUsbDevice(String[] ServiceCollection)
    {
        if (ServiceCollection == null || ServiceCollection.Length == 0)
            return WhoUsbDevice(UInt16.MinValue, UInt16.MinValue, Guid.Empty);

        List<PnPEntityInfo> UsbDevices = new List<PnPEntityInfo>();

        // ��ȡUSB������������������豸ʵ��
        ManagementObjectCollection USBControllerDeviceCollection = new ManagementObjectSearcher("SELECT * FROM Win32_USBControllerDevice").Get();
        if (USBControllerDeviceCollection != null)
        {
            foreach (ManagementObject USBControllerDevice in USBControllerDeviceCollection)
            {   // ��ȡ�豸ʵ���DeviceID
                String Dependent = (USBControllerDevice["Dependent"] as String).Split(new Char[] { '=' })[1];

                // ���˵�û��VID��PID��USB�豸
                Match match = Regex.Match(Dependent, "VID_[0-9|A-F]{4}&PID_[0-9|A-F]{4}");
                if (match.Success)
                {
                    ManagementObjectCollection PnPEntityCollection = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE DeviceID=" + Dependent).Get();
                    if (PnPEntityCollection != null)
                    {
                        foreach (ManagementObject Entity in PnPEntityCollection)
                        {
                            String theService = Entity["Service"] as String;          // ����
                            if (String.IsNullOrEmpty(theService)) continue;

                            foreach (String Service in ServiceCollection)
                            {   // ע�⣺���Ӵ�Сд
                                if (String.Compare(theService, Service, true) != 0) continue;

                                PnPEntityInfo Element;
                                Element.PNPDeviceID = Entity["PNPDeviceID"] as String;  // �豸ID
                                Element.Name = Entity["Name"] as String;                // �豸����
                                Element.Description = Entity["Description"] as String;  // �豸����
                                Element.Service = theService;                           // ����
                                Element.Status = Entity["Status"] as String;            // �豸״̬
                                Element.VendorID = Convert.ToUInt16(match.Value.Substring(4, 4), 16);   // ��Ӧ�̱�ʶ   
                                Element.ProductID = Convert.ToUInt16(match.Value.Substring(13, 4), 16); // ��Ʒ���
                                Element.ClassGuid = new Guid(Entity["ClassGuid"] as String);            // �豸��װ��GUID

                                UsbDevices.Add(Element);
                                break;
                            }
                        }
                    }
                }
            }
        }

        if (UsbDevices.Count == 0) return null; else return UsbDevices.ToArray();
    }
    #endregion

    #region PnPEntity
    /// <summary>
    /// ���м��弴���豸ʵ�壨����û��VID��PID���豸��
    /// </summary>
    public static PnPEntityInfo[] AllPnPEntities
    {
        get
        {
            return WhoPnPEntity(UInt16.MinValue, UInt16.MinValue, Guid.Empty);
        }
    }

    /// <summary>
    /// ����VID��PID���豸��װ��GUID��λ���弴���豸ʵ��
    /// </summary>
    /// <param name="VendorID">��Ӧ�̱�ʶ��MinValue����</param>
    /// <param name="ProductID">��Ʒ��ţ�MinValue����</param>
    /// <param name="ClassGuid">�豸��װ��Guid��Empty����</param>
    /// <returns>�豸�б�</returns>
    /// <remarks>
    /// HID��{745a17a0-74d3-11d0-b6fe-00a0c90f57da}
    /// Imaging Device��{6bdd1fc6-810f-11d0-bec7-08002be2092f}
    /// Keyboard��{4d36e96b-e325-11ce-bfc1-08002be10318} 
    /// Mouse��{4d36e96f-e325-11ce-bfc1-08002be10318}
    /// Network Adapter��{4d36e972-e325-11ce-bfc1-08002be10318}
    /// USB��{36fc9e60-c465-11cf-8056-444553540000}
    /// </remarks>
    public static PnPEntityInfo[] WhoPnPEntity(UInt16 VendorID, UInt16 ProductID, Guid ClassGuid)
    {
        List<PnPEntityInfo> PnPEntities = new List<PnPEntityInfo>();

        // ö�ټ��弴���豸ʵ��
        String VIDPID;
        if (VendorID == UInt16.MinValue)
        {
            if (ProductID == UInt16.MinValue)
                VIDPID = "'%VID[_]____&PID[_]____%'";
            else
                VIDPID = "'%VID[_]____&PID[_]" + ProductID.ToString("X4") + "%'";
        }
        else
        {
            if (ProductID == UInt16.MinValue)
                VIDPID = "'%VID[_]" + VendorID.ToString("X4") + "&PID[_]____%'";
            else
                VIDPID = "'%VID[_]" + VendorID.ToString("X4") + "&PID[_]" + ProductID.ToString("X4") + "%'";
        }

        String QueryString;
        if (ClassGuid == Guid.Empty)
            QueryString = "SELECT * FROM Win32_PnPEntity WHERE PNPDeviceID LIKE" + VIDPID;
        else
            QueryString = "SELECT * FROM Win32_PnPEntity WHERE PNPDeviceID LIKE" + VIDPID + " AND ClassGuid='" + ClassGuid.ToString("B") + "'";

        ManagementObjectCollection PnPEntityCollection = new ManagementObjectSearcher(QueryString).Get();
        if (PnPEntityCollection != null)
        {
            foreach (ManagementObject Entity in PnPEntityCollection)
            {
                String PNPDeviceID = Entity["PNPDeviceID"] as String;
                Match match = Regex.Match(PNPDeviceID, "VID_[0-9|A-F]{4}&PID_[0-9|A-F]{4}");
                if (match.Success)
                {
                    PnPEntityInfo Element;

                    Element.PNPDeviceID = PNPDeviceID;                      // �豸ID
                    Element.Name = Entity["Name"] as String;                // �豸����
                    Element.Description = Entity["Description"] as String;  // �豸����
                    Element.Service = Entity["Service"] as String;          // ����
                    Element.Status = Entity["Status"] as String;            // �豸״̬
                    Element.VendorID = Convert.ToUInt16(match.Value.Substring(4, 4), 16);   // ��Ӧ�̱�ʶ
                    Element.ProductID = Convert.ToUInt16(match.Value.Substring(13, 4), 16); // ��Ʒ���
                    Element.ClassGuid = new Guid(Entity["ClassGuid"] as String);            // �豸��װ��GUID

                    PnPEntities.Add(Element);
                }
            }
        }

        if (PnPEntities.Count == 0) return null; else return PnPEntities.ToArray();
    }

    /// <summary>
    /// ����VID��PID��λ���弴���豸ʵ��
    /// </summary>
    /// <param name="VendorID">��Ӧ�̱�ʶ��MinValue����</param>
    /// <param name="ProductID">��Ʒ��ţ�MinValue����</param>
    /// <returns>�豸�б�</returns>
    public static PnPEntityInfo[] WhoPnPEntity(UInt16 VendorID, UInt16 ProductID)
    {
        return WhoPnPEntity(VendorID, ProductID, Guid.Empty);
    }

    /// <summary>
    /// �����豸��װ��GUID��λ���弴���豸ʵ��
    /// </summary>
    /// <param name="ClassGuid">�豸��װ��Guid��Empty����</param>
    /// <returns>�豸�б�</returns>
    public static PnPEntityInfo[] WhoPnPEntity(Guid ClassGuid)
    {
        return WhoPnPEntity(UInt16.MinValue, UInt16.MinValue, ClassGuid);
    }

    /// <summary>
    /// �����豸ID��λ�豸
    /// </summary>
    /// <param name="PNPDeviceID">�豸ID�������ǲ�������Ϣ</param>
    /// <returns>�豸�б�</returns>
    /// <remarks>
    /// ע�⣺�����»��ߣ���Ҫд�ɡ�[_]����������Ϊ�����ַ�
    /// </remarks>
    public static PnPEntityInfo[] WhoPnPEntity(String PNPDeviceID)
    {
        List<PnPEntityInfo> PnPEntities = new List<PnPEntityInfo>();

        // ö�ټ��弴���豸ʵ��
        String QueryString;
        if (String.IsNullOrEmpty(PNPDeviceID))
        {
            QueryString = "SELECT * FROM Win32_PnPEntity WHERE PNPDeviceID LIKE '%VID[_]____&PID[_]____%'";
        }
        else
        {   // LIKE�Ӿ����з�б���ַ���������WQL��ѯ�쳣
            QueryString = "SELECT * FROM Win32_PnPEntity WHERE PNPDeviceID LIKE '%" + PNPDeviceID.Replace('\\', '_') + "%'";
        }

        ManagementObjectCollection PnPEntityCollection = new ManagementObjectSearcher(QueryString).Get();
        if (PnPEntityCollection != null)
        {
            foreach (ManagementObject Entity in PnPEntityCollection)
            {
                String thePNPDeviceID = Entity["PNPDeviceID"] as String;
                Match match = Regex.Match(thePNPDeviceID, "VID_[0-9|A-F]{4}&PID_[0-9|A-F]{4}");
                if (match.Success)
                {
                    PnPEntityInfo Element;

                    Element.PNPDeviceID = thePNPDeviceID;                   // �豸ID
                    Element.Name = Entity["Name"] as String;                // �豸����
                    Element.Description = Entity["Description"] as String;  // �豸����
                    Element.Service = Entity["Service"] as String;          // ����
                    Element.Status = Entity["Status"] as String;            // �豸״̬
                    Element.VendorID = Convert.ToUInt16(match.Value.Substring(4, 4), 16);   // ��Ӧ�̱�ʶ
                    Element.ProductID = Convert.ToUInt16(match.Value.Substring(13, 4), 16); // ��Ʒ���
                    Element.ClassGuid = new Guid(Entity["ClassGuid"] as String);            // �豸��װ��GUID

                    PnPEntities.Add(Element);
                }
            }
        }

        if (PnPEntities.Count == 0) return null; else return PnPEntities.ToArray();
    }

    /// <summary>
    /// ���ݷ���λ�豸
    /// </summary>
    /// <param name="ServiceCollection">Ҫ��ѯ�ķ��񼯺ϣ�null����</param>
    /// <returns>�豸�б�</returns>
    /// <remarks>
    /// ��������ص��ࣺ
    ///     Win32_SystemDriverPNPEntity
    ///     Win32_SystemDriver
    /// </remarks>
    public static PnPEntityInfo[] WhoPnPEntity(String[] ServiceCollection)
    {
        if (ServiceCollection == null || ServiceCollection.Length == 0)
            return WhoPnPEntity(UInt16.MinValue, UInt16.MinValue, Guid.Empty);

        List<PnPEntityInfo> PnPEntities = new List<PnPEntityInfo>();

        // ö�ټ��弴���豸ʵ��
        String QueryString = "SELECT * FROM Win32_PnPEntity WHERE PNPDeviceID LIKE '%VID[_]____&PID[_]____%'";
        ManagementObjectCollection PnPEntityCollection = new ManagementObjectSearcher(QueryString).Get();
        if (PnPEntityCollection != null)
        {
            foreach (ManagementObject Entity in PnPEntityCollection)
            {
                String PNPDeviceID = Entity["PNPDeviceID"] as String;
                Match match = Regex.Match(PNPDeviceID, "VID_[0-9|A-F]{4}&PID_[0-9|A-F]{4}");
                if (match.Success)
                {
                    String theService = Entity["Service"] as String;            // ����
                    if (String.IsNullOrEmpty(theService)) continue;

                    foreach (String Service in ServiceCollection)
                    {   // ע�⣺���Ӵ�Сд
                        if (String.Compare(theService, Service, true) != 0) continue;

                        PnPEntityInfo Element;

                        Element.PNPDeviceID = PNPDeviceID;                      // �豸ID
                        Element.Name = Entity["Name"] as String;                // �豸����
                        Element.Description = Entity["Description"] as String;  // �豸����
                        Element.Service = theService;                           // ����
                        Element.Status = Entity["Status"] as String;            // �豸״̬
                        Element.VendorID = Convert.ToUInt16(match.Value.Substring(4, 4), 16);   // ��Ӧ�̱�ʶ
                        Element.ProductID = Convert.ToUInt16(match.Value.Substring(13, 4), 16); // ��Ʒ���
                        Element.ClassGuid = new Guid(Entity["ClassGuid"] as String);            // �豸��װ��GUID

                        PnPEntities.Add(Element);
                        break;
                    }
                }
            }
        }

        if (PnPEntities.Count == 0) return null; else return PnPEntities.ToArray();
    }
    #endregion
    /// <summary>
    /// ���弴���豸��Ϣ�ṹ
    /// </summary>
    public struct PnPEntityInfo
    {
        public String PNPDeviceID;      // �豸ID
        public String Name;             // �豸����
        public String Description;      // �豸����
        public String Service;          // ����
        public String Status;           // �豸״̬
        public UInt16 VendorID;         // ��Ӧ�̱�ʶ
        public UInt16 ProductID;        // ��Ʒ��� 
        public Guid ClassGuid;          // �豸��װ��GUID
    }
}
}