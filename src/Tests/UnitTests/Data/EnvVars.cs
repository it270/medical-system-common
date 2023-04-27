using System.Collections.Generic;

namespace It270.MedicalSystem.Common.UnitTests.Data;

public static class EnvVars
{
    public static readonly KeyValuePair<string, string> AesKey = new("SYSTEM_AES_KEY", "3cza+181v0YN6v/4LNV8KkqrE0/PPA00AXvrYS/envk=");
    public static readonly KeyValuePair<string, string> AesIv = new("SYSTEM_AES_IV", "tst8a8IdZ5E2IX+pFs7v1g==");
}