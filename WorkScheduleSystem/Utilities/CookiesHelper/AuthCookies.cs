using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using WorkScheduleSystem.Common;
using WorkScheduleSystem.Model.Models;

namespace WorkScheduleSystem.Utilities.CookiesHelper
{
    public class AuthCookies
    {
        private static String Md5AuthKey = "Karma"; // 對個人資訊的cookie使用md5加密
        private static String AesAuthKey = "ReapWhatWeHasSow"; // md5加密後的cookie在使用一次Aes加密

        // cookies加密
        public static void AuthCookiesSetter(string account, int valueTime = 1)
        {

            string todayStr = DateTime.Now.ToString("yyyyMMdd");
            string md5Str = account + valueTime + todayStr + Md5AuthKey;
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            md5Str = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(md5Str))).Replace("-", null);            

            #region SetCookies
            string cookiesStr = $@"{account}|{valueTime}|{todayStr}|{md5Str}";
            string AesCookiesStr = AesEncrypt(cookiesStr, AesAuthKey);
            HttpCookie authCookie = new HttpCookie("WssAuth", AesCookiesStr);
            HttpContext.Current.Response.Cookies.Add(authCookie);
            #endregion
        }

        // cookies解密 =>  -1:不明錯誤(過期...)， 0:無此cookies， 1:驗證成功， 
        public static int AuthCookiesReader()
        {
            #region 步驟一: 查看是否有此cookies
            if (HttpContext.Current.Request.Cookies["WssAuth"] == null)
            {
                return 0; // 0:無此cookies
            }
            #endregion

            #region 步驟二: 有此cookie，先使用AES解密，再拆解字串
            string AesCookiesStr = HttpContext.Current.Request.Cookies["WssAuth"].Value;
            string cookieStr = AesDecrypt(AesCookiesStr, AesAuthKey);
            string[] cookieArr = cookieStr.Split(new char[] { '|' });
            #endregion

            #region 步驟三: 判斷cookie是否過期
            int todayStr = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int valueTime = int.Parse(cookieArr[1]);
            int cookieDate = int.Parse(cookieArr[2]);
            if ((todayStr - cookieDate) > valueTime) //若(今天-登入日)大於保留天數
            {
                CookieClear("WssAuth");
                return -1; // 2:cookie過期
            }
            #endregion

            #region 步驟四: 帳號解密處理
            string account = cookieArr[0];
            #endregion

            #region 步驟五: 比對加密字串是否一樣
            string md5Str = cookieArr[0] + cookieArr[1] + cookieArr[2] + Md5AuthKey;
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            md5Str = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(md5Str))).Replace("-", null);
            if (md5Str == cookieArr[3])
            {
                // AuthCookiesSetter(cookieArr[0]); // 若是有需要，重新設置時間
                return 1;  // 1:驗證成功
            }
            #endregion

            CookieClear("WssAuth");
            return -1;  // 2:不明錯誤
        }

        // cookies解密 => -1:不明錯誤， 0:無此cookies， >0:用戶Id
        public static int GetAuthUserUid()
        {
            #region 步驟一: 查看是否有此cookies
            if (HttpContext.Current.Request.Cookies["WssAuth"] == null)
            {
                return 0; // 0:無此cookies
            }
            #endregion

            #region 步驟二: 有此cookie，先使用AES解密，再拆解字串
            string AesCookiesStr = HttpContext.Current.Request.Cookies["WssAuth"].Value;
            string cookieStr = AesDecrypt(AesCookiesStr, AesAuthKey);
            string[] cookieArr = cookieStr.Split(new char[] { '|' });
            #endregion

            #region 步驟三: 判斷cookie是否過期
            int todayStr = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int valueTime = int.Parse(cookieArr[1]);
            int cookieDate = int.Parse(cookieArr[2]);
            if ((todayStr - cookieDate) > valueTime) //若(今天-登入日)大於保留天數
            {
                CookieClear("WssAuth");
                return 2; // 2:cookie過期
            }
            #endregion

            #region 步驟四: 帳號解密處理
            string account = cookieArr[0];
            #endregion

            #region 步驟五: 比對加密字串是否一樣
            string md5Str = cookieArr[0] + cookieArr[1] + cookieArr[2] + Md5AuthKey;
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            md5Str = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(md5Str))).Replace("-", null);
            if (md5Str == cookieArr[3])
            {
                // AuthCookiesSetter(cookieArr[0]); // 若是有需要，重新設置時間
                return SimpleFactory.CreateInstance().FindAll<UsersModel>().AsQueryable().Where(x => x.account == cookieArr[0]).FirstOrDefault().Id;
            }
            #endregion

            CookieClear("WssAuth");
            return -1;  // 2:不明錯誤
        }

        // 清除指定cookie
        public static void CookieClear(string key)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            var _response = httpContext.Response;

            HttpCookie cookie = new HttpCookie(key)
            {
                Expires = DateTime.Now.AddDays(-1) // or any other time in the past
            };
            _response.Cookies.Set(cookie);
        }


        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="str">明文（待加密）</param>
        /// <param name="key">密文</param>
        /// <returns></returns>
        private static string AesEncrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray);
        }

        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str">明文（待解密）</param>
        /// <param name="key">密文</param>
        /// <returns></returns>
        private static string AesDecrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            ICryptoTransform cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }
    }
}