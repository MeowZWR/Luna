namespace Luna;

/// <summary> A text input to rename folders in the context menu. </summary>
/// <param name="fileSystem"> The file system. </param>
public sealed class RenameFolderInput(BaseFileSystem fileSystem) : BaseButton<IFileSystemFolder>
{
    /// <inheritdoc/>
    public override ReadOnlySpan<byte> Label(in IFileSystemFolder _)
        => "##Rename"u8;

    /// <summary> Replaces the normal menu item handling for a text input, so the other fields are not used. </summary>
    /// <inheritdoc/>
    public override bool DrawMenuItem(in IFileSystemFolder data)
    {
        var       currentPath = data.FullPath;
        var       ret         = false;
        using var style       = Im.Style.PushDefault(ImStyleDouble.FramePadding);

        MenuSeparator.DrawSeparator();

        if (Im.Window.Appearing)
            Im.Keyboard.SetFocusHere();
        Im.Text("重命名组（仅影响显示）:"u8);
        if (ImEx.InputOnDeactivation.Text("##Display"u8, data.DisplayName ?? string.Empty, out string newName, "组名称..."u8))
        {
            fileSystem.ChangeFolderDisplayName(data, newName);
            ret = true;
        }

        Im.Tooltip.OnHover("输入一个显示名称来重命名这个组。组将根据其路径排序，但显示此文本作为其名称。\n"u8
          + "一个空的显示名称意味着它使用路径作为其名称。\n\n"u8
          + "请注意，显示名称不必唯一，路径用于引用组。"u8);

        MenuSeparator.DrawSeparator();
        Im.Text("移动组:"u8);
        if (Im.Input.Text(Label(data), ref currentPath, flags: InputTextFlags.EnterReturnsTrue) && currentPath.Length > 0)
        {
            fileSystem.RenameAndMove(data, currentPath);
            fileSystem.ExpandAllAncestors(data);
            ret = true;
        }

        Im.Tooltip.OnHover("输入一个完整路径来移动或重命名这个组。如果符合条件，自动创建所有必需的父组。"u8);
        return ret;
    }
}
