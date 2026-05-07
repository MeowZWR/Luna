namespace Luna;

/// <summary> The button to dissolve a folder and move all its content into the parent folder. </summary>
/// <param name="fileSystem"> The file system. </param>
public sealed class DissolveFolderButton(BaseFileSystem fileSystem) : BaseButton<IFileSystemFolder>
{
    /// <inheritdoc/>
    public override ReadOnlySpan<byte> Label(in IFileSystemFolder _)
        => "解除折叠组"u8;

    /// <inheritdoc/>
    public override void DrawTooltip(in IFileSystemFolder _)
    {
        Im.Text("删除这个折叠组并将其所有子组移动到其父组中（如果有父组的话）。"u8);
        if (!LunaStyle.Modifier.Destructive)
            Im.Text($"\n按住 {LunaStyle.Modifier.Destructive} 并单击以解除折叠组。");
    }

    /// <inheritdoc/>
    public override bool HasTooltip
        => true;

    public override bool Enabled(in IFileSystemFolder data)
        => LunaStyle.Modifier.Destructive;

    /// <inheritdoc/>
    public override void OnClick(in IFileSystemFolder folder)
    {
        if (!folder.IsRoot)
            fileSystem.Merge(folder, folder.Parent!);
    }
}
