#region Header
///Author:Mojiex
///Github:https://github.com/mojiex/Mojiex
///Create Time:2022/8/27
///Framework Description:This framework is developed based on unity2019LTS,Lower unity version may not supported.
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Mojiex
{
    public class PlayerPrefabHelper
    {
        private const string IV = "oregakimeruxxxxx";
	    public static void Save<T>(T data) where T : BaseSaveInfo
        {
            string saveStr = EncryptString(JsonMapper.ToJson(data), Const.Key, IV);
            PlayerPrefs.SetString(typeof(T).ToString(), saveStr);
        }

        public static T Load<T>() where T : BaseSaveInfo, new()
        {
            string key = typeof(T).ToString();
            if (!PlayerPrefs.HasKey(key))
            {
                T res = new T();
                res.InitDefault();
                return res;
            }
            string savedStr = DecryptString(PlayerPrefs.GetString(key), Const.Key, IV);
            T obj = JsonMapper.ToObject<T>(savedStr);
            //这里的保存主要是为了应对新添加的变量
            Save(obj);
            return obj;
        }

        public static string EncryptString(string plainText,string Key,string IV)
        {
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            byte[] encrypted;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.ASCII.GetBytes(Key);
                aesAlg.IV = Encoding.ASCII.GetBytes(IV);
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt, Encoding.UTF8))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        public static string DecryptString(string cipherStr, string Key, string IV)
        {
            var cipherText = Convert.FromBase64String(cipherStr);
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            string plaintext = null;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.ASCII.GetBytes(Key);
                aesAlg.IV = Encoding.ASCII.GetBytes(IV);
                //aesAlg.Padding = PaddingMode.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt, Encoding.UTF8))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
    }
}