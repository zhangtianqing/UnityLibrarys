using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//#if UNITY_ANDROID
//using UnityEngine.Android;
//#endif

namespace Assets.Scripts.Framework.Utils.Android
{
    public class AndroidPermission
    {
        //        // Start is called before the first frame update
        //        public static void RequestPermission()
        //        {
        //#if UNITY_ANDROID
        //            InitPermissionEnumKey();
        //            permissionEnum.Clear();
        //            foreach (var item in keyValuePairs)
        //            {
        //                permissionEnum.Add(item.Key);
        //            }
        //            CheckPremissions();
        //#endif
        //        }

        //        static List<PermissionEnum> permissionEnum = new List<PermissionEnum>();
        //        static Dictionary<PermissionEnum, string> keyValuePairs = new Dictionary<PermissionEnum, string>();

        //        static void CheckPremissions()
        //        {
        //            foreach (PermissionEnum permission in permissionEnum)
        //            {
        //                //����1 ͨ��Permission��ȡ
        //                if (!Permission.HasUserAuthorizedPermission(keyValuePairs[permission]))
        //                {
        //                    //Debug.Log("����Ȩ�ޣ�" + keyValuePairs[permission]);
        //                    Permission.RequestUserPermission(keyValuePairs[permission]);
        //                }
        //                else
        //                {
        //                    //Debug.Log($"Ȩ���ѻ�ȡ:{keyValuePairs[permission]}");
        //                }
        //            }
        //        }
        //        /// <summary>
        //        /// ת�շ巨
        //        /// </summary>
        //        /// <param name="str"></param>
        //        /// <returns></returns>
        //        private static string ToPascal(string str)
        //        {
        //            string[] split = str.Trim().Split(new char[] { '/', ' ', '_', '.', '-' });
        //            string newStr = "";
        //            foreach (var item in split)
        //            {

        //                char[] chars = item.ToCharArray();
        //                chars[0] = char.ToUpper(chars[0]);
        //                for (int i = 1; i < chars.Length; i++)
        //                {
        //                    chars[i] = char.ToLower(chars[i]);
        //                }
        //                newStr += new string(chars);
        //            }
        //            return newStr;
        //        }


        //        private static void ��ȡ�����ַ���()
        //        {
        //            string str = "���ʵǼ�����\tandroid.permission.ACCESS_CHECKIN_PROPERTIES ����ȡ��д��Ǽ�check-in���ݿ����Ա��Ȩ��\r\n��ȡ����λ��\tandroid.permission.ACCESS_COARSE_LOCATION��ͨ��WiFi���ƶ���վ�ķ�ʽ��ȡ�û����Եľ�γ����Ϣ����λ���ȴ�������30~1500��\r\n��ȡ��ȷλ��\tandroid.permission.ACCESS_FINE_LOCATION��ͨ��GPSоƬ�������ǵĶ�λ��Ϣ����λ���ȴ�10������\r\n���ʶ�λ��������\tandroid.permission.ACCESS_LOCATION_EXTRA_COMMANDS�����������ʶ���Ķ�λ�ṩ��ָ��\r\n��ȡģ�ⶨλ��Ϣ\tandroid.permission.ACCESS_MOCK_LOCATION����ȡģ�ⶨλ��Ϣ��һ�����ڰ��������ߵ���Ӧ��\r\n��ȡ����״̬\tandroid.permission.ACCESS_NETWORK_STATE����ȡ������Ϣ״̬���統ǰ�����������Ƿ���Ч\r\n����Surface Flinger\tandroid.permission.ACCESS_SURFACE_FLINGER��Androidƽ̨�ϵײ��ͼ����ʾ֧�֣�һ��������Ϸ�������Ԥ������͵ײ�ģʽ����Ļ��ͼ\r\n��ȡWiFi״̬\tandroid.permission.ACCESS_WIFI_STATE����ȡ��ǰWiFi�����״̬�Լ�WLAN�ȵ����Ϣ\r\n�˻�����\tandroid.permission.ACCOUNT_MANAGER����ȡ�˻���֤��Ϣ����ҪΪGMail�˻���Ϣ��ֻ��ϵͳ�����̲��ܷ��ʵ�Ȩ��\r\n��֤�˻�\tandroid.permission.AUTHENTICATE_ACCOUNTS������һ������ͨ���˻���֤��ʽ�����˻�����ACCOUNT_MANAGER�����Ϣ\r\n����ͳ��\tandroid.permission.BATTERY_STATS����ȡ��ص���ͳ����Ϣ\r\n��С���\tandroid.permission.BIND_APPWIDGET������һ���������appWidget������Ҫ����С��������ݿ⣬ֻ�зǳ��ٵ�Ӧ�ò��õ���Ȩ��\r\n���豸����\tandroid.permission.BIND_DEVICE_ADMIN������ϵͳ����Ա������receiver��ֻ��ϵͳ����ʹ��\r\n�����뷨\tandroid.permission.BIND_INPUT_METHOD ������InputMethodService����ֻ��ϵͳ����ʹ��\r\n��RemoteView\tandroid.permission.BIND_REMOTEVIEWS������ͨ��RemoteViewsService����������ֻ��ϵͳ������\r\n�󶨱�ֽ\tandroid.permission.BIND_WALLPAPER������ͨ��WallpaperService����������ֻ��ϵͳ������\r\nʹ������\tandroid.permission.BLUETOOTH���������������Թ��������豸\r\n��������\tandroid.permission.BLUETOOTH_ADMIN�����������з��ֺ�����µ������豸\r\n���שͷ\tandroid.permission.BRICK���ܹ������ֻ����ǳ�Σ�գ�����˼��������ֻ����שͷ\r\nӦ��ɾ��ʱ�㲥\tandroid.permission.BROADCAST_PACKAGE_REMOVED����һ��Ӧ����ɾ��ʱ����һ���㲥\r\n�յ�����ʱ�㲥\tandroid.permission.BROADCAST_SMS�����յ�����ʱ����һ���㲥\r\n�����㲥\tandroid.permission.BROADCAST_STICKY������һ�������յ��㲥������յ���һ���㲥\r\nWAP PUSH�㲥\tandroid.permission.BROADCAST_WAP_PUSH��WAP PUSH�����յ��󴥷�һ���㲥\r\n����绰\tandroid.permission.CALL_PHONE���������ӷ�ϵͳ������������绰����\r\nͨ��Ȩ��\tandroid.permission.CALL_PRIVILEGED��������򲦴�绰���滻ϵͳ�Ĳ���������\r\n����Ȩ��\tandroid.permission.CAMERA�������������ͷ��������\r\n�ı����״̬\tandroid.permission.CHANGE_COMPONENT_ENABLED_STATE���ı�����Ƿ�����״̬\r\n�ı�����\tandroid.permission.CHANGE_CONFIGURATION������ǰӦ�øı����ã��綨λ\r\n�ı�����״̬\tandroid.permission.CHANGE_NETWORK_STATE���ı�����״̬���Ƿ�������\r\n�ı�WiFi�ಥ״̬\tandroid.permission.CHANGE_WIFI_MULTICAST_STATE���ı�WiFi�ಥ״̬\r\n�ı�WiFi״̬\tandroid.permission.CHANGE_WIFI_STATE���ı�WiFi״̬\r\n���Ӧ�û���\tandroid.permission.CLEAR_APP_CACHE�����Ӧ�û���\r\n����û�����\tandroid.permission.CLEAR_APP_USER_DATA�����Ӧ�õ��û�����\r\n�ײ����Ȩ��\tandroid.permission.CWJ_GROUP������CWJ�˻�����ʵײ���Ϣ\r\n�ֻ��Ż���ʦ��չȨ��\tandroid.permission.CELL_PHONE_MASTER_EX���ֻ��Ż���ʦ��չȨ��\r\n���ƶ�λ����\tandroid.permission.CONTROL_LOCATION_UPDATES���������ƶ����綨λ��Ϣ�ı�\r\nɾ�������ļ�\tandroid.permission.DELETE_CACHE_FILES������Ӧ��ɾ�������ļ�\r\nɾ��Ӧ��\tandroid.permission.DELETE_PACKAGES���������ɾ��Ӧ��\r\n��Դ����\tandroid.permission.DEVICE_POWER��������ʵײ��Դ����\r\nӦ�����\tandroid.permission.DIAGNOSTIC���������RW�������Դ\r\n���ü�����\tandroid.permission.DISABLE_KEYGUARD�����������ü�����\r\nת��ϵͳ��Ϣ\tandroid.permission.DUMP����������ȡϵͳdump��Ϣ��ϵͳ����\r\n״̬������\tandroid.permission.EXPAND_STATUS_BAR�����������չ������״̬��\r\n��������ģʽ\tandroid.permission.FACTORY_TEST������������й�������ģʽ\r\nʹ�������\tandroid.permission.FLASHLIGHT��������������\r\nǿ�ƺ���\tandroid.permission.FORCE_BACK���������ǿ��ʹ��back���˰���������Activity�Ƿ��ڶ���\r\n�����˻�Gmail�б�\tandroid.permission.GET_ACCOUNTS������GMail�˻��б�\r\n��ȡӦ�ô�С\tandroid.permission.GET_PACKAGE_SIZE����ȡӦ�õ��ļ���С\r\n��ȡ������Ϣ\tandroid.permission.GET_TASKS����������ȡ��ǰ��������е�Ӧ��\r\n����ȫ������\tandroid.permission.GLOBAL_SEARCH���������ʹ��ȫ����������\r\nӲ������\tandroid.permission.HARDWARE_TEST������Ӳ�������豸������Ӳ������\r\nע���¼�\tandroid.permission.INJECT_EVENTS��������ʱ�����ĵײ��¼�����ȡ�������켣����¼���\r\n��װ��λ�ṩ\tandroid.permission.INSTALL_LOCATION_PROVIDER����װ��λ�ṩ\r\n��װӦ�ó���\tandroid.permission.INSTALL_PACKAGES���������װӦ��\r\n�ڲ�ϵͳ����\tandroid.permission.INTERNAL_SYSTEM_WINDOW�����������ڲ����ڣ����Ե�����Ӧ�ó��򿪷Ŵ�Ȩ��\r\n��������\tandroid.permission.INTERNET�������������ӣ����ܲ���GPRS����\r\n������̨����\tandroid.permission.KILL_BACKGROUND_PROCESSES������������killBackgroundProcesses(String).����������̨����\r\n�����˻�\tandroid.permission.MANAGE_ACCOUNTS������������AccountManager�е��˻��б�\r\n�����������\tandroid.permission.MANAGE_APP_TOKENS�����������ݻ١�Z��˳�򣬽�����ϵͳ\r\n�߼�Ȩ��\tandroid.permission.MTWEAK_USER������mTweak�û����ʸ߼�ϵͳȨ��\r\n����Ȩ��\tandroid.permission.MTWEAK_FORUM������ʹ��mTweak����Ȩ��\r\n���ʽ��\tandroid.permission.MASTER_CLEAR���������ִ�����ʽ����ɾ��ϵͳ������Ϣ\r\n�޸���������\tandroid.permission.MODIFY_AUDIO_SETTINGS���޸�����������Ϣ\r\n�޸ĵ绰״̬\tandroid.permission.MODIFY_PHONE_STATE���޸ĵ绰״̬�������ģʽ�����������滻ϵͳ����������\r\n��ʽ���ļ�ϵͳ\tandroid.permission.MOUNT_FORMAT_FILESYSTEMS����ʽ�����ƶ��ļ�ϵͳ�������ʽ�����SD��\r\n�����ļ�ϵͳ\tandroid.permission.MOUNT_UNMOUNT_FILESYSTEMS�����ء��������ⲿ�ļ�ϵͳ\r\n����NFCͨѶ\tandroid.permission.NFC���������ִ��NFC������ͨѶ�����������ƶ�֧��\r\n����Activity\tandroid.permission.PERSISTENT_ACTIVITY������һ�����õ�Activity���ù��ܱ��Ϊ���������Ƴ�\r\n�������绰\tandroid.permission.PROCESS_OUTGOING_CALLS�����������ӣ��޸Ļ���������绰\r\n��ȡ�ճ�����\tandroid.permission.READ_CALENDAR����������ȡ�û����ճ���Ϣ\r\n��ȡ��ϵ��\tandroid.permission.READ_CONTACTS������Ӧ�÷�����ϵ��ͨѶ¼��Ϣ\r\n��Ļ��ͼ\tandroid.permission.READ_FRAME_BUFFER����ȡ֡����������Ļ��ͼ\r\n��ȡ�ղؼк���ʷ��¼\tcom.android.browser.permission.READ_HISTORY_BOOKMARKS����ȡ������ղؼк���ʷ��¼\r\n��ȡ����״̬\tandroid.permission.READ_INPUT_STATE����ȡ��ǰ��������״̬��������ϵͳ\r\n��ȡϵͳ��־\tandroid.permission.READ_LOGS����ȡϵͳ�ײ���־\r\n��ȡ�绰״̬\tandroid.permission.READ_PHONE_STATE�����ʵ绰״̬\r\n��ȡ��������\tandroid.permission.READ_SMS����ȡ��������\r\n��ȡͬ������\tandroid.permission.READ_SYNC_SETTINGS����ȡͬ�����ã���ȡGoogle����ͬ������\r\n��ȡͬ��״̬\tandroid.permission.READ_SYNC_STATS����ȡͬ��״̬�����Google����ͬ��״̬\r\n�����豸\tandroid.permission.REBOOT������������������豸\r\n�����Զ�����\tandroid.permission.RECEIVE_BOOT_COMPLETED��������򿪻��Զ�����\r\n���ղ���\tandroid.permission.RECEIVE_MMS�����ղ���\r\n���ն���\tandroid.permission.RECEIVE_SMS�����ն���\r\n����Wap Push\tandroid.permission.RECEIVE_WAP_PUSH������WAP PUSH��Ϣ\r\n¼��\tandroid.permission.RECORD_AUDIO��¼������ͨ���ֻ�����������\r\n����ϵͳ����\tandroid.permission.REORDER_TASKS����������ϵͳZ�������е�����\r\n����ϵͳ����\tandroid.permission.RESTART_PACKAGES����������ͨ��restartPackage(String)�������÷�ʽ������������\r\n���Ͷ���\tandroid.permission.SEND_SMS�����Ͷ���\r\n����Activity�۲���\tandroid.permission.SET_ACTIVITY_WATCHER������Activity�۲���һ������monkey����\r\n������������\tcom.android.alarm.permission.SET_ALARM��������������\r\n���������˳�\tandroid.permission.SET_ALWAYS_FINISH�����ó����ں�̨�Ƿ������˳�\r\n���ö�������\tandroid.permission.SET_ANIMATION_SCALE������ȫ�ֶ�������\r\n���õ��Գ���\tandroid.permission.SET_DEBUG_APP�����õ��Գ���һ�����ڿ���\r\n������Ļ����\tandroid.permission.SET_ORIENTATION��������Ļ����Ϊ�������׼��ʽ��ʾ����������ͨӦ��\r\n����Ӧ�ò���\tandroid.permission.SET_PREFERRED_APPLICATIONS������Ӧ�õĲ������Ѳ��ٹ�������鿴addPackageToPreferred(String) ����\r\n���ý�������\tandroid.permission.SET_PROCESS_LIMIT����������������Ľ�������������\r\n����ϵͳʱ��\tandroid.permission.SET_TIME������ϵͳʱ��\r\n����ϵͳʱ��\tandroid.permission.SET_TIME_ZONE������ϵͳʱ��\r\n���������ֽ\tandroid.permission.SET_WALLPAPER�����������ֽ\r\n���ñ�ֽ����\tandroid.permission.SET_WALLPAPER_HINTS�����ñ�ֽ����\r\n�������ý����ź�\tandroid.permission.SIGNAL_PERSISTENT_PROCESSES������һ�����õĽ����ź�\r\n״̬������\tandroid.permission.STATUS_BAR���������򿪡��رա�����״̬��\r\n���ʶ�������\tandroid.permission.SUBSCRIBED_FEEDS_READ�����ʶ�����Ϣ�����ݿ�\r\nд�붩������\tandroid.permission.SUBSCRIBED_FEEDS_WRITE��д����޸Ķ������ݵ����ݿ�\r\n��ʾϵͳ����\tandroid.permission.SYSTEM_ALERT_WINDOW����ʾϵͳ����\r\n�����豸״̬\tandroid.permission.UPDATE_DEVICE_STATS�������豸״̬\r\nʹ��֤��\tandroid.permission.USE_CREDENTIALS���������������֤��AccountManager\r\nʹ��SIP��Ƶ\tandroid.permission.USE_SIP���������ʹ��SIP��Ƶ����\r\nʹ����\tandroid.permission.VIBRATE��������\r\n��������\tandroid.permission.WAKE_LOCK������������ֻ���Ļ�رպ��̨������Ȼ����\r\nд��GPRS���������\tandroid.permission.WRITE_APN_SETTINGS��д������GPRS���������\r\nд���ճ�����\tandroid.permission.WRITE_CALENDAR��д���ճ̣������ɶ�ȡ\r\nд����ϵ��\tandroid.permission.WRITE_CONTACTS��д����ϵ�ˣ������ɶ�ȡ\r\nд���ⲿ�洢\tandroid.permission.WRITE_EXTERNAL_STORAGE���������д���ⲿ�洢����SD����д�ļ�\r\nд��Google��ͼ����\tandroid.permission.WRITE_GSERVICES���������д��Google Map��������\r\nд���ղؼк���ʷ��¼\tcom.android.browser.permission.WRITE_HISTORY_BOOKMARKS��д���������ʷ��¼���ղؼУ������ɶ�ȡ\r\n��дϵͳ��������\tandroid.permission.WRITE_SECURE_SETTINGS����������дϵͳ��ȫ���е�������\r\n��дϵͳ����\tandroid.permission.WRITE_SETTINGS�������дϵͳ������\r\n��д����\tandroid.permission.WRITE_SMS�������д����\r\nд������ͬ������\tandroid.permission.WRITE_SYNC_SETTINGS��д��Google����ͬ������";
        //            string[] lines = str.Split('\n');
        //            string Enum = "";
        //            string initEnumStr = "";
        //            foreach (string line in lines)
        //            {
        //                string[] content = line.Split('\t');
        //                string[] stringK = content[1].Split('��');
        //                Debug.Log(stringK[0]);

        //                Enum += "/// <summary>" + "\n" +
        //                $"/// {content[0]}" + "\n" +
        //                $"/// {stringK[1]}" + "\n" +
        //                "/// </summary>" + "\n" +
        //               ToPascal(stringK[0].Split('.')[stringK[0].Split('.').Length - 1]) + ",\n";

        //                initEnumStr += " keyValuePairs.Add(PermissionEnum." + ToPascal(stringK[0].Split('.')[stringK[0].Split('.').Length - 1]) + $",\"{stringK[0]}\");\n";
        //                //Enum += ToPascal(stringK[0].Split(".")[stringK[0].Split(".").Length - 1]) + "\n"; //stringK[0].Split(".")[stringK[0].Split(".").Length - 1]
        //                //Debug.Log($"Ȩ������:{content[0]},Ȩ���ַ���:{stringK[0]}��Ȩ��˵��:{stringK[1]}");
        //            }
        //            Debug.Log(Enum);
        //            Debug.Log(initEnumStr);
        //            return;
        //        }

        //        private static void InitPermissionEnumKey()
        //        {
        //            keyValuePairs.Clear();
        //            keyValuePairs.Add(PermissionEnum.AccessCheckinProperties, "android.permission.ACCESS_CHECKIN_PROPERTIES ");
        //            keyValuePairs.Add(PermissionEnum.AccessCoarseLocation, "android.permission.ACCESS_COARSE_LOCATION");
        //            keyValuePairs.Add(PermissionEnum.AccessFineLocation, "android.permission.ACCESS_FINE_LOCATION");
        //            keyValuePairs.Add(PermissionEnum.AccessLocationExtraCommands, "android.permission.ACCESS_LOCATION_EXTRA_COMMANDS");
        //            keyValuePairs.Add(PermissionEnum.AccessMockLocation, "android.permission.ACCESS_MOCK_LOCATION");
        //            keyValuePairs.Add(PermissionEnum.AccessNetworkState, "android.permission.ACCESS_NETWORK_STATE");
        //            keyValuePairs.Add(PermissionEnum.AccessSurfaceFlinger, "android.permission.ACCESS_SURFACE_FLINGER");
        //            keyValuePairs.Add(PermissionEnum.AccessWifiState, "android.permission.ACCESS_WIFI_STATE");
        //            keyValuePairs.Add(PermissionEnum.AccountManager, "android.permission.ACCOUNT_MANAGER");
        //            keyValuePairs.Add(PermissionEnum.AuthenticateAccounts, "android.permission.AUTHENTICATE_ACCOUNTS");
        //            keyValuePairs.Add(PermissionEnum.BatteryStats, "android.permission.BATTERY_STATS");
        //            keyValuePairs.Add(PermissionEnum.BindAppwidget, "android.permission.BIND_APPWIDGET");
        //            keyValuePairs.Add(PermissionEnum.BindDeviceAdmin, "android.permission.BIND_DEVICE_ADMIN");
        //            keyValuePairs.Add(PermissionEnum.BindInputMethod, "android.permission.BIND_INPUT_METHOD ");
        //            keyValuePairs.Add(PermissionEnum.BindRemoteviews, "android.permission.BIND_REMOTEVIEWS");
        //            keyValuePairs.Add(PermissionEnum.BindWallpaper, "android.permission.BIND_WALLPAPER");
        //            keyValuePairs.Add(PermissionEnum.Bluetooth, "android.permission.BLUETOOTH");
        //            keyValuePairs.Add(PermissionEnum.BluetoothAdmin, "android.permission.BLUETOOTH_ADMIN");
        //            keyValuePairs.Add(PermissionEnum.Brick, "android.permission.BRICK");
        //            keyValuePairs.Add(PermissionEnum.BroadcastPackageRemoved, "android.permission.BROADCAST_PACKAGE_REMOVED");
        //            keyValuePairs.Add(PermissionEnum.BroadcastSms, "android.permission.BROADCAST_SMS");
        //            keyValuePairs.Add(PermissionEnum.BroadcastSticky, "android.permission.BROADCAST_STICKY");
        //            keyValuePairs.Add(PermissionEnum.BroadcastWapPush, "android.permission.BROADCAST_WAP_PUSH");
        //            keyValuePairs.Add(PermissionEnum.CallPhone, "android.permission.CALL_PHONE");
        //            keyValuePairs.Add(PermissionEnum.CallPrivileged, "android.permission.CALL_PRIVILEGED");
        //            keyValuePairs.Add(PermissionEnum.Camera, "android.permission.CAMERA");
        //            keyValuePairs.Add(PermissionEnum.ChangeComponentEnabledState, "android.permission.CHANGE_COMPONENT_ENABLED_STATE");
        //            keyValuePairs.Add(PermissionEnum.ChangeConfiguration, "android.permission.CHANGE_CONFIGURATION");
        //            keyValuePairs.Add(PermissionEnum.ChangeNetworkState, "android.permission.CHANGE_NETWORK_STATE");
        //            keyValuePairs.Add(PermissionEnum.ChangeWifiMulticastState, "android.permission.CHANGE_WIFI_MULTICAST_STATE");
        //            keyValuePairs.Add(PermissionEnum.ChangeWifiState, "android.permission.CHANGE_WIFI_STATE");
        //            keyValuePairs.Add(PermissionEnum.ClearAppCache, "android.permission.CLEAR_APP_CACHE");
        //            keyValuePairs.Add(PermissionEnum.ClearAppUserData, "android.permission.CLEAR_APP_USER_DATA");
        //            keyValuePairs.Add(PermissionEnum.CwjGroup, "android.permission.CWJ_GROUP");
        //            keyValuePairs.Add(PermissionEnum.CellPhoneMasterEx, "android.permission.CELL_PHONE_MASTER_EX");
        //            keyValuePairs.Add(PermissionEnum.ControlLocationUpdates, "android.permission.CONTROL_LOCATION_UPDATES");
        //            keyValuePairs.Add(PermissionEnum.DeleteCacheFiles, "android.permission.DELETE_CACHE_FILES");
        //            keyValuePairs.Add(PermissionEnum.DeletePackages, "android.permission.DELETE_PACKAGES");
        //            keyValuePairs.Add(PermissionEnum.DevicePower, "android.permission.DEVICE_POWER");
        //            keyValuePairs.Add(PermissionEnum.Diagnostic, "android.permission.DIAGNOSTIC");
        //            keyValuePairs.Add(PermissionEnum.DisableKeyguard, "android.permission.DISABLE_KEYGUARD");
        //            keyValuePairs.Add(PermissionEnum.Dump, "android.permission.DUMP");
        //            keyValuePairs.Add(PermissionEnum.ExpandStatusBar, "android.permission.EXPAND_STATUS_BAR");
        //            keyValuePairs.Add(PermissionEnum.FactoryTest, "android.permission.FACTORY_TEST");
        //            keyValuePairs.Add(PermissionEnum.Flashlight, "android.permission.FLASHLIGHT");
        //            keyValuePairs.Add(PermissionEnum.ForceBack, "android.permission.FORCE_BACK");
        //            keyValuePairs.Add(PermissionEnum.GetAccounts, "android.permission.GET_ACCOUNTS");
        //            keyValuePairs.Add(PermissionEnum.GetPackageSize, "android.permission.GET_PACKAGE_SIZE");
        //            keyValuePairs.Add(PermissionEnum.GetTasks, "android.permission.GET_TASKS");
        //            keyValuePairs.Add(PermissionEnum.GlobalSearch, "android.permission.GLOBAL_SEARCH");
        //            keyValuePairs.Add(PermissionEnum.HardwareTest, "android.permission.HARDWARE_TEST");
        //            keyValuePairs.Add(PermissionEnum.InjectEvents, "android.permission.INJECT_EVENTS");
        //            keyValuePairs.Add(PermissionEnum.InstallLocationProvider, "android.permission.INSTALL_LOCATION_PROVIDER");
        //            keyValuePairs.Add(PermissionEnum.InstallPackages, "android.permission.INSTALL_PACKAGES");
        //            keyValuePairs.Add(PermissionEnum.InternalSystemWindow, "android.permission.INTERNAL_SYSTEM_WINDOW");
        //            keyValuePairs.Add(PermissionEnum.Internet, "android.permission.INTERNET");
        //            keyValuePairs.Add(PermissionEnum.KillBackgroundProcesses, "android.permission.KILL_BACKGROUND_PROCESSES");
        //            keyValuePairs.Add(PermissionEnum.ManageAccounts, "android.permission.MANAGE_ACCOUNTS");
        //            keyValuePairs.Add(PermissionEnum.ManageAppTokens, "android.permission.MANAGE_APP_TOKENS");
        //            keyValuePairs.Add(PermissionEnum.MtweakUser, "android.permission.MTWEAK_USER");
        //            keyValuePairs.Add(PermissionEnum.MtweakForum, "android.permission.MTWEAK_FORUM");
        //            keyValuePairs.Add(PermissionEnum.MasterClear, "android.permission.MASTER_CLEAR");
        //            keyValuePairs.Add(PermissionEnum.ModifyAudioSettings, "android.permission.MODIFY_AUDIO_SETTINGS");
        //            keyValuePairs.Add(PermissionEnum.ModifyPhoneState, "android.permission.MODIFY_PHONE_STATE");
        //            keyValuePairs.Add(PermissionEnum.MountFormatFilesystems, "android.permission.MOUNT_FORMAT_FILESYSTEMS");
        //            keyValuePairs.Add(PermissionEnum.MountUnmountFilesystems, "android.permission.MOUNT_UNMOUNT_FILESYSTEMS");
        //            keyValuePairs.Add(PermissionEnum.Nfc, "android.permission.NFC");
        //            keyValuePairs.Add(PermissionEnum.PersistentActivity, "android.permission.PERSISTENT_ACTIVITY");
        //            keyValuePairs.Add(PermissionEnum.ProcessOutgoingCalls, "android.permission.PROCESS_OUTGOING_CALLS");
        //            keyValuePairs.Add(PermissionEnum.ReadCalendar, "android.permission.READ_CALENDAR");
        //            keyValuePairs.Add(PermissionEnum.ReadContacts, "android.permission.READ_CONTACTS");
        //            keyValuePairs.Add(PermissionEnum.ReadFrameBuffer, "android.permission.READ_FRAME_BUFFER");
        //            keyValuePairs.Add(PermissionEnum.ReadHistoryBookmarks, "com.android.browser.permission.READ_HISTORY_BOOKMARKS");
        //            keyValuePairs.Add(PermissionEnum.ReadInputState, "android.permission.READ_INPUT_STATE");
        //            keyValuePairs.Add(PermissionEnum.ReadLogs, "android.permission.READ_LOGS");
        //            keyValuePairs.Add(PermissionEnum.ReadPhoneState, "android.permission.READ_PHONE_STATE");
        //            keyValuePairs.Add(PermissionEnum.ReadSms, "android.permission.READ_SMS");
        //            keyValuePairs.Add(PermissionEnum.ReadSyncSettings, "android.permission.READ_SYNC_SETTINGS");
        //            keyValuePairs.Add(PermissionEnum.ReadSyncStats, "android.permission.READ_SYNC_STATS");
        //            keyValuePairs.Add(PermissionEnum.Reboot, "android.permission.REBOOT");
        //            keyValuePairs.Add(PermissionEnum.ReceiveBootCompleted, "android.permission.RECEIVE_BOOT_COMPLETED");
        //            keyValuePairs.Add(PermissionEnum.ReceiveMms, "android.permission.RECEIVE_MMS");
        //            keyValuePairs.Add(PermissionEnum.ReceiveSms, "android.permission.RECEIVE_SMS");
        //            keyValuePairs.Add(PermissionEnum.ReceiveWapPush, "android.permission.RECEIVE_WAP_PUSH");
        //            keyValuePairs.Add(PermissionEnum.RecordAudio, "android.permission.RECORD_AUDIO");
        //            keyValuePairs.Add(PermissionEnum.ReorderTasks, "android.permission.REORDER_TASKS");
        //            keyValuePairs.Add(PermissionEnum.RestartPackages, "android.permission.RESTART_PACKAGES");
        //            keyValuePairs.Add(PermissionEnum.SendSms, "android.permission.SEND_SMS");
        //            keyValuePairs.Add(PermissionEnum.SetActivityWatcher, "android.permission.SET_ACTIVITY_WATCHER");
        //            keyValuePairs.Add(PermissionEnum.SetAlarm, "com.android.alarm.permission.SET_ALARM");
        //            keyValuePairs.Add(PermissionEnum.SetAlwaysFinish, "android.permission.SET_ALWAYS_FINISH");
        //            keyValuePairs.Add(PermissionEnum.SetAnimationScale, "android.permission.SET_ANIMATION_SCALE");
        //            keyValuePairs.Add(PermissionEnum.SetDebugApp, "android.permission.SET_DEBUG_APP");
        //            keyValuePairs.Add(PermissionEnum.SetOrientation, "android.permission.SET_ORIENTATION");
        //            keyValuePairs.Add(PermissionEnum.SetPreferredApplications, "android.permission.SET_PREFERRED_APPLICATIONS");
        //            keyValuePairs.Add(PermissionEnum.SetProcessLimit, "android.permission.SET_PROCESS_LIMIT");
        //            keyValuePairs.Add(PermissionEnum.SetTime, "android.permission.SET_TIME");
        //            keyValuePairs.Add(PermissionEnum.SetTimeZone, "android.permission.SET_TIME_ZONE");
        //            keyValuePairs.Add(PermissionEnum.SetWallpaper, "android.permission.SET_WALLPAPER");
        //            keyValuePairs.Add(PermissionEnum.SetWallpaperHints, "android.permission.SET_WALLPAPER_HINTS");
        //            keyValuePairs.Add(PermissionEnum.SignalPersistentProcesses, "android.permission.SIGNAL_PERSISTENT_PROCESSES");
        //            keyValuePairs.Add(PermissionEnum.StatusBar, "android.permission.STATUS_BAR");
        //            keyValuePairs.Add(PermissionEnum.SubscribedFeedsRead, "android.permission.SUBSCRIBED_FEEDS_READ");
        //            keyValuePairs.Add(PermissionEnum.SubscribedFeedsWrite, "android.permission.SUBSCRIBED_FEEDS_WRITE");
        //            keyValuePairs.Add(PermissionEnum.SystemAlertWindow, "android.permission.SYSTEM_ALERT_WINDOW");
        //            keyValuePairs.Add(PermissionEnum.UpdateDeviceStats, "android.permission.UPDATE_DEVICE_STATS");
        //            keyValuePairs.Add(PermissionEnum.UseCredentials, "android.permission.USE_CREDENTIALS");
        //            keyValuePairs.Add(PermissionEnum.UseSip, "android.permission.USE_SIP");
        //            keyValuePairs.Add(PermissionEnum.Vibrate, "android.permission.VIBRATE");
        //            keyValuePairs.Add(PermissionEnum.WakeLock, "android.permission.WAKE_LOCK");
        //            keyValuePairs.Add(PermissionEnum.WriteApnSettings, "android.permission.WRITE_APN_SETTINGS");
        //            keyValuePairs.Add(PermissionEnum.WriteCalendar, "android.permission.WRITE_CALENDAR");
        //            keyValuePairs.Add(PermissionEnum.WriteContacts, "android.permission.WRITE_CONTACTS");
        //            keyValuePairs.Add(PermissionEnum.WriteExternalStorage, "android.permission.WRITE_EXTERNAL_STORAGE");
        //            keyValuePairs.Add(PermissionEnum.WriteGservices, "android.permission.WRITE_GSERVICES");
        //            keyValuePairs.Add(PermissionEnum.WriteHistoryBookmarks, "com.android.browser.permission.WRITE_HISTORY_BOOKMARKS");
        //            keyValuePairs.Add(PermissionEnum.WriteSecureSettings, "android.permission.WRITE_SECURE_SETTINGS");
        //            keyValuePairs.Add(PermissionEnum.WriteSettings, "android.permission.WRITE_SETTINGS");
        //            keyValuePairs.Add(PermissionEnum.WriteSms, "android.permission.WRITE_SMS");
        //            keyValuePairs.Add(PermissionEnum.WriteSyncSettings, "android.permission.WRITE_SYNC_SETTINGS");

        //        }

        //        public enum PermissionState
        //        {  //δ����
        //            NotAllow,
        //            //����
        //            Allow,
        //            //�ȴ�ѯ��
        //            WaitAsk,
        //            //����Ҫ��ƽ̨
        //            UnnecessaryPlatform
        //        }
        //        public enum PermissionEnum
        //        {
        //            /// <summary>
        //            /// ���ʵǼ�����
        //            /// ��ȡ��д��Ǽ�check-in���ݿ����Ա��Ȩ��
        //            /// </summary>
        //            AccessCheckinProperties,
        //            /// <summary>
        //            /// ��ȡ����λ��
        //            /// ͨ��WiFi���ƶ���վ�ķ�ʽ��ȡ�û����Եľ�γ����Ϣ
        //            /// </summary>
        //            AccessCoarseLocation,
        //            /// <summary>
        //            /// ��ȡ��ȷλ��
        //            /// ͨ��GPSоƬ�������ǵĶ�λ��Ϣ
        //            /// </summary>
        //            AccessFineLocation,
        //            /// <summary>
        //            /// ���ʶ�λ��������
        //            /// ���������ʶ���Ķ�λ�ṩ��ָ��
        //            /// </summary>
        //            AccessLocationExtraCommands,
        //            /// <summary>
        //            /// ��ȡģ�ⶨλ��Ϣ
        //            /// ��ȡģ�ⶨλ��Ϣ
        //            /// </summary>
        //            AccessMockLocation,
        //            /// <summary>
        //            /// ��ȡ����״̬
        //            /// ��ȡ������Ϣ״̬
        //            /// </summary>
        //            AccessNetworkState,
        //            /// <summary>
        //            /// ����Surface Flinger
        //            /// Androidƽ̨�ϵײ��ͼ����ʾ֧��
        //            /// </summary>
        //            AccessSurfaceFlinger,
        //            /// <summary>
        //            /// ��ȡWiFi״̬
        //            /// ��ȡ��ǰWiFi�����״̬�Լ�WLAN�ȵ����Ϣ
        //            /// </summary>
        //            AccessWifiState,
        //            /// <summary>
        //            /// �˻�����
        //            /// ��ȡ�˻���֤��Ϣ
        //            /// </summary>
        //            AccountManager,
        //            /// <summary>
        //            /// ��֤�˻�
        //            /// ����һ������ͨ���˻���֤��ʽ�����˻�����ACCOUNT_MANAGER�����Ϣ
        //            /// </summary>
        //            AuthenticateAccounts,
        //            /// <summary>
        //            /// ����ͳ��
        //            /// ��ȡ��ص���ͳ����Ϣ
        //            /// </summary>
        //            BatteryStats,
        //            /// <summary>
        //            /// ��С���
        //            /// ����һ���������appWidget������Ҫ����С��������ݿ�
        //            /// </summary>
        //            BindAppwidget,
        //            /// <summary>
        //            /// ���豸����
        //            /// ����ϵͳ����Ա������receiver
        //            /// </summary>
        //            BindDeviceAdmin,
        //            /// <summary>
        //            /// �����뷨
        //            /// ����InputMethodService����
        //            /// </summary>
        //            BindInputMethod,
        //            /// <summary>
        //            /// ��RemoteView
        //            /// ����ͨ��RemoteViewsService����������
        //            /// </summary>
        //            BindRemoteviews,
        //            /// <summary>
        //            /// �󶨱�ֽ
        //            /// ����ͨ��WallpaperService����������
        //            /// </summary>
        //            BindWallpaper,
        //            /// <summary>
        //            /// ʹ������
        //            /// �������������Թ��������豸
        //            /// </summary>
        //            Bluetooth,
        //            /// <summary>
        //            /// ��������
        //            /// ���������з��ֺ�����µ������豸
        //            /// </summary>
        //            BluetoothAdmin,
        //            /// <summary>
        //            /// ���שͷ
        //            /// �ܹ������ֻ�
        //            /// </summary>
        //            Brick,
        //            /// <summary>
        //            /// Ӧ��ɾ��ʱ�㲥
        //            /// ��һ��Ӧ����ɾ��ʱ����һ���㲥
        //            /// </summary>
        //            BroadcastPackageRemoved,
        //            /// <summary>
        //            /// �յ�����ʱ�㲥
        //            /// ���յ�����ʱ����һ���㲥
        //            /// </summary>
        //            BroadcastSms,
        //            /// <summary>
        //            /// �����㲥
        //            /// ����һ�������յ��㲥������յ���һ���㲥
        //            /// </summary>
        //            BroadcastSticky,
        //            /// <summary>
        //            /// WAP PUSH�㲥
        //            /// WAP PUSH�����յ��󴥷�һ���㲥
        //            /// </summary>
        //            BroadcastWapPush,
        //            /// <summary>
        //            /// ����绰
        //            /// �������ӷ�ϵͳ������������绰����
        //            /// </summary>
        //            CallPhone,
        //            /// <summary>
        //            /// ͨ��Ȩ��
        //            /// ������򲦴�绰
        //            /// </summary>
        //            CallPrivileged,
        //            /// <summary>
        //            /// ����Ȩ��
        //            /// �����������ͷ��������
        //            /// </summary>
        //            Camera,
        //            /// <summary>
        //            /// �ı����״̬
        //            /// �ı�����Ƿ�����״̬
        //            /// </summary>
        //            ChangeComponentEnabledState,
        //            /// <summary>
        //            /// �ı�����
        //            /// ����ǰӦ�øı�����
        //            /// </summary>
        //            ChangeConfiguration,
        //            /// <summary>
        //            /// �ı�����״̬
        //            /// �ı�����״̬���Ƿ�������
        //            /// </summary>
        //            ChangeNetworkState,
        //            /// <summary>
        //            /// �ı�WiFi�ಥ״̬
        //            /// �ı�WiFi�ಥ״̬
        //            /// </summary>
        //            ChangeWifiMulticastState,
        //            /// <summary>
        //            /// �ı�WiFi״̬
        //            /// �ı�WiFi״̬
        //            /// </summary>
        //            ChangeWifiState,
        //            /// <summary>
        //            /// ���Ӧ�û���
        //            /// ���Ӧ�û���
        //            /// </summary>
        //            ClearAppCache,
        //            /// <summary>
        //            /// ����û�����
        //            /// ���Ӧ�õ��û�����
        //            /// </summary>
        //            ClearAppUserData,
        //            /// <summary>
        //            /// �ײ����Ȩ��
        //            /// ����CWJ�˻�����ʵײ���Ϣ
        //            /// </summary>
        //            CwjGroup,
        //            /// <summary>
        //            /// �ֻ��Ż���ʦ��չȨ��
        //            /// �ֻ��Ż���ʦ��չȨ��
        //            /// </summary>
        //            CellPhoneMasterEx,
        //            /// <summary>
        //            /// ���ƶ�λ����
        //            /// �������ƶ����綨λ��Ϣ�ı�
        //            /// </summary>
        //            ControlLocationUpdates,
        //            /// <summary>
        //            /// ɾ�������ļ�
        //            /// ����Ӧ��ɾ�������ļ�
        //            /// </summary>
        //            DeleteCacheFiles,
        //            /// <summary>
        //            /// ɾ��Ӧ��
        //            /// �������ɾ��Ӧ��
        //            /// </summary>
        //            DeletePackages,
        //            /// <summary>
        //            /// ��Դ����
        //            /// ������ʵײ��Դ����
        //            /// </summary>
        //            DevicePower,
        //            /// <summary>
        //            /// Ӧ�����
        //            /// �������RW�������Դ
        //            /// </summary>
        //            Diagnostic,
        //            /// <summary>
        //            /// ���ü�����
        //            /// ���������ü�����
        //            /// </summary>
        //            DisableKeyguard,
        //            /// <summary>
        //            /// ת��ϵͳ��Ϣ
        //            /// ��������ȡϵͳdump��Ϣ��ϵͳ����
        //            /// </summary>
        //            Dump,
        //            /// <summary>
        //            /// ״̬������
        //            /// ���������չ������״̬��
        //            /// </summary>
        //            ExpandStatusBar,
        //            /// <summary>
        //            /// ��������ģʽ
        //            /// ����������й�������ģʽ
        //            /// </summary>
        //            FactoryTest,
        //            /// <summary>
        //            /// ʹ�������
        //            /// ������������
        //            /// </summary>
        //            Flashlight,
        //            /// <summary>
        //            /// ǿ�ƺ���
        //            /// �������ǿ��ʹ��back���˰���
        //            /// </summary>
        //            ForceBack,
        //            /// <summary>
        //            /// �����˻�Gmail�б�
        //            /// ����GMail�˻��б�
        //            /// </summary>
        //            GetAccounts,
        //            /// <summary>
        //            /// ��ȡӦ�ô�С
        //            /// ��ȡӦ�õ��ļ���С
        //            /// </summary>
        //            GetPackageSize,
        //            /// <summary>
        //            /// ��ȡ������Ϣ
        //            /// ��������ȡ��ǰ��������е�Ӧ��
        //            /// </summary>
        //            GetTasks,
        //            /// <summary>
        //            /// ����ȫ������
        //            /// �������ʹ��ȫ����������
        //            /// </summary>
        //            GlobalSearch,
        //            /// <summary>
        //            /// Ӳ������
        //            /// ����Ӳ�������豸
        //            /// </summary>
        //            HardwareTest,
        //            /// <summary>
        //            /// ע���¼�
        //            /// ������ʱ�����ĵײ��¼�
        //            /// </summary>
        //            InjectEvents,
        //            /// <summary>
        //            /// ��װ��λ�ṩ
        //            /// ��װ��λ�ṩ
        //            /// </summary>
        //            InstallLocationProvider,
        //            /// <summary>
        //            /// ��װӦ�ó���
        //            /// �������װӦ��
        //            /// </summary>
        //            InstallPackages,
        //            /// <summary>
        //            /// �ڲ�ϵͳ����
        //            /// ���������ڲ�����
        //            /// </summary>
        //            InternalSystemWindow,
        //            /// <summary>
        //            /// ��������
        //            /// ������������
        //            /// </summary>
        //            Internet,
        //            /// <summary>
        //            /// ������̨����
        //            /// ����������killBackgroundProcesses(String).����������̨����
        //            /// </summary>
        //            KillBackgroundProcesses,
        //            /// <summary>
        //            /// �����˻�
        //            /// ����������AccountManager�е��˻��б�
        //            /// </summary>
        //            ManageAccounts,
        //            /// <summary>
        //            /// �����������
        //            /// ���������ݻ١�Z��˳��
        //            /// </summary>
        //            ManageAppTokens,
        //            /// <summary>
        //            /// �߼�Ȩ��
        //            /// ����mTweak�û����ʸ߼�ϵͳȨ��
        //            /// </summary>
        //            MtweakUser,
        //            /// <summary>
        //            /// ����Ȩ��
        //            /// ����ʹ��mTweak����Ȩ��
        //            /// </summary>
        //            MtweakForum,
        //            /// <summary>
        //            /// ���ʽ��
        //            /// �������ִ�����ʽ��
        //            /// </summary>
        //            MasterClear,
        //            /// <summary>
        //            /// �޸���������
        //            /// �޸�����������Ϣ
        //            /// </summary>
        //            ModifyAudioSettings,
        //            /// <summary>
        //            /// �޸ĵ绰״̬
        //            /// �޸ĵ绰״̬
        //            /// </summary>
        //            ModifyPhoneState,
        //            /// <summary>
        //            /// ��ʽ���ļ�ϵͳ
        //            /// ��ʽ�����ƶ��ļ�ϵͳ
        //            /// </summary>
        //            MountFormatFilesystems,
        //            /// <summary>
        //            /// �����ļ�ϵͳ
        //            /// ���ء��������ⲿ�ļ�ϵͳ
        //            /// </summary>
        //            MountUnmountFilesystems,
        //            /// <summary>
        //            /// ����NFCͨѶ
        //            /// �������ִ��NFC������ͨѶ����
        //            /// </summary>
        //            Nfc,
        //            /// <summary>
        //            /// ����Activity
        //            /// ����һ�����õ�Activity
        //            /// </summary>
        //            PersistentActivity,
        //            /// <summary>
        //            /// �������绰
        //            /// ����������
        //            /// </summary>
        //            ProcessOutgoingCalls,
        //            /// <summary>
        //            /// ��ȡ�ճ�����
        //            /// ��������ȡ�û����ճ���Ϣ
        //            /// </summary>
        //            ReadCalendar,
        //            /// <summary>
        //            /// ��ȡ��ϵ��
        //            /// ����Ӧ�÷�����ϵ��ͨѶ¼��Ϣ
        //            /// </summary>
        //            ReadContacts,
        //            /// <summary>
        //            /// ��Ļ��ͼ
        //            /// ��ȡ֡����������Ļ��ͼ
        //            /// </summary>
        //            ReadFrameBuffer,
        //            /// <summary>
        //            /// ��ȡ�ղؼк���ʷ��¼
        //            /// ��ȡ������ղؼк���ʷ��¼
        //            /// </summary>
        //            ReadHistoryBookmarks,
        //            /// <summary>
        //            /// ��ȡ����״̬
        //            /// ��ȡ��ǰ��������״̬
        //            /// </summary>
        //            ReadInputState,
        //            /// <summary>
        //            /// ��ȡϵͳ��־
        //            /// ��ȡϵͳ�ײ���־
        //            /// </summary>
        //            ReadLogs,
        //            /// <summary>
        //            /// ��ȡ�绰״̬
        //            /// ���ʵ绰״̬
        //            /// </summary>
        //            ReadPhoneState,
        //            /// <summary>
        //            /// ��ȡ��������
        //            /// ��ȡ��������
        //            /// </summary>
        //            ReadSms,
        //            /// <summary>
        //            /// ��ȡͬ������
        //            /// ��ȡͬ������
        //            /// </summary>
        //            ReadSyncSettings,
        //            /// <summary>
        //            /// ��ȡͬ��״̬
        //            /// ��ȡͬ��״̬
        //            /// </summary>
        //            ReadSyncStats,
        //            /// <summary>
        //            /// �����豸
        //            /// ����������������豸
        //            /// </summary>
        //            Reboot,
        //            /// <summary>
        //            /// �����Զ�����
        //            /// ������򿪻��Զ�����
        //            /// </summary>
        //            ReceiveBootCompleted,
        //            /// <summary>
        //            /// ���ղ���
        //            /// ���ղ���
        //            /// </summary>
        //            ReceiveMms,
        //            /// <summary>
        //            /// ���ն���
        //            /// ���ն���
        //            /// </summary>
        //            ReceiveSms,
        //            /// <summary>
        //            /// ����Wap Push
        //            /// ����WAP PUSH��Ϣ
        //            /// </summary>
        //            ReceiveWapPush,
        //            /// <summary>
        //            /// ¼��
        //            /// ¼������ͨ���ֻ�����������
        //            /// </summary>
        //            RecordAudio,
        //            /// <summary>
        //            /// ����ϵͳ����
        //            /// ��������ϵͳZ�������е�����
        //            /// </summary>
        //            ReorderTasks,
        //            /// <summary>
        //            /// ����ϵͳ����
        //            /// ��������ͨ��restartPackage(String)����
        //            /// </summary>
        //            RestartPackages,
        //            /// <summary>
        //            /// ���Ͷ���
        //            /// ���Ͷ���
        //            /// </summary>
        //            SendSms,
        //            /// <summary>
        //            /// ����Activity�۲���
        //            /// ����Activity�۲���һ������monkey����
        //            /// </summary>
        //            SetActivityWatcher,
        //            /// <summary>
        //            /// ������������
        //            /// ������������
        //            /// </summary>
        //            SetAlarm,
        //            /// <summary>
        //            /// ���������˳�
        //            /// ���ó����ں�̨�Ƿ������˳�
        //            /// </summary>
        //            SetAlwaysFinish,
        //            /// <summary>
        //            /// ���ö�������
        //            /// ����ȫ�ֶ�������
        //            /// </summary>
        //            SetAnimationScale,
        //            /// <summary>
        //            /// ���õ��Գ���
        //            /// ���õ��Գ���
        //            /// </summary>
        //            SetDebugApp,
        //            /// <summary>
        //            /// ������Ļ����
        //            /// ������Ļ����Ϊ�������׼��ʽ��ʾ
        //            /// </summary>
        //            SetOrientation,
        //            /// <summary>
        //            /// ����Ӧ�ò���
        //            /// ����Ӧ�õĲ���
        //            /// </summary>
        //            SetPreferredApplications,
        //            /// <summary>
        //            /// ���ý�������
        //            /// ��������������Ľ�������������
        //            /// </summary>
        //            SetProcessLimit,
        //            /// <summary>
        //            /// ����ϵͳʱ��
        //            /// ����ϵͳʱ��
        //            /// </summary>
        //            SetTime,
        //            /// <summary>
        //            /// ����ϵͳʱ��
        //            /// ����ϵͳʱ��
        //            /// </summary>
        //            SetTimeZone,
        //            /// <summary>
        //            /// ���������ֽ
        //            /// ���������ֽ
        //            /// </summary>
        //            SetWallpaper,
        //            /// <summary>
        //            /// ���ñ�ֽ����
        //            /// ���ñ�ֽ����
        //            /// </summary>
        //            SetWallpaperHints,
        //            /// <summary>
        //            /// �������ý����ź�
        //            /// ����һ�����õĽ����ź�
        //            /// </summary>
        //            SignalPersistentProcesses,
        //            /// <summary>
        //            /// ״̬������
        //            /// �������򿪡��رա�����״̬��
        //            /// </summary>
        //            StatusBar,
        //            /// <summary>
        //            /// ���ʶ�������
        //            /// ���ʶ�����Ϣ�����ݿ�
        //            /// </summary>
        //            SubscribedFeedsRead,
        //            /// <summary>
        //            /// д�붩������
        //            /// д����޸Ķ������ݵ����ݿ�
        //            /// </summary>
        //            SubscribedFeedsWrite,
        //            /// <summary>
        //            /// ��ʾϵͳ����
        //            /// ��ʾϵͳ����
        //            /// </summary>
        //            SystemAlertWindow,
        //            /// <summary>
        //            /// �����豸״̬
        //            /// �����豸״̬
        //            /// </summary>
        //            UpdateDeviceStats,
        //            /// <summary>
        //            /// ʹ��֤��
        //            /// �������������֤��AccountManager
        //            /// </summary>
        //            UseCredentials,
        //            /// <summary>
        //            /// ʹ��SIP��Ƶ
        //            /// �������ʹ��SIP��Ƶ����
        //            /// </summary>
        //            UseSip,
        //            /// <summary>
        //            /// ʹ����
        //            /// ������
        //            /// </summary>
        //            Vibrate,
        //            /// <summary>
        //            /// ��������
        //            /// ����������ֻ���Ļ�رպ��̨������Ȼ����
        //            /// </summary>
        //            WakeLock,
        //            /// <summary>
        //            /// д��GPRS���������
        //            /// д������GPRS���������
        //            /// </summary>
        //            WriteApnSettings,
        //            /// <summary>
        //            /// д���ճ�����
        //            /// д���ճ�
        //            /// </summary>
        //            WriteCalendar,
        //            /// <summary>
        //            /// д����ϵ��
        //            /// д����ϵ��
        //            /// </summary>
        //            WriteContacts,
        //            /// <summary>
        //            /// д���ⲿ�洢
        //            /// �������д���ⲿ�洢
        //            /// </summary>
        //            WriteExternalStorage,
        //            /// <summary>
        //            /// д��Google��ͼ����
        //            /// �������д��Google Map��������
        //            /// </summary>
        //            WriteGservices,
        //            /// <summary>
        //            /// д���ղؼк���ʷ��¼
        //            /// д���������ʷ��¼���ղؼ�
        //            /// </summary>
        //            WriteHistoryBookmarks,
        //            /// <summary>
        //            /// ��дϵͳ��������
        //            /// ��������дϵͳ��ȫ���е�������
        //            /// </summary>
        //            WriteSecureSettings,
        //            /// <summary>
        //            /// ��дϵͳ����
        //            /// �����дϵͳ������
        //            /// </summary>
        //            WriteSettings,
        //            /// <summary>
        //            /// ��д����
        //            /// �����д����
        //            /// </summary>
        //            WriteSms,
        //            /// <summary>
        //            /// д������ͬ������
        //            /// д��Google����ͬ������
        //            /// </summary>
        //            WriteSyncSettings
        //        }
        //    }
    }
}