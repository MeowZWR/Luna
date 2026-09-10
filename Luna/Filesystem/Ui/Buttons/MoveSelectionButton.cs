namespace Luna;

/// <summary> The button to move the current selection into the respective folder. </summary>
/// <param name="fileSystem"> The file system. </param>
public sealed class MoveSelectionButton(BaseFileSystem fileSystem) : BaseButton<IFileSystemFolder>
{
    /// <inheritdoc/>
    public override ReadOnlySpan<byte> Label(in IFileSystemFolder _)
        => "将选中项移动到此处"u8;

    /// <inheritdoc/>
    public override bool Enabled(in IFileSystemFolder data)
        => LunaStyle.Modifier.Misclick;

    /// <inheritdoc/>
    public override void OnClick(in IFileSystemFolder folder)
    {
        foreach (var obj in fileSystem.Selection.OrderedNodes)
        {
            if (obj != folder && obj.Parent != folder && folder.GetAncestors().All(a => a != obj))
                fileSystem.Move(obj, folder);
        }
    }

    /// <inheritdoc/>
    public override bool HasTooltip
        => true;

    /// <inheritdoc/>
    public override void DrawTooltip(in IFileSystemFolder _)
    {
        Im.Text("尽可能将当前选中项移动到此折叠组。其上级折叠组会被忽略。"u8);
        if (!LunaStyle.Modifier.Misclick)
            Im.Text($"\n按住 {LunaStyle.Modifier.Misclick} 点击以移动。");
    }
}
