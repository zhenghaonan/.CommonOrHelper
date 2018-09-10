using hnzheng.FunctionHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace hnzheng.FunctionHelper
{
    public class FuncOperateService : IFuncOperateService
    {

        private static FuncOperateService _funcOperateService = null;
        public static FuncOperateService CreateInstance()
        {
            if (_funcOperateService == null)
                _funcOperateService = new FuncOperateService();
            return _funcOperateService;
        }
        private static readonly Object obj = new Object();
        public async Task<ExecuteResult> Excute<T>(String methodName, Object[] parameters, Type[] parametersTypes, Boolean defaultPubicMethod = false) where T : class
        {
            return await Excute(typeof(T), methodName, parameters, parametersTypes, defaultPubicMethod);
        }
        public Task<ExecuteResult> Excute(String assemblynName, String classFullName, String methodName, Object[] parameters, Type[] parametersTypes, Boolean defaultPubicMethod = false)
        {
            try
            {
                Assembly assembly = Assembly.Load(new AssemblyName(assemblynName));
                if (assembly == null)
                    throw new ArgumentNullException(String.Format("Can't load Assembly {0}", assemblynName));
                Type type = assembly.GetType(classFullName);
                if (type == null)
                    throw new ArgumentNullException(String.Format("Can't get type,Assembly {0}", assemblynName));
                return Excute(type, methodName, parameters, parametersTypes, defaultPubicMethod);
            }
            catch
            {
                throw new Exception(String.Format("Can't load Assembly {0}", assemblynName));
            }
        }
        public async Task<ExecuteResult> Excute(String assemblyFilePath, String assemblynName, String classFullName, String methodName, Object[] parameters, Type[] parametersTypes, Boolean defaultPubicMethod = false)
        {
            try
            {
                //Assembly assembly = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "Bin/App_Code.dll");  
                //加载程序集(dll文件地址)，使用Assembly类   
                Assembly assembly = Assembly.LoadFile(assemblyFilePath);
                if (assembly == null)
                    throw new ArgumentNullException(String.Format("Can't load Assembly {0}", assemblyFilePath));
                Type type = assembly.GetType(assemblynName + "." + classFullName);
                if (type == null)
                    throw new ArgumentNullException(String.Format("Can't get type,Assembly {0}", assemblyFilePath));
                return await Excute(type, methodName, parameters, parametersTypes, defaultPubicMethod);
            }
            catch
            {
                throw new Exception(String.Format("Can't load Assembly {0}", assemblynName));
            }

        }

        public IEnumerable<Assembly> LoadAssemblys(IEnumerable<String> AssemblyNames)
        {
            foreach (string assembly in AssemblyNames)
            {
                yield return Assembly.Load(assembly);
            }
        }
        public virtual void GetParameterValue(ref Object obj, Type type, Object value)
        {
            try
            {
                if (type == typeof(System.String))
                    obj = value;
                else if (type == typeof(System.Int32))
                    obj = Convert.ToInt32(value);
                else if (type == typeof(System.DateTime))
                    obj = Convert.ToDateTime(value);
                else if (type == typeof(System.Decimal))
                    obj = Convert.ToDecimal(value);
                else if (type == typeof(System.Boolean))
                    obj = Convert.ToBoolean(value);
                else if (type == typeof(System.Double))
                    obj = Convert.ToDouble(value);
                else if (type == typeof(System.Single))
                    obj = Convert.ToSingle(value);
                else
                    throw new ArgumentOutOfRangeException(String.Format("Out of range,Please check the  value type,detail:obj:{0},Type:{1},Value:{2}", obj, type, value));
            }
            catch
            {
                throw new ArgumentException(String.Format("Can't convet {0} to {1}", value, type));
            }
        }
        public virtual void GetParameterValue(ref Object obj, string typeName, Object value)
        {
            try
            {
                if (typeName == "System.String")
                    obj = value;
                else if (typeName == "System.Int32")
                    obj = Convert.ToInt32(value);
                else if (typeName == "System.DateTime")
                    obj = Convert.ToDateTime(value);
                else if (typeName == "System.Decimal")
                    obj = Convert.ToDecimal(value);
                else if (typeName == "System.Boolean")
                    obj = Convert.ToBoolean(value);
                else if (typeName == "System.Double")
                    obj = Convert.ToDouble(value);
                else if (typeName == "System.Single")
                    obj = Convert.ToSingle(value);
                else
                    throw new ArgumentOutOfRangeException(String.Format("Out of range,Please check the  value type,detail:obj:{0},Type:{1},Value:{2}", obj, typeName, value));
            }
            catch
            {
                throw new ArgumentException(String.Format("Can't convet {0} to {1}", value, typeName));
            }

        }
        public virtual Object[] GetParameterValues(IEnumerable<Tuple<Type, object>> tuples)
        {
            Object[] obj = new Object[tuples.Count()];
            int i = 0;
            foreach (var item in tuples)
            {
                GetParameterValue(ref obj[i], item.Item1, item.Item2);
                i++;
            }
            return obj;
        }
        public virtual IEnumerable<Type> GetClass<T>(Assembly assembly) where T : Attribute
        {
            try
            {
                IList<Type> newTypes = new List<Type>();
                foreach (Type type in assembly.GetTypes())
                {
                    var attr = type.GetCustomAttributesData();
                    if (attr.Any() && attr.First().AttributeType == typeof(T))
                        newTypes.Add(type);
                }
                return newTypes;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Get type Failed,Detail:{0},Error{1}", "GetClass", ex));
            }
        }
        public ParameterInfo[] GetParameters<T>(String methodName) where T : class
        {
            return typeof(T).GetTypeInfo().GetMethod(methodName).GetParameters();
        }
        public ParameterInfo[] GetParameters(String assemblyName, String ClassFullName, String methodName)
        {
            try
            {
                Assembly assembly = Assembly.Load(new AssemblyName(assemblyName));
                Type type = assembly.GetType(ClassFullName);
                return type.GetTypeInfo().GetMethod(methodName).GetParameters();
            }
            catch
            {
                throw new Exception(String.Format("Can't load Assembly {0}", assemblyName));
            }
        }



        #region  private

        private async Task<ExecuteResult> Excute(Type type, String methodName, Object[] parameters, Type[] parametersTypes, Boolean defaultPubicMethod = false)
        {
            ExecuteResult executeResult = new ExecuteResult();
            executeResult.IsSuccess = false;
            executeResult.ExcuteStatus = ExcuteStatus.ToExecute;
            MethodInfo method = defaultPubicMethod ? type.GetTypeInfo().GetMethod(methodName) : type.GetTypeInfo().GetMethod(methodName, parametersTypes);
            if (method == null)
                throw new Exception("The method can't find，Please check the assemblynNameInfo！");
            lock (obj)
            {
                if (!method.IsStatic)
                {
                    Object operate = Activator.CreateInstance(type);
                    if (operate != null)
                    {
                        try
                        {
                            executeResult.ExcuteStatus = ExcuteStatus.Executing;
                            executeResult.ReturnData = method.Invoke(operate, parameters);
                            executeResult.ExcuteStatus = ExcuteStatus.Executed;
                            executeResult.IsSuccess = true;
                        }
                        catch
                        {
                            throw new MethodAccessException("method  operate failed");
                        }
                    }
                    else
                        throw new ArgumentNullException("Create instance  failed!");
                }
                else
                {
                    try
                    {
                        executeResult.ExcuteStatus = ExcuteStatus.Executing;
                        executeResult.ReturnData = method.Invoke(null, parameters);
                        executeResult.ExcuteStatus = ExcuteStatus.Executed;
                        executeResult.IsSuccess = true;
                    }
                    catch
                    {
                        throw; //new MethodAccessException("Parameters mismatch,Please check the assemblynNameInfo！");
                    }
                }
            }
            executeResult.IsSuccess = true;
            return executeResult;
        }
        #endregion
    }
}
