namespace Forms.HotKeys;

/// <summary>
/// Interface for OS-specific keyboard shortcuts.
/// </summary>
public interface IHotKeys
{
    string Modifier { get; }

    string SelectAll { get; }
    string Copy { get; }
    string Paste { get; }
    string Cut { get; }
    string Undo { get; }
    string Redo { get; }
    string Save { get; }
    string Find { get; }

    string OpenFilterMenu { get; }
    string ClearFormatting { get; }
    string InsertLink { get; }
    string Bold { get; }
    string Italic { get; }
    string Underline { get; }
}
