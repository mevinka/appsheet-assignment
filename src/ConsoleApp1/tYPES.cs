using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;


/*
Defines the basic type classes for the JSON objects
the API's methods can return
*/
namespace Types
{
    /*
    A representation of a person
    Persons are ordered/comparable by age
    Persons have an ID, a name, an age, and an optional phone number
    */
    [DataContract]
    class Person : IComparable<Person>
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public String name { get; set; }
        [DataMember]
        public int age { get; set; }
        [DataMember]
        public String number { get; set; }

        /*
        Override: use age to compare two persons
        */
        public int CompareTo(Person that)
        {
            if (this.age < that.age) return -1;
            if (this.age == that.age) return 0;
            return 1;
        }

        /*
        Override: format object string form as:
        <ID>. <Name>, <age> (<number>) (optional phone number)
        */
        public override string ToString()
        {
            string result = id + ". " + name + ", age " + age;
            if (number.Length != 0)
            {
                result += " [" + number + "]";
            }

            return result;
        }
    }

    /*
    Basic representation of a list of IDs and the corresponding 
    token that the API returns
    */
    [DataContract]
    class IdList
    {
        [DataMember]
        public int[] result { get; set; }
        [DataMember]
        public String token { get; set; }
    }
}
