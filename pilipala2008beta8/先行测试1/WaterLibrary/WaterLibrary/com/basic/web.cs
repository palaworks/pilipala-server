using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Web;
using System.Web.UI;

namespace WaterLibrary.com.basic
{
    /// <summary>
    /// 该类包含web编程常用的基本方法
    /// </summary>
    public class WebH : Page
    {
        /* cookie操作 */
        /// <summary>
        /// 判断Cookie对象是否存在
        /// </summary>
        /// <param name="Name">被判断Cookie对象的名称</param>
        /// <returns>存在返回true，反之false</returns>
        public bool IsCookiesExist(string Name)
        {
            if (HttpContext.Current.Request.Cookies[Name] == null
                || HttpContext.Current.Request.Cookies[Name].ToString() == "")//如果Cookie对象不存在或为空值
            {
                return false;//返回不存在
            }
            else
            {
                return true;
            }

        }
        /// <summary>
        /// 判断Cookie对象是否存在（重载二：判断索引是否存在）
        /// </summary>
        /// <param name="Name">被判断Cookie对象的名称</param>
        /// <param name="Key">索引名，属于被判断的Cookie</param>
        /// <returns>存在返回true，反之false</returns>
        public bool IsCookiesExist(string Name, string Key)
        {
            if (IsCookiesExist(Name) == false
                && HttpContext.Current.Request.Cookies[Name][Key] == null
                && HttpContext.Current.Request.Cookies[Name][Key].ToString() == "")//如果Cookie对象及其索引存在且不为空值
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// 读取Cookie对象到指定类型
        /// </summary>
        /// <typeparam name="T">指定泛型</typeparam>
        /// <param name="Name">被读取Cookie对象名</param>
        /// <returns>返回泛型值，读取失败则返回泛型默认值</returns>
        public T GetCookie<T>(string Name)
        {
            if (IsCookiesExist(Name) == true)//若Cookie存在
            {
                //转换为泛型返回
                return (T)Convert.ChangeType(HttpContext.Current.Request.Cookies[Name].Value, typeof(T));
            }
            else
            {
                return default;
            }

        }
        /// <summary>
        /// 读取Cookie对象的指定索引值到指定类型
        /// </summary>
        /// <typeparam name="T">指定泛型</typeparam>
        /// <param name="Name">被读取Cookie对象名</param>
        /// <param name="Key">索引名，属于当前Cookie对象</param>
        /// <returns>返回泛型值，读取失败则返回泛型默认值</returns>
        public T GetCookie<T>(string Name, string Key)
        {
            if (IsCookiesExist(Name, Key) == true)
            {
                return (T)Convert.ChangeType(HttpContext.Current.Request.Cookies[Name][Key].ToString(), typeof(T));
            }
            else
            {
                return default;
            }

        }

        /// <summary>
        /// 设置Cookie值
        /// </summary>
        /// <param name="Name">Cookie名，承担该操作</param>
        /// <param name="Val">设置值</param>
        /// <returns>设置成功返回true，反之false</returns>
        public bool SetCookie(string Name, object Val)
        {
            HttpContext.Current.Response.Cookies[Name].Value = Val.ToString();
            return true;
        }
        /// <summary>
        /// 设置Cookie值（重载二：索引设置）
        /// </summary>
        /// <param name="Name">Cookie名，承担该操作</param>
        /// <param name="Key">索引名，属于承担该操作的Cookie</param>
        /// <param name="Val">设置值</param>
        /// <returns>设置成功返回true，反之false</returns>
        public bool SetCookie(string Name, string Key, object Val)
        {
            HttpContext.Current.Response.Cookies[Name][Key] = Val.ToString();
            return true;
        }


        /* session操作 */
        /// <summary>
        /// 判断Cookie对象是否存在
        /// </summary>
        /// <param name="Name">被判断Cookie对象的名称</param>
        /// <returns>存在返回true，反之false</returns>
        public bool IsSessionExist(string Name)
        {
            if (Session[Name] == null
                || Session[Name].ToString() == "")//如果Session对象不存在或为空值
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// 读取Session对象到指定类型
        /// </summary>
        /// <typeparam name="T">指定泛型</typeparam>
        /// <param name="Name">变量名，属于Session对象，承担该操作</param>
        /// <returns>返回泛型值，读取失败则返回泛型默认值</returns>
        public T GetSession<T>(string Name)
        {
            if (IsSessionExist(Name) == true)
            {
                return (T)Convert.ChangeType(HttpContext.Current.Session[Name], typeof(T));
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// 设置Session对象的变量值
        /// </summary>
        /// <param name="Name">变量名，属于Session对象，承担该操作</param>
        /// <param name="Val">设置值</param>
        /// <returns>设置成功返回true，反之false</returns>
        public bool SetSession(string Name, object Val)
        {
            HttpContext.Current.Session[Name] = Val;
            return true;
        }
    }

}
