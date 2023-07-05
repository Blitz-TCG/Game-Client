using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using UnityEngine;

public class CardanoQuery : MonoBehaviour
{
    public static CardanoQuery instance;

    public List<string> tokenIDs = new List<string>();
    public List<long> tokenAmounts = new List<long>();

    public List<long> timer = new List<long>();

    private void Start()
    {
        Main();
    }

    public async void Main()
    {
        for (int i = 0; i < 1; i++)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            var client = new HttpClient();

            var payload = new Address
            {
                _addresses = new string[] { "addr1qyyhkt6gfxr3gut8p6j5wfwhssxs8yqklqpy2a5w3w4v78zf9d42g0jqnvsd3jk5ngkmqq9eqt0uw8ceg5xflmr34saq5apk6h" }//{ "addr1q9s7pyhdsxaykm3v64z0rmc6jn52adshcm8s9gjzdfm3zyk7wwuec6mu6rzxd77c2v3xsw8r9glp4g3nrlrycs6ttdjq9l7hkj" }
                //,"addr1q9xvgr4ehvu5k5tmaly7ugpnvekpqvnxj8xy50pa7kyetlnhel389pa4rnq6fmkzwsaynmw0mnldhlmchn2sfd589fgsz9dd0y" } //more address can be added via a comma delimited list
                //"addr1qyyhkt6gfxr3gut8p6j5wfwhssxs8yqklqpy2a5w3w4v78zf9d42g0jqnvsd3jk5ngkmqq9eqt0uw8ceg5xflmr34saq5apk6h"
            };

            var jsonData = JsonConvert.SerializeObject(payload);
            var contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.koios.rest/api/v0/address_assets", contentData);

            if (response.IsSuccessStatusCode)
            {

                var responseString = await response.Content.ReadAsStringAsync();

                var fullJSON = JsonConvert.DeserializeObject(responseString);
                Debug.Log(fullJSON);

                JsonTextReader reader = new JsonTextReader(new StringReader(responseString));
                using (reader)
                {
                    var result = new JsonSerializer().Deserialize<AddressAndAssetList[]>(reader);

                    foreach (var r in result)
                    {
                        Debug.Log($"Address: {r.Address}");
                        foreach (var c in r.Assets)
                        {
                            Debug.Log($"Policy ID: {c.PolicyID}");

                            //would add an if check here for if policy ID == "ABC", etc
                            /*foreach (var d in c.CompanyDefinitions)

                            {
                                //Debug.Log($"Asset Name: {d.AssetName}");
                                Debug.Log($"Asset Name Ascii: {d.AssetNameAscii}");
                                Debug.Log($"Balance: {d.Balance}");
                                tokenIDs.Add(d.AssetNameAscii);
                                tokenAmounts.Add(Int64.Parse(d.Balance));
                            }*/
                            Debug.Log($"Fingerprint: {c.Fingerprint}");
                            Debug.Log($"Quantity: {c.Quantity}");
                            tokenIDs.Add(c.Asset_Name);
                            tokenAmounts.Add(Int64.Parse(c.Quantity));
                        }
                    }
                }

            }
            else if (!response.IsSuccessStatusCode)
            {
                Debug.Log("throw error");
            }

            watch.Stop();
            //Debug.Log($"Execution Time: {watch.ElapsedMilliseconds} ms");
            timer.Add(watch.ElapsedMilliseconds);
        }

        double average = timer.Average();
        Debug.Log(average.ToString());
    }

    public class Address //payload out
    {
        public string[] _addresses { get; set; }

    }

    public sealed class AddressAndAssetList
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("asset_list")]
        public List<AssetListDetails> Assets { get; set; }
    }
    public sealed class AssetListDetails
    {
        [JsonProperty("policy_id")]
        public string PolicyID { get; set; }

        [JsonProperty("asset_name")]
        public string Asset_Name { get; set; }

        [JsonProperty("fingerprint")]
        public string Fingerprint { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

/*        [JsonProperty("assets")]
        public List<CompanyDefinition> CompanyDefinitions { get; set; }*/
    }
}
