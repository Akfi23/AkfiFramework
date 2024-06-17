using System;
using System.Collections.Generic;

public struct AKGenerationData 
{
    public AKGenerationData(string fileName, string[] usings, Type akType,
        Dictionary<int, string> properties)
    {
        FileName = fileName;
        Usings = usings;
        AKType = akType;
        Properties = properties;
    }

    public string FileName;
    public string[] Usings;
    public Type AKType;
    public Dictionary<int, string> Properties;
}
