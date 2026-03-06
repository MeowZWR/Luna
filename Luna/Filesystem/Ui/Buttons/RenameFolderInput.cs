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

        if (Im.Input.Text(Label(data), ref currentPath, flags: InputTextFlags.EnterReturnsTrue))
        {
            fileSystem.RenameAndMove(data, currentPath);
            fileSystem.ExpandAllAncestors(data);
            ret = true;
        }

        Im.Tooltip.OnHover("输入一个完整路径来移动或重命名这个折叠组。如果可能的话，会创建所有需要的父折叠组。"u8);
        return ret;
    }
}
