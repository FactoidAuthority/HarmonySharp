#HarmonySharp API DLL
### An Open Source .NET implementation of the Harmony Connect API written in C#.


##### Depends on:

* Newtonsoft.json
* RestSharp


NOTE: As this was developed conjunction with the IOT-SAS.tech board, the Identity API has not been tested.

NuGet package is available.


#### Example use:

```
var harmony = new HarmonyClient("<api code>", "<api key>");
```

 ```
 var myChain = new Chain(harmony);
 var reply3 = myChain.GetChain("50535ffeca47307d4f912a396d4190105b150c23f95a1b4bab8acb0871b43328");
```
```
 var entry = new EntryData("This is a test", FactomUtils.MakeRandomExtIDs());
 var createEntry = new CreateAnEntry(harmony);
 createEntry.WriteToChain("50535ffeca47307d4f912a396d4190105b150c23f95a1b4bab8acb0871b43328", entry);
 ```
 
