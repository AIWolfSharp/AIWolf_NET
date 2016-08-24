﻿//
// DataConverter.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace AIWolf.Common.Net
{
    /// <summary>
    /// Encodes object and decodes packet string.
    /// </summary>
    public class DataConverter
    {
        static DataConverter converter;

        /// <summary>
        /// A unique instance of this class.
        /// </summary>
        /// <returns>A unique instance of DataConverter class.</returns>
        public static DataConverter GetInstance()
        {
            if (converter == null)
            {
                converter = new DataConverter();
            }
            return converter;
        }

        JsonSerializerSettings serializerSetting;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        DataConverter()
        {
            serializerSetting = new JsonSerializerSettings();
            // Sort.
            serializerSetting.ContractResolver = new OrderedContractResolver();
            // Do not convert enum into integer.
            serializerSetting.Converters.Add(new StringEnumConverter());
        }

        /// <summary>
        /// Serializes the given object into the JSON string.
        /// </summary>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>The JSON string serialized from the given object.</returns>
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, serializerSetting);
        }

        /// <summary>
        /// Deserializes the given JSON string into the object of type T.
        /// </summary>
        /// <typeparam name="T">The type of object returned.</typeparam>
        /// <param name="json">The JSON string to be deserialized.</param>
        /// <returns>The object of type T deserialized from the JSON string.</returns>
        public T Deserialize<T>(string json)
        {
            return (T)JsonConvert.DeserializeObject<T>(json, serializerSetting);
        }

        class OrderedContractResolver : DefaultContractResolver
        {
            protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization)
            {
                return base.CreateProperties(type, memberSerialization).OrderBy(p => p.PropertyName).ToList();
            }
        }
    }
}
