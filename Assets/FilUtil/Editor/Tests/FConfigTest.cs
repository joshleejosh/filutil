using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using FilUtil;

namespace UnityTest {
internal class FConfigTest {

    [Test]
    public void TestScalar() {
        var s = @"{
            ""astring"": ""this is a string."",
            ""anint"": 52461,
            ""afloat"": 0.95294,
            ""abool"": false,
            ""anull"": null
        }";

        FConfig d = new FConfig();
        d.LoadString(s);

        Assert.That(d.Get("astring") is string, Is.True);
        Assert.That((string)d.Get("astring"), Is.EqualTo("this is a string."));
        Assert.That(d.GetString("astring"), Is.EqualTo("this is a string."));

        Assert.That(d.Get("anint") is int, Is.True);
        Assert.That((int)d.Get("anint"), Is.EqualTo(52461));
        Assert.That(d.GetInt("anint"), Is.EqualTo(52461));

        Assert.That(d.Get("afloat") is float, Is.True);
        Assert.That((float)d.Get("afloat"), Is.EqualTo(0.95294f));
        Assert.That(d.GetFloat("afloat"), Is.EqualTo(0.95294f));

        Assert.That(d.Get("abool") is bool, Is.True);
        Assert.That((bool)d.Get("abool"), Is.False);
        Assert.That(d.GetBool("abool"), Is.False);

        Assert.That(d.Get("anull"), Is.Null);
    }

    [Test]
    public void TestVector() {
        var s = @"{
            ""a0"": [],
            ""a1"": [ 12 ],
            ""a2"": [ 34.5, 6 ],
            ""a3"": [ 7.89, 0.123, 45 ],
            ""d0"": {},
            ""dx"": { ""x"": 67.8 },
            ""dy"": { ""y"": 9.01 },
            ""dz"": { ""z"": 234.5 },
            ""dxy"": { ""x"": 6.7, ""y"": 901 },
            ""dxz"": { ""z"": 234, ""x"": 567 },
            ""dyz"": { ""y"": 890, ""z"": 123 },
            ""dxyz"": { ""x"": 456, ""y"": 789, ""z"": 0.12 },
        }";

        FConfig d = new FConfig();
        d.LoadString(s);
        Assert.That(d.GetVector("a0"), Is.EqualTo(Vector3.zero));
        Assert.That(d.GetVector("a1"), Is.EqualTo(new Vector3(12, 0, 0)));
        Assert.That(d.GetVector("a2"), Is.EqualTo(new Vector3(34.5f, 6, 0)));
        Assert.That(d.GetVector("a3"), Is.EqualTo(new Vector3(7.89f, 0.123f, 45)));

        Assert.That(d.GetVector("d0"), Is.EqualTo(Vector3.zero));

        Assert.That(d.GetVector("dx"), Is.EqualTo(new Vector3(67.8f, 0, 0)));
        Assert.That(d.GetVector("dy"), Is.EqualTo(new Vector3(0, 9.01f, 0)));
        Assert.That(d.GetVector("dz"), Is.EqualTo(new Vector3(0, 0, 234.5f)));

        Assert.That(d.GetVector("dxy"), Is.EqualTo(new Vector3(6.7f, 901, 0)));
        Assert.That(d.GetVector("dxz"), Is.EqualTo(new Vector3(567, 0, 234)));
        Assert.That(d.GetVector("dyz"), Is.EqualTo(new Vector3(0, 890, 123)));

        Assert.That(d.GetVector("dxyz"), Is.EqualTo(new Vector3(456, 789, 0.12f)));
    }

    [Test]
    public void TestDict() {
        var s = @"{
            ""a0"": {
                ""b0"": {
                    ""c0"": 123,
                    ""c1"": 456
                },
                ""b1"": {
                    ""c0"": 789,
                    ""c1"": 012,
                },
                ""b2"": {
                    ""c0"": 345,
                    ""c1"": 678,
                }
            },
            ""a1"": ""hi there!"",
            ""a2"": {
                ""b0"": {
                    ""c0"": 901,
                    ""c1"": 234
                },
                ""b1"": 567,
                ""b2"": {
                    ""c0"": 890,
                    ""c1"": 123,
                }
            }
        }";
        FConfig d = new FConfig();
        d.LoadString(s);

        Assert.That(d.GetInt("a0/b0/c0"), Is.EqualTo(123));
        Assert.That(d.GetInt("a0/b0/c1"), Is.EqualTo(456));
        Assert.That(d.GetInt("a0/b1/c0"), Is.EqualTo(789));
        Assert.That(d.GetInt("a0/b1/c1"), Is.EqualTo(12));
        Assert.That(d.GetInt("a0/b2/c0"), Is.EqualTo(345));
        Assert.That(d.GetInt("a0/b2/c1"), Is.EqualTo(678));
        Assert.That(d.GetString("a1"), Is.EqualTo("hi there!"));
        Assert.That(d.GetInt("a2/b0/c0"), Is.EqualTo(901));
        Assert.That(d.GetInt("a2/b0/c1"), Is.EqualTo(234));
        Assert.That(d.GetInt("a2/b1"), Is.EqualTo(567));
        Assert.That(d.GetInt("a2/b2/c0"), Is.EqualTo(890));
        Assert.That(d.GetInt("a2/b2/c1"), Is.EqualTo(123));
    }

    [Test]
    public void TestList() {
        var s = @"{
            ""da"": [
                ""a string"",
                ""another one"",
                ""the third in a series""
            ],
            ""db"": [
                { ""a"": 1, ""b"": 2, ""c"": 3 },
                { ""a"": 8, ""b"": 7, ""c"": 6 },
                { ""a"": 4, ""b"": 3, ""c"": 2 },
                { ""a"": 5, ""b"": 6, ""c"": 7 },
            ],
            ""dc"": [
                [ 46, 24, 86, 62 ],
                134.974,
                { ""p"": ""bug"", ""q"": ""lizard"" }
            ]
        }";
        FConfig d = new FConfig();
        d.LoadString(s);

        Assert.That(d.GetString("da/0"), Is.EqualTo("a string"));
        Assert.That(d.GetString("da/1"), Is.EqualTo("another one"));
        Assert.That(d.GetString("da/2"), Is.EqualTo("the third in a series"));

        Assert.That(d.GetInt("db/0/a"), Is.EqualTo(1));
        Assert.That(d.GetInt("db/1/b"), Is.EqualTo(7));
        Assert.That(d.GetInt("db/2/c"), Is.EqualTo(2));
        Assert.That(d.GetInt("db/3/b"), Is.EqualTo(6));

        Assert.That(d.GetInt("dc/0/1"), Is.EqualTo(24));
        Assert.That(d.GetInt("dc/0/3"), Is.EqualTo(62));
        Assert.That(d.GetFloat("dc/1"), Is.EqualTo(134.974f));
        Assert.That(d.GetString("dc/2/p"), Is.EqualTo("bug"));
    }

    [Test]
    public void TestPreparsed() {
        var d = new Dictionary<string,object>();
        d["a"] = 1;
        d["b"] = 2;
        d["c"] = 3;
        FConfig c = new FConfig(d);

        Assert.That(c.GetInt("a"), Is.EqualTo(1));
        Assert.That(c.GetInt("b"), Is.EqualTo(2));
        Assert.That(c.GetInt("c"), Is.EqualTo(3));

        // Giving an empty path to Get() should return the root.
        Assert.That(c.Get(""), Is.EqualTo(d));
    }
}
}
