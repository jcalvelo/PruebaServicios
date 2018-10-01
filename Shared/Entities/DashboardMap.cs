using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Entities {
    public class DashboardMap {
        [DataMember][BsonId]
        public int id;
        [DataMember]
        public String tipo;
        [DataMember]
        public Dictionary<string,string> map;
    }
}
