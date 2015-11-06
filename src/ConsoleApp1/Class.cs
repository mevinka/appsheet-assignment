using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Net;
using System.IO;
using Types;

/* 
ApiConnector provides methods for interacting with the demo API
It defines methods for retrieving data from the API's two methods
This class assumes a connection to a specific URL
*/
class ApiConnector
{
    String Url { get; }
    WebClient Client;

    /*
    Constructor: assumes the provided URL
    */
    public ApiConnector()
    {
        Url = "https://appsheettest1.azurewebsites.net/sample/";
        Client = new WebClient();

    }

    /*
    Param: Optional token for retrieving certain range of IDs
    Returns a list of IDs taken from the API and a token if it exists 
    */
    public IdList MakeListRequest(String Token = "")
    {
        Byte[] result = Client.DownloadData(Url + "list?token=" + Token);
        MemoryStream apiResultStream = new MemoryStream(result);
        DataContractJsonSerializer serializer =
            new DataContractJsonSerializer(typeof(IdList));

        return (IdList)serializer.ReadObject(apiResultStream);
    }

    /*
    Param: Takes numerical ID for a specific record
    Returns a specific individual from the API 
    */
    public Person MakeDetailsRequest(int Id)
    {
        Byte[] result = Client.DownloadData(Url + "detail/" + Id);
        MemoryStream apiResultStream = new MemoryStream(result);
        DataContractJsonSerializer serializer =
            new DataContractJsonSerializer(typeof(Person));

        return (Person)serializer.ReadObject(apiResultStream);
    }

    /*
    Returns an array of all the ID numbers in the server
    using the list function in the API
    */
    public int[] GetAllIds()
    {
        List<int> ids = new List<int>();

        IdList responseIds = MakeListRequest();

        String token = responseIds.token;
        while (token.Length != 0)
        {
            foreach (int index in responseIds.result)
            {
                ids.Add(index);
            }

            responseIds = MakeListRequest(token);
            if (responseIds.token != null)      // Case: More IDs to request
            {
                token = responseIds.token;
            }
            else                               // Case: Run out of IDs
            {
                token = "";
            }
        }

        // Fencepost: do the last, non-token containing set of results
        foreach(int index in responseIds.result)
        {
            ids.Add(index);
        }

        return ids.ToArray();
    }
}
