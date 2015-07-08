using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SimpleJSON;

public static class JSONSaver {

    public static BindingFlags FLAGS = BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance;

    public static string Save<T>(object obj, string location) {
        Type type = typeof(T);

        string s = "";
        try {
            ConvertToJSON(type, obj, ref s);
        } catch (Exception ex) {
            Debug.Log(ex);
        }


        return s;
    }

    public static void ConvertToJSON(Type type, object obj, ref string data) {
        data += "{";
        FieldInfo[] fields = type.GetFields(FLAGS);
        PropertyInfo[] props = type.GetProperties(FLAGS);

        foreach (FieldInfo field in fields) {
            //Debug.Log("--- Current Field: " + field.Name);
            resolveField(type, obj, field, ref data);
            data += ",";
            //Debug.Log("--- Field Done. ---");
        }

        if (fields.Length > 0 && props.Length == 0)
            data = data.Remove(data.Length - 1);
        foreach (PropertyInfo prop in props) {
            //Debug.Log("--- Current Property: " + prop.Name);
            if (resolveProperty(type, obj, prop, ref data))
                data += ",";
            //Debug.Log("--- Property Done. ---");
        }
        if (data[data.Length - 1] == ',')
            data = data.Remove(data.Length - 1);
        data += "}";
    }

    private static void resolveField(Type parentType, object obj, FieldInfo field, ref string data) {
        Type type = field.FieldType;

        if (type == typeof(int) || type == typeof(float) || type == typeof(double)) {
            data += "\"" + field.Name + "\":" + field.GetValue(obj).ToString();
        } else if (type == typeof(bool)) {
            data += "\"" + field.Name + "\":" + (((bool)field.GetValue(obj)) ? 1 : 0);
        } else if (type == typeof(string)) {
            try {
                data += "\"" + field.Name + "\":\"" + field.GetValue(obj).ToString() + "\"";
            } catch {
                Debug.Log(field.Name + " " + field.FieldType + " " + obj.GetType());
            }
        } else if (type.IsEnum) {
            object value = field.GetValue(obj);
            data += "\"" + field.Name + "\":" + Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()));
        } else if (type.IsValueType && !type.IsEnum && !type.IsPrimitive) {
            data += "\"" + field.Name + "\":";
            ConvertToJSON(type, field.GetValue(obj), ref data);
        } else {
            object o = null;
            try {
                o = field.GetValue(obj);
            } catch (Exception) {
                o = obj;
            }
            
            
            if (o == null) {
                data += "\"" + field.Name + "\":[]";
            } else {
                Type t = o.GetType();
                if (t.IsArray) {
                    Debug.LogError("JSON conversion from list not yet implemented.");
                } else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>)) {

                    data += "\"" + field.Name + "\":[";

                    foreach (object item in (IEnumerable)o) {
                        ConvertToJSON(item.GetType(), item, ref data);
                        data += ",";
                    }
                    if (data[data.Length - 1] == ',')
                        data = data.Remove(data.Length - 1);
                    data += "]";
                } else {
                    Debug.Log("Unknown type: " + t);
                }
            }
        }
    }


    private static bool resolveProperty(Type parentType, object obj, PropertyInfo prop, ref string data) {
        if (prop.GetSetMethod() == null)
            return false;

        Type type = prop.PropertyType;

        if (type == typeof(int) || type == typeof(float) || type == typeof(double)) {
            try {
                data += "\"" + prop.Name + "\":" + prop.GetValue(obj, null).ToString();
            } catch (TargetParameterCountException) {
                return false;
            } catch (Exception ex) {
                Debug.LogError(ex);
            }

            return true;
        } else if (type == typeof(bool)) {
            data += "\"" + prop.Name + "\":" + (((bool)prop.GetValue(obj, null)) ? 1 : 0);
        } else if (type == typeof(string)) {
            try {
                data += "\"" + prop.Name + "\":\"" + prop.GetValue(obj, null).ToString() + "\"";
                return true;
            } catch {
                Debug.Log(prop.Name + " " + prop.PropertyType + " " + obj.GetType());
            }
        } else if (type.IsEnum) {
            object value = prop.GetValue(obj, null);
            data += "\"" + prop.Name + "\":" + Convert.ChangeType(value, Enum.GetUnderlyingType(value.GetType()));
            return true;
        } else if (type.IsValueType && !type.IsEnum && !type.IsPrimitive) {

            object o = prop.GetValue(obj, null);
            if (o.GetType() != type) {
                data += "\"" + prop.Name + "\":";
                ConvertToJSON(type, o, ref data);
                return true;
            } else {
                return false;
            }

        } else {
            object o = prop.GetValue(obj, null);
            if (o == null) {
                data += "\"" + prop.Name + "\":[]";
                return true;
            } else {
                Type t = o.GetType();
                if (t.IsArray) {
                    Debug.LogError("JSON conversion from list not yet implemented.");
                } else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>)) {

                    data += "\"" + prop.Name + "\":[";

                    foreach (object item in (IEnumerable)o) {
                        ConvertToJSON(item.GetType(), item, ref data);
                        data += ",";
                    }
                    if (data[data.Length - 1] == ',')
                        data = data.Remove(data.Length - 1);
                    data += "]";
                    return true;
                } else {
                    Debug.Log("Unknown type: " + t);
                }
            }
        }
        return false;
    }

}