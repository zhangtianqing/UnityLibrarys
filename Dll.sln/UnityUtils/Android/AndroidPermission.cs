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
        //                //方法1 通过Permission获取
        //                if (!Permission.HasUserAuthorizedPermission(keyValuePairs[permission]))
        //                {
        //                    //Debug.Log("请求权限：" + keyValuePairs[permission]);
        //                    Permission.RequestUserPermission(keyValuePairs[permission]);
        //                }
        //                else
        //                {
        //                    //Debug.Log($"权限已获取:{keyValuePairs[permission]}");
        //                }
        //            }
        //        }
        //        /// <summary>
        //        /// 转驼峰法
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


        //        private static void 提取生成字符串()
        //        {
        //            string str = "访问登记属性\tandroid.permission.ACCESS_CHECKIN_PROPERTIES ，读取或写入登记check-in数据库属性表的权限\r\n获取错略位置\tandroid.permission.ACCESS_COARSE_LOCATION，通过WiFi或移动基站的方式获取用户错略的经纬度信息，定位精度大概误差在30~1500米\r\n获取精确位置\tandroid.permission.ACCESS_FINE_LOCATION，通过GPS芯片接收卫星的定位信息，定位精度达10米以内\r\n访问定位额外命令\tandroid.permission.ACCESS_LOCATION_EXTRA_COMMANDS，允许程序访问额外的定位提供者指令\r\n获取模拟定位信息\tandroid.permission.ACCESS_MOCK_LOCATION，获取模拟定位信息，一般用于帮助开发者调试应用\r\n获取网络状态\tandroid.permission.ACCESS_NETWORK_STATE，获取网络信息状态，如当前的网络连接是否有效\r\n访问Surface Flinger\tandroid.permission.ACCESS_SURFACE_FLINGER，Android平台上底层的图形显示支持，一般用于游戏或照相机预览界面和底层模式的屏幕截图\r\n获取WiFi状态\tandroid.permission.ACCESS_WIFI_STATE，获取当前WiFi接入的状态以及WLAN热点的信息\r\n账户管理\tandroid.permission.ACCOUNT_MANAGER，获取账户验证信息，主要为GMail账户信息，只有系统级进程才能访问的权限\r\n验证账户\tandroid.permission.AUTHENTICATE_ACCOUNTS，允许一个程序通过账户验证方式访问账户管理ACCOUNT_MANAGER相关信息\r\n电量统计\tandroid.permission.BATTERY_STATS，获取电池电量统计信息\r\n绑定小插件\tandroid.permission.BIND_APPWIDGET，允许一个程序告诉appWidget服务需要访问小插件的数据库，只有非常少的应用才用到此权限\r\n绑定设备管理\tandroid.permission.BIND_DEVICE_ADMIN，请求系统管理员接收者receiver，只有系统才能使用\r\n绑定输入法\tandroid.permission.BIND_INPUT_METHOD ，请求InputMethodService服务，只有系统才能使用\r\n绑定RemoteView\tandroid.permission.BIND_REMOTEVIEWS，必须通过RemoteViewsService服务来请求，只有系统才能用\r\n绑定壁纸\tandroid.permission.BIND_WALLPAPER，必须通过WallpaperService服务来请求，只有系统才能用\r\n使用蓝牙\tandroid.permission.BLUETOOTH，允许程序连接配对过的蓝牙设备\r\n蓝牙管理\tandroid.permission.BLUETOOTH_ADMIN，允许程序进行发现和配对新的蓝牙设备\r\n变成砖头\tandroid.permission.BRICK，能够禁用手机，非常危险，顾名思义就是让手机变成砖头\r\n应用删除时广播\tandroid.permission.BROADCAST_PACKAGE_REMOVED，当一个应用在删除时触发一个广播\r\n收到短信时广播\tandroid.permission.BROADCAST_SMS，当收到短信时触发一个广播\r\n连续广播\tandroid.permission.BROADCAST_STICKY，允许一个程序收到广播后快速收到下一个广播\r\nWAP PUSH广播\tandroid.permission.BROADCAST_WAP_PUSH，WAP PUSH服务收到后触发一个广播\r\n拨打电话\tandroid.permission.CALL_PHONE，允许程序从非系统拨号器里输入电话号码\r\n通话权限\tandroid.permission.CALL_PRIVILEGED，允许程序拨打电话，替换系统的拨号器界面\r\n拍照权限\tandroid.permission.CAMERA，允许访问摄像头进行拍照\r\n改变组件状态\tandroid.permission.CHANGE_COMPONENT_ENABLED_STATE，改变组件是否启用状态\r\n改变配置\tandroid.permission.CHANGE_CONFIGURATION，允许当前应用改变配置，如定位\r\n改变网络状态\tandroid.permission.CHANGE_NETWORK_STATE，改变网络状态如是否能联网\r\n改变WiFi多播状态\tandroid.permission.CHANGE_WIFI_MULTICAST_STATE，改变WiFi多播状态\r\n改变WiFi状态\tandroid.permission.CHANGE_WIFI_STATE，改变WiFi状态\r\n清除应用缓存\tandroid.permission.CLEAR_APP_CACHE，清除应用缓存\r\n清除用户数据\tandroid.permission.CLEAR_APP_USER_DATA，清除应用的用户数据\r\n底层访问权限\tandroid.permission.CWJ_GROUP，允许CWJ账户组访问底层信息\r\n手机优化大师扩展权限\tandroid.permission.CELL_PHONE_MASTER_EX，手机优化大师扩展权限\r\n控制定位更新\tandroid.permission.CONTROL_LOCATION_UPDATES，允许获得移动网络定位信息改变\r\n删除缓存文件\tandroid.permission.DELETE_CACHE_FILES，允许应用删除缓存文件\r\n删除应用\tandroid.permission.DELETE_PACKAGES，允许程序删除应用\r\n电源管理\tandroid.permission.DEVICE_POWER，允许访问底层电源管理\r\n应用诊断\tandroid.permission.DIAGNOSTIC，允许程序到RW到诊断资源\r\n禁用键盘锁\tandroid.permission.DISABLE_KEYGUARD，允许程序禁用键盘锁\r\n转存系统信息\tandroid.permission.DUMP，允许程序获取系统dump信息从系统服务\r\n状态栏控制\tandroid.permission.EXPAND_STATUS_BAR，允许程序扩展或收缩状态栏\r\n工厂测试模式\tandroid.permission.FACTORY_TEST，允许程序运行工厂测试模式\r\n使用闪光灯\tandroid.permission.FLASHLIGHT，允许访问闪光灯\r\n强制后退\tandroid.permission.FORCE_BACK，允许程序强制使用back后退按键，无论Activity是否在顶层\r\n访问账户Gmail列表\tandroid.permission.GET_ACCOUNTS，访问GMail账户列表\r\n获取应用大小\tandroid.permission.GET_PACKAGE_SIZE，获取应用的文件大小\r\n获取任务信息\tandroid.permission.GET_TASKS，允许程序获取当前或最近运行的应用\r\n允许全局搜索\tandroid.permission.GLOBAL_SEARCH，允许程序使用全局搜索功能\r\n硬件测试\tandroid.permission.HARDWARE_TEST，访问硬件辅助设备，用于硬件测试\r\n注射事件\tandroid.permission.INJECT_EVENTS，允许访问本程序的底层事件，获取按键、轨迹球的事件流\r\n安装定位提供\tandroid.permission.INSTALL_LOCATION_PROVIDER，安装定位提供\r\n安装应用程序\tandroid.permission.INSTALL_PACKAGES，允许程序安装应用\r\n内部系统窗口\tandroid.permission.INTERNAL_SYSTEM_WINDOW，允许程序打开内部窗口，不对第三方应用程序开放此权限\r\n访问网络\tandroid.permission.INTERNET，访问网络连接，可能产生GPRS流量\r\n结束后台进程\tandroid.permission.KILL_BACKGROUND_PROCESSES，允许程序调用killBackgroundProcesses(String).方法结束后台进程\r\n管理账户\tandroid.permission.MANAGE_ACCOUNTS，允许程序管理AccountManager中的账户列表\r\n管理程序引用\tandroid.permission.MANAGE_APP_TOKENS，管理创建、摧毁、Z轴顺序，仅用于系统\r\n高级权限\tandroid.permission.MTWEAK_USER，允许mTweak用户访问高级系统权限\r\n社区权限\tandroid.permission.MTWEAK_FORUM，允许使用mTweak社区权限\r\n软格式化\tandroid.permission.MASTER_CLEAR，允许程序执行软格式化，删除系统配置信息\r\n修改声音设置\tandroid.permission.MODIFY_AUDIO_SETTINGS，修改声音设置信息\r\n修改电话状态\tandroid.permission.MODIFY_PHONE_STATE，修改电话状态，如飞行模式，但不包含替换系统拨号器界面\r\n格式化文件系统\tandroid.permission.MOUNT_FORMAT_FILESYSTEMS，格式化可移动文件系统，比如格式化清空SD卡\r\n挂载文件系统\tandroid.permission.MOUNT_UNMOUNT_FILESYSTEMS，挂载、反挂载外部文件系统\r\n允许NFC通讯\tandroid.permission.NFC，允许程序执行NFC近距离通讯操作，用于移动支持\r\n永久Activity\tandroid.permission.PERSISTENT_ACTIVITY，创建一个永久的Activity，该功能标记为将来将被移除\r\n处理拨出电话\tandroid.permission.PROCESS_OUTGOING_CALLS，允许程序监视，修改或放弃播出电话\r\n读取日程提醒\tandroid.permission.READ_CALENDAR，允许程序读取用户的日程信息\r\n读取联系人\tandroid.permission.READ_CONTACTS，允许应用访问联系人通讯录信息\r\n屏幕截图\tandroid.permission.READ_FRAME_BUFFER，读取帧缓存用于屏幕截图\r\n读取收藏夹和历史记录\tcom.android.browser.permission.READ_HISTORY_BOOKMARKS，读取浏览器收藏夹和历史记录\r\n读取输入状态\tandroid.permission.READ_INPUT_STATE，读取当前键的输入状态，仅用于系统\r\n读取系统日志\tandroid.permission.READ_LOGS，读取系统底层日志\r\n读取电话状态\tandroid.permission.READ_PHONE_STATE，访问电话状态\r\n读取短信内容\tandroid.permission.READ_SMS，读取短信内容\r\n读取同步设置\tandroid.permission.READ_SYNC_SETTINGS，读取同步设置，读取Google在线同步设置\r\n读取同步状态\tandroid.permission.READ_SYNC_STATS，读取同步状态，获得Google在线同步状态\r\n重启设备\tandroid.permission.REBOOT，允许程序重新启动设备\r\n开机自动允许\tandroid.permission.RECEIVE_BOOT_COMPLETED，允许程序开机自动运行\r\n接收彩信\tandroid.permission.RECEIVE_MMS，接收彩信\r\n接收短信\tandroid.permission.RECEIVE_SMS，接收短信\r\n接收Wap Push\tandroid.permission.RECEIVE_WAP_PUSH，接收WAP PUSH信息\r\n录音\tandroid.permission.RECORD_AUDIO，录制声音通过手机或耳机的麦克\r\n排序系统任务\tandroid.permission.REORDER_TASKS，重新排序系统Z轴运行中的任务\r\n结束系统任务\tandroid.permission.RESTART_PACKAGES，结束任务通过restartPackage(String)方法，该方式将在外来放弃\r\n发送短信\tandroid.permission.SEND_SMS，发送短信\r\n设置Activity观察其\tandroid.permission.SET_ACTIVITY_WATCHER，设置Activity观察器一般用于monkey测试\r\n设置闹铃提醒\tcom.android.alarm.permission.SET_ALARM，设置闹铃提醒\r\n设置总是退出\tandroid.permission.SET_ALWAYS_FINISH，设置程序在后台是否总是退出\r\n设置动画缩放\tandroid.permission.SET_ANIMATION_SCALE，设置全局动画缩放\r\n设置调试程序\tandroid.permission.SET_DEBUG_APP，设置调试程序，一般用于开发\r\n设置屏幕方向\tandroid.permission.SET_ORIENTATION，设置屏幕方向为横屏或标准方式显示，不用于普通应用\r\n设置应用参数\tandroid.permission.SET_PREFERRED_APPLICATIONS，设置应用的参数，已不再工作具体查看addPackageToPreferred(String) 介绍\r\n设置进程限制\tandroid.permission.SET_PROCESS_LIMIT，允许程序设置最大的进程数量的限制\r\n设置系统时间\tandroid.permission.SET_TIME，设置系统时间\r\n设置系统时区\tandroid.permission.SET_TIME_ZONE，设置系统时区\r\n设置桌面壁纸\tandroid.permission.SET_WALLPAPER，设置桌面壁纸\r\n设置壁纸建议\tandroid.permission.SET_WALLPAPER_HINTS，设置壁纸建议\r\n发送永久进程信号\tandroid.permission.SIGNAL_PERSISTENT_PROCESSES，发送一个永久的进程信号\r\n状态栏控制\tandroid.permission.STATUS_BAR，允许程序打开、关闭、禁用状态栏\r\n访问订阅内容\tandroid.permission.SUBSCRIBED_FEEDS_READ，访问订阅信息的数据库\r\n写入订阅内容\tandroid.permission.SUBSCRIBED_FEEDS_WRITE，写入或修改订阅内容的数据库\r\n显示系统窗口\tandroid.permission.SYSTEM_ALERT_WINDOW，显示系统窗口\r\n更新设备状态\tandroid.permission.UPDATE_DEVICE_STATS，更新设备状态\r\n使用证书\tandroid.permission.USE_CREDENTIALS，允许程序请求验证从AccountManager\r\n使用SIP视频\tandroid.permission.USE_SIP，允许程序使用SIP视频服务\r\n使用振动\tandroid.permission.VIBRATE，允许振动\r\n唤醒锁定\tandroid.permission.WAKE_LOCK，允许程序在手机屏幕关闭后后台进程仍然运行\r\n写入GPRS接入点设置\tandroid.permission.WRITE_APN_SETTINGS，写入网络GPRS接入点设置\r\n写入日程提醒\tandroid.permission.WRITE_CALENDAR，写入日程，但不可读取\r\n写入联系人\tandroid.permission.WRITE_CONTACTS，写入联系人，但不可读取\r\n写入外部存储\tandroid.permission.WRITE_EXTERNAL_STORAGE，允许程序写入外部存储，如SD卡上写文件\r\n写入Google地图数据\tandroid.permission.WRITE_GSERVICES，允许程序写入Google Map服务数据\r\n写入收藏夹和历史记录\tcom.android.browser.permission.WRITE_HISTORY_BOOKMARKS，写入浏览器历史记录或收藏夹，但不可读取\r\n读写系统敏感设置\tandroid.permission.WRITE_SECURE_SETTINGS，允许程序读写系统安全敏感的设置项\r\n读写系统设置\tandroid.permission.WRITE_SETTINGS，允许读写系统设置项\r\n编写短信\tandroid.permission.WRITE_SMS，允许编写短信\r\n写入在线同步设置\tandroid.permission.WRITE_SYNC_SETTINGS，写入Google在线同步设置";
        //            string[] lines = str.Split('\n');
        //            string Enum = "";
        //            string initEnumStr = "";
        //            foreach (string line in lines)
        //            {
        //                string[] content = line.Split('\t');
        //                string[] stringK = content[1].Split('，');
        //                Debug.Log(stringK[0]);

        //                Enum += "/// <summary>" + "\n" +
        //                $"/// {content[0]}" + "\n" +
        //                $"/// {stringK[1]}" + "\n" +
        //                "/// </summary>" + "\n" +
        //               ToPascal(stringK[0].Split('.')[stringK[0].Split('.').Length - 1]) + ",\n";

        //                initEnumStr += " keyValuePairs.Add(PermissionEnum." + ToPascal(stringK[0].Split('.')[stringK[0].Split('.').Length - 1]) + $",\"{stringK[0]}\");\n";
        //                //Enum += ToPascal(stringK[0].Split(".")[stringK[0].Split(".").Length - 1]) + "\n"; //stringK[0].Split(".")[stringK[0].Split(".").Length - 1]
        //                //Debug.Log($"权限名称:{content[0]},权限字符串:{stringK[0]}，权限说明:{stringK[1]}");
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
        //        {  //未允许
        //            NotAllow,
        //            //允许
        //            Allow,
        //            //等待询问
        //            WaitAsk,
        //            //不需要的平台
        //            UnnecessaryPlatform
        //        }
        //        public enum PermissionEnum
        //        {
        //            /// <summary>
        //            /// 访问登记属性
        //            /// 读取或写入登记check-in数据库属性表的权限
        //            /// </summary>
        //            AccessCheckinProperties,
        //            /// <summary>
        //            /// 获取错略位置
        //            /// 通过WiFi或移动基站的方式获取用户错略的经纬度信息
        //            /// </summary>
        //            AccessCoarseLocation,
        //            /// <summary>
        //            /// 获取精确位置
        //            /// 通过GPS芯片接收卫星的定位信息
        //            /// </summary>
        //            AccessFineLocation,
        //            /// <summary>
        //            /// 访问定位额外命令
        //            /// 允许程序访问额外的定位提供者指令
        //            /// </summary>
        //            AccessLocationExtraCommands,
        //            /// <summary>
        //            /// 获取模拟定位信息
        //            /// 获取模拟定位信息
        //            /// </summary>
        //            AccessMockLocation,
        //            /// <summary>
        //            /// 获取网络状态
        //            /// 获取网络信息状态
        //            /// </summary>
        //            AccessNetworkState,
        //            /// <summary>
        //            /// 访问Surface Flinger
        //            /// Android平台上底层的图形显示支持
        //            /// </summary>
        //            AccessSurfaceFlinger,
        //            /// <summary>
        //            /// 获取WiFi状态
        //            /// 获取当前WiFi接入的状态以及WLAN热点的信息
        //            /// </summary>
        //            AccessWifiState,
        //            /// <summary>
        //            /// 账户管理
        //            /// 获取账户验证信息
        //            /// </summary>
        //            AccountManager,
        //            /// <summary>
        //            /// 验证账户
        //            /// 允许一个程序通过账户验证方式访问账户管理ACCOUNT_MANAGER相关信息
        //            /// </summary>
        //            AuthenticateAccounts,
        //            /// <summary>
        //            /// 电量统计
        //            /// 获取电池电量统计信息
        //            /// </summary>
        //            BatteryStats,
        //            /// <summary>
        //            /// 绑定小插件
        //            /// 允许一个程序告诉appWidget服务需要访问小插件的数据库
        //            /// </summary>
        //            BindAppwidget,
        //            /// <summary>
        //            /// 绑定设备管理
        //            /// 请求系统管理员接收者receiver
        //            /// </summary>
        //            BindDeviceAdmin,
        //            /// <summary>
        //            /// 绑定输入法
        //            /// 请求InputMethodService服务
        //            /// </summary>
        //            BindInputMethod,
        //            /// <summary>
        //            /// 绑定RemoteView
        //            /// 必须通过RemoteViewsService服务来请求
        //            /// </summary>
        //            BindRemoteviews,
        //            /// <summary>
        //            /// 绑定壁纸
        //            /// 必须通过WallpaperService服务来请求
        //            /// </summary>
        //            BindWallpaper,
        //            /// <summary>
        //            /// 使用蓝牙
        //            /// 允许程序连接配对过的蓝牙设备
        //            /// </summary>
        //            Bluetooth,
        //            /// <summary>
        //            /// 蓝牙管理
        //            /// 允许程序进行发现和配对新的蓝牙设备
        //            /// </summary>
        //            BluetoothAdmin,
        //            /// <summary>
        //            /// 变成砖头
        //            /// 能够禁用手机
        //            /// </summary>
        //            Brick,
        //            /// <summary>
        //            /// 应用删除时广播
        //            /// 当一个应用在删除时触发一个广播
        //            /// </summary>
        //            BroadcastPackageRemoved,
        //            /// <summary>
        //            /// 收到短信时广播
        //            /// 当收到短信时触发一个广播
        //            /// </summary>
        //            BroadcastSms,
        //            /// <summary>
        //            /// 连续广播
        //            /// 允许一个程序收到广播后快速收到下一个广播
        //            /// </summary>
        //            BroadcastSticky,
        //            /// <summary>
        //            /// WAP PUSH广播
        //            /// WAP PUSH服务收到后触发一个广播
        //            /// </summary>
        //            BroadcastWapPush,
        //            /// <summary>
        //            /// 拨打电话
        //            /// 允许程序从非系统拨号器里输入电话号码
        //            /// </summary>
        //            CallPhone,
        //            /// <summary>
        //            /// 通话权限
        //            /// 允许程序拨打电话
        //            /// </summary>
        //            CallPrivileged,
        //            /// <summary>
        //            /// 拍照权限
        //            /// 允许访问摄像头进行拍照
        //            /// </summary>
        //            Camera,
        //            /// <summary>
        //            /// 改变组件状态
        //            /// 改变组件是否启用状态
        //            /// </summary>
        //            ChangeComponentEnabledState,
        //            /// <summary>
        //            /// 改变配置
        //            /// 允许当前应用改变配置
        //            /// </summary>
        //            ChangeConfiguration,
        //            /// <summary>
        //            /// 改变网络状态
        //            /// 改变网络状态如是否能联网
        //            /// </summary>
        //            ChangeNetworkState,
        //            /// <summary>
        //            /// 改变WiFi多播状态
        //            /// 改变WiFi多播状态
        //            /// </summary>
        //            ChangeWifiMulticastState,
        //            /// <summary>
        //            /// 改变WiFi状态
        //            /// 改变WiFi状态
        //            /// </summary>
        //            ChangeWifiState,
        //            /// <summary>
        //            /// 清除应用缓存
        //            /// 清除应用缓存
        //            /// </summary>
        //            ClearAppCache,
        //            /// <summary>
        //            /// 清除用户数据
        //            /// 清除应用的用户数据
        //            /// </summary>
        //            ClearAppUserData,
        //            /// <summary>
        //            /// 底层访问权限
        //            /// 允许CWJ账户组访问底层信息
        //            /// </summary>
        //            CwjGroup,
        //            /// <summary>
        //            /// 手机优化大师扩展权限
        //            /// 手机优化大师扩展权限
        //            /// </summary>
        //            CellPhoneMasterEx,
        //            /// <summary>
        //            /// 控制定位更新
        //            /// 允许获得移动网络定位信息改变
        //            /// </summary>
        //            ControlLocationUpdates,
        //            /// <summary>
        //            /// 删除缓存文件
        //            /// 允许应用删除缓存文件
        //            /// </summary>
        //            DeleteCacheFiles,
        //            /// <summary>
        //            /// 删除应用
        //            /// 允许程序删除应用
        //            /// </summary>
        //            DeletePackages,
        //            /// <summary>
        //            /// 电源管理
        //            /// 允许访问底层电源管理
        //            /// </summary>
        //            DevicePower,
        //            /// <summary>
        //            /// 应用诊断
        //            /// 允许程序到RW到诊断资源
        //            /// </summary>
        //            Diagnostic,
        //            /// <summary>
        //            /// 禁用键盘锁
        //            /// 允许程序禁用键盘锁
        //            /// </summary>
        //            DisableKeyguard,
        //            /// <summary>
        //            /// 转存系统信息
        //            /// 允许程序获取系统dump信息从系统服务
        //            /// </summary>
        //            Dump,
        //            /// <summary>
        //            /// 状态栏控制
        //            /// 允许程序扩展或收缩状态栏
        //            /// </summary>
        //            ExpandStatusBar,
        //            /// <summary>
        //            /// 工厂测试模式
        //            /// 允许程序运行工厂测试模式
        //            /// </summary>
        //            FactoryTest,
        //            /// <summary>
        //            /// 使用闪光灯
        //            /// 允许访问闪光灯
        //            /// </summary>
        //            Flashlight,
        //            /// <summary>
        //            /// 强制后退
        //            /// 允许程序强制使用back后退按键
        //            /// </summary>
        //            ForceBack,
        //            /// <summary>
        //            /// 访问账户Gmail列表
        //            /// 访问GMail账户列表
        //            /// </summary>
        //            GetAccounts,
        //            /// <summary>
        //            /// 获取应用大小
        //            /// 获取应用的文件大小
        //            /// </summary>
        //            GetPackageSize,
        //            /// <summary>
        //            /// 获取任务信息
        //            /// 允许程序获取当前或最近运行的应用
        //            /// </summary>
        //            GetTasks,
        //            /// <summary>
        //            /// 允许全局搜索
        //            /// 允许程序使用全局搜索功能
        //            /// </summary>
        //            GlobalSearch,
        //            /// <summary>
        //            /// 硬件测试
        //            /// 访问硬件辅助设备
        //            /// </summary>
        //            HardwareTest,
        //            /// <summary>
        //            /// 注射事件
        //            /// 允许访问本程序的底层事件
        //            /// </summary>
        //            InjectEvents,
        //            /// <summary>
        //            /// 安装定位提供
        //            /// 安装定位提供
        //            /// </summary>
        //            InstallLocationProvider,
        //            /// <summary>
        //            /// 安装应用程序
        //            /// 允许程序安装应用
        //            /// </summary>
        //            InstallPackages,
        //            /// <summary>
        //            /// 内部系统窗口
        //            /// 允许程序打开内部窗口
        //            /// </summary>
        //            InternalSystemWindow,
        //            /// <summary>
        //            /// 访问网络
        //            /// 访问网络连接
        //            /// </summary>
        //            Internet,
        //            /// <summary>
        //            /// 结束后台进程
        //            /// 允许程序调用killBackgroundProcesses(String).方法结束后台进程
        //            /// </summary>
        //            KillBackgroundProcesses,
        //            /// <summary>
        //            /// 管理账户
        //            /// 允许程序管理AccountManager中的账户列表
        //            /// </summary>
        //            ManageAccounts,
        //            /// <summary>
        //            /// 管理程序引用
        //            /// 管理创建、摧毁、Z轴顺序
        //            /// </summary>
        //            ManageAppTokens,
        //            /// <summary>
        //            /// 高级权限
        //            /// 允许mTweak用户访问高级系统权限
        //            /// </summary>
        //            MtweakUser,
        //            /// <summary>
        //            /// 社区权限
        //            /// 允许使用mTweak社区权限
        //            /// </summary>
        //            MtweakForum,
        //            /// <summary>
        //            /// 软格式化
        //            /// 允许程序执行软格式化
        //            /// </summary>
        //            MasterClear,
        //            /// <summary>
        //            /// 修改声音设置
        //            /// 修改声音设置信息
        //            /// </summary>
        //            ModifyAudioSettings,
        //            /// <summary>
        //            /// 修改电话状态
        //            /// 修改电话状态
        //            /// </summary>
        //            ModifyPhoneState,
        //            /// <summary>
        //            /// 格式化文件系统
        //            /// 格式化可移动文件系统
        //            /// </summary>
        //            MountFormatFilesystems,
        //            /// <summary>
        //            /// 挂载文件系统
        //            /// 挂载、反挂载外部文件系统
        //            /// </summary>
        //            MountUnmountFilesystems,
        //            /// <summary>
        //            /// 允许NFC通讯
        //            /// 允许程序执行NFC近距离通讯操作
        //            /// </summary>
        //            Nfc,
        //            /// <summary>
        //            /// 永久Activity
        //            /// 创建一个永久的Activity
        //            /// </summary>
        //            PersistentActivity,
        //            /// <summary>
        //            /// 处理拨出电话
        //            /// 允许程序监视
        //            /// </summary>
        //            ProcessOutgoingCalls,
        //            /// <summary>
        //            /// 读取日程提醒
        //            /// 允许程序读取用户的日程信息
        //            /// </summary>
        //            ReadCalendar,
        //            /// <summary>
        //            /// 读取联系人
        //            /// 允许应用访问联系人通讯录信息
        //            /// </summary>
        //            ReadContacts,
        //            /// <summary>
        //            /// 屏幕截图
        //            /// 读取帧缓存用于屏幕截图
        //            /// </summary>
        //            ReadFrameBuffer,
        //            /// <summary>
        //            /// 读取收藏夹和历史记录
        //            /// 读取浏览器收藏夹和历史记录
        //            /// </summary>
        //            ReadHistoryBookmarks,
        //            /// <summary>
        //            /// 读取输入状态
        //            /// 读取当前键的输入状态
        //            /// </summary>
        //            ReadInputState,
        //            /// <summary>
        //            /// 读取系统日志
        //            /// 读取系统底层日志
        //            /// </summary>
        //            ReadLogs,
        //            /// <summary>
        //            /// 读取电话状态
        //            /// 访问电话状态
        //            /// </summary>
        //            ReadPhoneState,
        //            /// <summary>
        //            /// 读取短信内容
        //            /// 读取短信内容
        //            /// </summary>
        //            ReadSms,
        //            /// <summary>
        //            /// 读取同步设置
        //            /// 读取同步设置
        //            /// </summary>
        //            ReadSyncSettings,
        //            /// <summary>
        //            /// 读取同步状态
        //            /// 读取同步状态
        //            /// </summary>
        //            ReadSyncStats,
        //            /// <summary>
        //            /// 重启设备
        //            /// 允许程序重新启动设备
        //            /// </summary>
        //            Reboot,
        //            /// <summary>
        //            /// 开机自动允许
        //            /// 允许程序开机自动运行
        //            /// </summary>
        //            ReceiveBootCompleted,
        //            /// <summary>
        //            /// 接收彩信
        //            /// 接收彩信
        //            /// </summary>
        //            ReceiveMms,
        //            /// <summary>
        //            /// 接收短信
        //            /// 接收短信
        //            /// </summary>
        //            ReceiveSms,
        //            /// <summary>
        //            /// 接收Wap Push
        //            /// 接收WAP PUSH信息
        //            /// </summary>
        //            ReceiveWapPush,
        //            /// <summary>
        //            /// 录音
        //            /// 录制声音通过手机或耳机的麦克
        //            /// </summary>
        //            RecordAudio,
        //            /// <summary>
        //            /// 排序系统任务
        //            /// 重新排序系统Z轴运行中的任务
        //            /// </summary>
        //            ReorderTasks,
        //            /// <summary>
        //            /// 结束系统任务
        //            /// 结束任务通过restartPackage(String)方法
        //            /// </summary>
        //            RestartPackages,
        //            /// <summary>
        //            /// 发送短信
        //            /// 发送短信
        //            /// </summary>
        //            SendSms,
        //            /// <summary>
        //            /// 设置Activity观察其
        //            /// 设置Activity观察器一般用于monkey测试
        //            /// </summary>
        //            SetActivityWatcher,
        //            /// <summary>
        //            /// 设置闹铃提醒
        //            /// 设置闹铃提醒
        //            /// </summary>
        //            SetAlarm,
        //            /// <summary>
        //            /// 设置总是退出
        //            /// 设置程序在后台是否总是退出
        //            /// </summary>
        //            SetAlwaysFinish,
        //            /// <summary>
        //            /// 设置动画缩放
        //            /// 设置全局动画缩放
        //            /// </summary>
        //            SetAnimationScale,
        //            /// <summary>
        //            /// 设置调试程序
        //            /// 设置调试程序
        //            /// </summary>
        //            SetDebugApp,
        //            /// <summary>
        //            /// 设置屏幕方向
        //            /// 设置屏幕方向为横屏或标准方式显示
        //            /// </summary>
        //            SetOrientation,
        //            /// <summary>
        //            /// 设置应用参数
        //            /// 设置应用的参数
        //            /// </summary>
        //            SetPreferredApplications,
        //            /// <summary>
        //            /// 设置进程限制
        //            /// 允许程序设置最大的进程数量的限制
        //            /// </summary>
        //            SetProcessLimit,
        //            /// <summary>
        //            /// 设置系统时间
        //            /// 设置系统时间
        //            /// </summary>
        //            SetTime,
        //            /// <summary>
        //            /// 设置系统时区
        //            /// 设置系统时区
        //            /// </summary>
        //            SetTimeZone,
        //            /// <summary>
        //            /// 设置桌面壁纸
        //            /// 设置桌面壁纸
        //            /// </summary>
        //            SetWallpaper,
        //            /// <summary>
        //            /// 设置壁纸建议
        //            /// 设置壁纸建议
        //            /// </summary>
        //            SetWallpaperHints,
        //            /// <summary>
        //            /// 发送永久进程信号
        //            /// 发送一个永久的进程信号
        //            /// </summary>
        //            SignalPersistentProcesses,
        //            /// <summary>
        //            /// 状态栏控制
        //            /// 允许程序打开、关闭、禁用状态栏
        //            /// </summary>
        //            StatusBar,
        //            /// <summary>
        //            /// 访问订阅内容
        //            /// 访问订阅信息的数据库
        //            /// </summary>
        //            SubscribedFeedsRead,
        //            /// <summary>
        //            /// 写入订阅内容
        //            /// 写入或修改订阅内容的数据库
        //            /// </summary>
        //            SubscribedFeedsWrite,
        //            /// <summary>
        //            /// 显示系统窗口
        //            /// 显示系统窗口
        //            /// </summary>
        //            SystemAlertWindow,
        //            /// <summary>
        //            /// 更新设备状态
        //            /// 更新设备状态
        //            /// </summary>
        //            UpdateDeviceStats,
        //            /// <summary>
        //            /// 使用证书
        //            /// 允许程序请求验证从AccountManager
        //            /// </summary>
        //            UseCredentials,
        //            /// <summary>
        //            /// 使用SIP视频
        //            /// 允许程序使用SIP视频服务
        //            /// </summary>
        //            UseSip,
        //            /// <summary>
        //            /// 使用振动
        //            /// 允许振动
        //            /// </summary>
        //            Vibrate,
        //            /// <summary>
        //            /// 唤醒锁定
        //            /// 允许程序在手机屏幕关闭后后台进程仍然运行
        //            /// </summary>
        //            WakeLock,
        //            /// <summary>
        //            /// 写入GPRS接入点设置
        //            /// 写入网络GPRS接入点设置
        //            /// </summary>
        //            WriteApnSettings,
        //            /// <summary>
        //            /// 写入日程提醒
        //            /// 写入日程
        //            /// </summary>
        //            WriteCalendar,
        //            /// <summary>
        //            /// 写入联系人
        //            /// 写入联系人
        //            /// </summary>
        //            WriteContacts,
        //            /// <summary>
        //            /// 写入外部存储
        //            /// 允许程序写入外部存储
        //            /// </summary>
        //            WriteExternalStorage,
        //            /// <summary>
        //            /// 写入Google地图数据
        //            /// 允许程序写入Google Map服务数据
        //            /// </summary>
        //            WriteGservices,
        //            /// <summary>
        //            /// 写入收藏夹和历史记录
        //            /// 写入浏览器历史记录或收藏夹
        //            /// </summary>
        //            WriteHistoryBookmarks,
        //            /// <summary>
        //            /// 读写系统敏感设置
        //            /// 允许程序读写系统安全敏感的设置项
        //            /// </summary>
        //            WriteSecureSettings,
        //            /// <summary>
        //            /// 读写系统设置
        //            /// 允许读写系统设置项
        //            /// </summary>
        //            WriteSettings,
        //            /// <summary>
        //            /// 编写短信
        //            /// 允许编写短信
        //            /// </summary>
        //            WriteSms,
        //            /// <summary>
        //            /// 写入在线同步设置
        //            /// 写入Google在线同步设置
        //            /// </summary>
        //            WriteSyncSettings
        //        }
        //    }
    }
}