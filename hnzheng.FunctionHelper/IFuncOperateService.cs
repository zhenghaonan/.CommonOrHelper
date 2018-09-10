using hnzheng.FunctionHelper.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace hnzheng.FunctionHelper
{
    /// <summary>
    /// IFuncOperateService
    /// </summary>
    public interface IFuncOperateService
    {
        /// <summary>
        /// 执行一个方法
        /// </summary>
        /// <typeparam name="T">当前对象</typeparam>
        /// <param name="methodName"></param>
        /// <param name="parameters">此对象数组在数量、顺序和类型方面与要调用的方法或构造函数的参数相同</param>
        /// <param name="parametersTypes">参数的类型,默认defaultPubicMethod时，parametersTypes为Null</param>
        /// <param name="defaultPubicMethod">是否只获取公共方法</param>
        /// <returns>返回执行结果</returns>
        Task<ExecuteResult> Excute<T>(String methodName, object[] parameters, Type[] parametersTypes, Boolean defaultPubicMethod = false) where T : class;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblynName">程序集的名称</param>
        /// <param name="classFullName">对象的全称，加对应的程序集名称</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="parameters">参数</param>
        /// <param name="parametersTypes">参数类型，如果默认defaultPubicMethod ，为null</param>
        /// <param name="defaultPubicMethod">是否只获取公共方法</param>
        /// <returns>返回执行结果</returns>
        Task<ExecuteResult> Excute(String assemblynName, String classFullName, String methodName, Object[] parameters, Type[] parametersTypes, Boolean defaultPubicMethod = false);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblyFilePath">程序集的详细路径</param>
        /// <param name="assemblynName">程序集的名称</param>
        /// <param name="classFullName">对象的名称</param>
        /// <param name="methodName">方法的名称</param>
        /// <param name="parameters">参数</param>
        /// <param name="parametersTypes">参数类型，如果默认defaultPubicMethod ，为null</param>
        /// <param name="defaultPubicMethod">是否只获取公共方法</param>
        /// <returns>返回执行结果</returns>
        Task<ExecuteResult> Excute(String assemblyFilePath, String assemblynName, String classFullName, String methodName, Object[] parameters, Type[] parametersTypes, Boolean defaultPubicMethod = false);
        /// <summary>
        /// 根据程序集的名称，返回Assembly的list
        /// </summary>
        /// <param name="AssemblyNames">程序集名称</param>
        /// <returns></returns>
        IEnumerable<Assembly> LoadAssemblys(IEnumerable<String> AssemblyNames);
        /// <summary>
        /// 将value按照对应的type进行转换
        /// </summary>
        /// <param name="obj">转化后的值</param>
        /// <param name="type">转化类型</param>
        /// <param name="value">待转换的值</param>
        void GetParameterValue(ref Object obj, Type type, Object value);
        /// <summary>
        /// 获取一个参数列表
        /// </summary>
        /// <param name="tuples">待转化的类型和对应的值</param>
        /// <returns>转化后的参数列表</returns>
        Object[] GetParameterValues(IEnumerable<Tuple<Type, object>> tuples);
        /// <summary>
        /// 获取method的参数信息
        /// </summary>
        /// <typeparam name="T">当前类</typeparam>
        /// <param name="methodName">方法名称</param>
        /// <returns>参数信息</returns>
        ParameterInfo[] GetParameters<T>(String methodName) where T : class;
        /// <summary>
        /// 获取method的参数信息
        /// </summary>
        /// <param name="AssemblyName">程序集名称</param>
        /// <param name="ClassFullName">clas的完整名称</param>
        /// <param name="methodName">方法名称</param>
        /// <returns>参数信息</returns>
        ParameterInfo[] GetParameters(String assemblyName, String ClassFullName, String methodName);

        /// <summary>
        /// 获取带有该属性的类
        /// </summary>
        /// <typeparam name="T">属性</typeparam>
        /// <param name="Assembly">程序集</param>
        /// <returns></returns>
        IEnumerable<Type> GetClass<T>(Assembly assembly) where T : Attribute;
        /// <summary>
        /// 根据类型的Name转化类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="typeName"></param>
        /// <param name="value"></param>
        void GetParameterValue(ref Object obj, string typeName, Object value);
    }
}
