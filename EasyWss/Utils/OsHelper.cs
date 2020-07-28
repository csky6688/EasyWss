using Microsoft.Win32;
using System;
using System.Security.Principal;

namespace EasyWss.Utils
{
    class OsHelper
    {

        public static bool IsAdministrator()
        {
            WindowsIdentity current = WindowsIdentity.GetCurrent();
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
            //WindowsBuiltInRole可以枚举出很多权限，例如系统用户、User、Guest等等
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private static bool IsRegeditItemExist(string key)
        {
            string[] subkeyNames;
            RegistryKey hkml = Registry.ClassesRoot;
            RegistryKey software = hkml.OpenSubKey(@"*\shell");
            subkeyNames = software.GetSubKeyNames();

            //取得该项下所有子项的名称的序列，并传递给预定的数组中
            foreach (string keyName in subkeyNames)  //遍历整个数组
            {

                if (keyName == key) //判断子项的名称

                {

                    hkml.Close();

                    return true;

                }

            }

            hkml.Close();

            return false;

        }

        public static bool AddFileContextMenuItem(string itemName, string meanuName, string associatedProgramFullPath)
        {
            //创建项：shell 
            RegistryKey shellKey = Registry.ClassesRoot.OpenSubKey(@"*\shell", true);
            if (shellKey == null)
            {
                shellKey = Registry.ClassesRoot.CreateSubKey(@"*\shell");
            }


            //创建项：右键显示的菜单名称
            RegistryKey rightCommondKey = shellKey.CreateSubKey(itemName);
            RegistryKey associatedProgramKey = rightCommondKey.CreateSubKey("command");


            //创建默认值：关联的程序
            associatedProgramKey.SetValue(string.Empty, "\"" + associatedProgramFullPath + "\"" + " \"" + "%1" + "\"");
            rightCommondKey.SetValue(string.Empty, meanuName);
            rightCommondKey.SetValue("icon", associatedProgramFullPath);

            //刷新到磁盘并释放资源
            associatedProgramKey.Close();
            rightCommondKey.Close();
            shellKey.Close();

            return IsRegeditItemExist(itemName);
        }

        public static bool DelFileContextMenuItem(string itemName)
        {
            if (IsRegeditItemExist(itemName))
            {
                RegistryKey localKey;
                if (Environment.Is64BitOperatingSystem)
                    localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                else
                    localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                RegistryKey rk = localKey.OpenSubKey(@"SOFTWARE\Classes\*\shell", true);
                rk.DeleteSubKeyTree(itemName, true);
                rk.Close();
                localKey.Close();
            }
            return !IsRegeditItemExist(itemName);

        }



    }
}
