using NUnit.Framework;

namespace GoogleDocs.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class BugAttribute(params int[] ids)
    : PropertyAttribute(string.Join(", ", ids));