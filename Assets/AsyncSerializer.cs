using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using UnityEngine;


namespace My.Namespace
{
    [DataContract(IsReference = true), KnownType(typeof(SaveName))]
    public abstract class SaveData
    {
        [DataMember] protected bool enable;
        [DataMember] public string name;
        [DataMember] public object [] list;

        protected abstract void Save(bool enable);
    }

    [System.Serializable, DataContract(IsReference = true)]
    public class SaveName : SaveData
    {
        public SaveName(bool enable)
        {
            Save(enable);
        }

        protected override void Save(bool enable)
        {
            this.enable = enable;
        }
    }
}

[System.Serializable, DataContract]
public class ReferenceObject : AsyncSerializer.IKeyValue
{
    public string Key => GetType().Name;
    public object Value => values;

    IEnumerable<My.Namespace.SaveData> values;

    public ReferenceObject()
    {
        var saves = new My.Namespace.SaveData[1];
        saves[0] = new My.Namespace.SaveName(true)
        {
            name = "MyName",
            list = new object[1] { "MyName" }
        };
        values = saves;
    }
}

[System.Serializable, DataContract]
public enum EnumType
{
    [EnumMember(Value = "First")]
    First,
    [EnumMember(Value = "Second")]
    Second
}

public enum EnumNoContract
{
    FIRST,
    SECOND
}

[System.Serializable, DataContract]
public class EnumValue : AsyncSerializer.IKeyValue
{
    public string Key => GetType().Name;
    public object Value => type;

    EnumType type;

    public EnumValue(EnumType type)
    {
        this.type = type;
    }
}

[System.Serializable, DataContract]
public class EnumValueNoContract : AsyncSerializer.IKeyValue
{
    public string Key => GetType().Name;
    public object Value => values;

    [System.Serializable, DataContract]
    public class SaveValues
    {
        [DataMember] public List<Content> list;
    }

    [System.Serializable]
    public class Content
    {
        public EnumNoContract type;
        public bool alpha;
    }

    SaveValues values;

    public EnumValueNoContract(EnumNoContract type)
    {
        values = new SaveValues();
        values.list = new List<Content>();
        values.list.Add(new Content());
        values.list[0].type = type;
        values.list[0].alpha = true;
    }
}

[System.Serializable, DataContract]
public class ListOfList : AsyncSerializer.IKeyValue
{
    public string Key => GetType().Name;
    public object Value => values;

    SaveValues values;

    [System.Serializable, DataContract]
    public class SaveValues
    {
        [DataMember] public List<List<bool>> flags;
    }

    public ListOfList()
    {
        values = new SaveValues();
        values.flags = new List<List<bool>>();
        values.flags.Add(new List<bool>());
        values.flags[0].Add(true);
    }
}

[System.Serializable, DataContract]
public class Vector : AsyncSerializer.IKeyValue
{
    public string Key => GetType().Name;
    public object Value => new Vector3(1, 2, 3);
}

[System.Serializable, DataContract]
public class Vector2D : AsyncSerializer.IKeyValue
{
    public string Key => GetType().Name;
    public object Value => new Vector2Int(1, 2);
}

[System.Serializable, DataContract]
public class BoolAsString : AsyncSerializer.IKeyValue
{
    public string Key => GetType().Name;
    public object Value => false.ToString();
}

[System.Serializable, DataContract]
public class BoolAsObject: AsyncSerializer.IKeyValue
{
    public string Key => GetType().Name;
    public object Value => false;
}

[System.Serializable, DataContract]
public class ArrayOfNull : AsyncSerializer.IKeyValue
{
    public string Key => GetType().Name;
    public object Value => arrayOfNull;

    List<Vector> arrayOfNull;

    public ArrayOfNull()
    {
        arrayOfNull = new List<Vector>();
        arrayOfNull.Add(null);
    }
}

[System.Serializable, DataContract]
public class ArrayOfInt: AsyncSerializer.IKeyValue
{
    public string Key => GetType().Name;
    public object Value => arrayOfInt;

    List<int> arrayOfInt;

    public ArrayOfInt()
    {
        arrayOfInt = new List<int>();
        arrayOfInt.Add(1);
    }
}

[System.Serializable]
public class NonContract
{
    public int i;
}

[System.Serializable, DataContract]
public class EmptyArrayOfNonContract : AsyncSerializer.IKeyValue
{
    public string Key => GetType().Name;
    public object Value => saveValues;

    SaveValues saveValues;

    [System.Serializable, DataContract]
    public class SaveValues
    {
        [DataMember] public List<NonContract> contracts;
    }

    public EmptyArrayOfNonContract()
    {
        saveValues = new SaveValues();
        saveValues.contracts = new List<NonContract>();
    }
}

[System.Serializable, DataContract]
public class DictionaryData : AsyncSerializer.IKeyValue
{
    public string Key => GetType().Name;
    public object Value => saveValues;

    SaveValues saveValues;

    [System.Serializable, DataContract]
    public class SaveValues
    {
        [DataMember] public Dictionary<int, bool> entries;
    }

    public DictionaryData()
    {
        saveValues = new SaveValues();
        saveValues.entries = new Dictionary<int, bool>();
        saveValues.entries.Add(0, true);
        saveValues.entries.Add(1, false);
    }
}

[System.Serializable, DataContract]
public class Container : AsyncSerializer.IKeyValue
{
    [System.Serializable, DataContract]
    public class SaveValues
    {
        public SaveValues(int secret)
        {
            this.secret = secret;
        }

        [DataMember] int secret;
        [DataMember] public Vector3 vector;
        [DataMember] public double alpha;
        [DataMember] public float phi;
        [DataMember] public int beta;
        [DataMember] public byte gamma;
        [DataMember] public bool condition;
        [DataMember] public List<string> list;
        [DataMember] public List<Vector3> vectors;
        [DataMember] public List<ContractType> contracts;
        [DataMember] public object nil;
    }

    [DataMember]
    SaveValues value;

    public Container(int value)
    {
        this.value = new SaveValues(value)
        {
            vector = Vector3.one * value,
            alpha = 1.0 / (value + 1),
            phi = 1.0f / (value + 1),
            beta = value,
            gamma = (byte)value,
            condition = value != 0,
            list = new List<string>(),
            vectors = new List<Vector3>(),
            contracts = new List<ContractType>(),
            nil = null
        };
        for (int i = 0; i < value; ++i)
        {
            this.value.list.Add(i.ToString());
            this.value.vectors.Add(Vector3.one * i);
            this.value.contracts.Add(new ContractType(i));
        }
    }

    public string Key => GetType().Name;
    public object Value => value;
}

[System.Serializable, DataContract]
public class ContractType
{
    public ContractType(int i)
    {
        alpha = i;
    }

    [DataMember] public double alpha;
}

[System.Serializable, DataContract]
public class ContainerList : AsyncSerializer.IKeyValue
{
    [System.Serializable, DataContract]
    public class SaveValues
    {
        [DataMember] public double alpha;
        [DataMember] public byte gamma;
        [DataMember] public int beta;
    }

    SaveValues[] list = new SaveValues[0];

    public ContainerList()
    {
    }

    public ContainerList(int value)
    {
        list = new SaveValues[value];
        for (int i = 0; i < value; ++i)
        {
            list[i] = new SaveValues()
            {
                alpha = value,
                beta = value,
                gamma = (byte)value,
            };
        }
    }

    public string Key => GetType().Name;
    public object Value => list;
}

public class AsyncSerializer : MonoBehaviour
{
    public interface IKeyValue
    {
        string Key {  get; }
        object Value {  get; }
    }

    [System.Serializable, DataContract]
    [KnownType(typeof(SaveValue))]
    public class SaveValue
    {
        public SaveValue(IKeyValue keyValue)
        {
            this.keyValue = keyValue;
        }
        IKeyValue keyValue;

        [DataMember]
        public string Key { get { return keyValue.Key; } set { } } 
        [DataMember]
        public object Value { get { return keyValue.Value; } set { } }
    }

    IEnumerator Start()
    {
        var list = new List<SaveValue>();
        list.Add(new SaveValue(new ReferenceObject()));
        list.Add(new SaveValue(new ReferenceObject()));
        list.Add(new SaveValue(new BoolAsObject()));
        list.Add(new SaveValue(new EnumValueNoContract(EnumNoContract.FIRST)));
        list.Add(new SaveValue(new ListOfList()));
        list.Add(new SaveValue(new Vector2D()));
        list.Add(new SaveValue(new EnumValue(EnumType.First)));
        list.Add(new SaveValue(new EnumValue(EnumType.Second)));
        list.Add(new SaveValue(new DictionaryData()));
        list.Add(new SaveValue(new EmptyArrayOfNonContract()));
        list.Add(new SaveValue(new ArrayOfInt()));
        list.Add(new SaveValue(new ArrayOfNull()));
        list.Add(new SaveValue(new BoolAsString()));
        list.Add(new SaveValue(new Vector()));
        list.Add(new SaveValue(new ContainerList()));
        list.Add(new SaveValue(new ContainerList(1)));
        list.Add(new SaveValue(new Container(0)));
        list.Add(new SaveValue(new Container(1)));
        list.Add(new SaveValue(new Container(2)));
        yield return null;
        var serialiser = new DataContractSerializer(list.GetType());
        var asyncSerialiser = new AsyncSerialization.DataContractSerializer(list.GetType());
        var settings = new XmlWriterSettings()
        {
            Indent = true,
            IndentChars = "\t",
            NamespaceHandling = NamespaceHandling.OmitDuplicates
        };
        string xml;
        XmlSpy spy;
        using (var writer = new StringWriter())
        {
            using (var xwr = XmlWriter.Create(writer, settings))
            {
                spy = new XmlSpy(xwr, "serialise.log");
                try
                {
                    serialiser.WriteObject(spy, list);
                }
                catch (System.Exception exception)
                {
                    Debug.Log(exception.ToString());
                }
                spy.WriteLog();
            }
            xml = writer.ToString();
        }
        File.WriteAllText("original.xml", xml.ToString());
        Debug.Log(xml);

        using (var writer = new StringWriter())
        {
            using (var xwr = XmlWriter.Create(writer, settings))
            {
                spy.CheckLog(xwr);
                foreach (var entry in asyncSerialiser.WriteObject(spy, list))
                {
                    yield return null;
                }
            }
            xml = writer.ToString();
        }

        Debug.Log(xml);
    }
}
