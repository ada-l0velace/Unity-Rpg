using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SimpleJSON;

public class JSONLoader {

    public static BindingFlags FLAGS = BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance;



    public static T Convert<T>(string json) {
        JSONNode js = JSON.Parse(json);
        Type type = typeof(T);
        object myClass = Activator.CreateInstance(type);
        try {
            initClass(type, ref myClass, js);
        } catch (Exception ex) {
            Debug.LogException(ex);
        }
        return (T)myClass;
    }

    private static void resolveProperty(
        Type type, object obj, PropertyInfo prop, JSONNode node) {
        JSONData data = (JSONData)node.Value;
        switch (data.getTag()) {
            case JSONBinaryTag.IntValue:
                try {
                    prop.SetValue(obj, data.AsInt, null);
                } catch (ArgumentException) {
                    prop.SetValue(obj, data.Value, null);
                }
                break;
            case JSONBinaryTag.FloatValue:
                prop.SetValue(obj, data.AsFloat, null);
                break;
            case JSONBinaryTag.BoolValue:
                prop.SetValue(obj, data.AsBool, null);
                break;
            case JSONBinaryTag.Value:
                double d;
                if (node.Count == 0) {
                    if (double.TryParse(data.Value, out d))
                        prop.SetValue(obj, d, null);
                    else {
                        try {
                            prop.SetValue(obj, node.Value, null);
                        } catch (Exception) {

                        }

                    }
                } else {
                    Type fieldType = prop.PropertyType;
                    IList list = (IList)fieldType
                          .GetConstructor(Type.EmptyTypes)
                          .Invoke(null);

                    prop.SetValue(obj, list, null);

                    Type lt = list.GetType().GetGenericArguments()[0];
                    for (int i = 0, length = node.Count; i < length; i++) {
                        object o = Activator.CreateInstance(lt);
                        initClass(lt, ref o, node);
                        list.Add(o);
                    }
                }
                break;

            default:
                throw new Exception("JSONLoader unknown property type found. Type:" + type + " Value: " + node.Value);
        }
    }

    private static void resolveField(
        Type type, object obj, FieldInfo field, JSONNode node) {
        JSONData data = (JSONData)node.Value;
        Type fieldType = field.FieldType;

        if (fieldType == typeof(int)) {
            field.SetValue(obj, data.AsInt);
        } else if (fieldType == typeof(float) || fieldType == typeof(Single)) {
            field.SetValue(obj, data.AsFloat);
        } else if (fieldType == typeof(double)) {
            double d = 0;
            double.TryParse(data.Value, out d);
            field.SetValue(obj, d);
        } else if (fieldType == typeof(bool)) {
            field.SetValue(obj, data.AsBool);
        } else if (fieldType == typeof(string)) {
            field.SetValue(obj, data.Value);
        } else if (fieldType.IsEnum) {
            field.SetValue(obj, data.AsInt);
        } else if (fieldType.IsValueType && !fieldType.IsEnum && !fieldType.IsPrimitive) {
            //struct
            object o = Activator.CreateInstance(fieldType);
            initClass(fieldType, ref o, node);
            field.SetValue(obj, o);
        } else {
            IList list = (IList)fieldType
                  .GetConstructor(Type.EmptyTypes)
                  .Invoke(null);

            field.SetValue(obj, list);

            Type lt = list.GetType().GetGenericArguments()[0];

            foreach (JSONNode item in node.Childs) {
                switch (((JSONData)item.Value).getTag()) {
                    case JSONBinaryTag.IntValue:
                        try {
                            list.Add(item.AsInt);
                        } catch (ArgumentException) {
                            list.Add(item.Value);
                        }
                        break;
                    case JSONBinaryTag.FloatValue:
                        list.Add(item.AsFloat);
                        break;
                    case JSONBinaryTag.BoolValue:
                        list.Add(item.AsBool);
                        break;
                    default:
                        object o = Activator.CreateInstance(lt);
                        initClass(lt, ref o, item);
                        list.Add(o);
                        break;
                }
            }

        }

    }


    private static void initClass(Type type, ref object obj, JSONNode node) {
        FieldInfo[] fields = type.GetFields(FLAGS);
        PropertyInfo[] props = type.GetProperties(FLAGS);

        foreach (FieldInfo field in fields) {
            //Debug.Log("--- Current Field: " + field.Name);
            resolveField(type, obj, field, node[field.Name]);
            //Debug.Log("--- Field Done. ---");
        }

        foreach (PropertyInfo prop in props) {

            resolveProperty(type, obj, prop, node[prop.Name]);
        }
    }


} //class