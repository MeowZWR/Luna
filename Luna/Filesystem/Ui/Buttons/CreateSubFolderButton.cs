namespace Luna;

/// <summary> The button to move the current selection into the respective folder. </summary>
/// <param name="fileSystem"> The file system. </param>
public sealed class CreateSubFolderButton(BaseFileSystem fileSystem) : BaseButton<IFileSystemFolder>
{
    /// <inheritdoc/>
    public override ReadOnlySpan<byte> Label(in IFileSystemFolder _)
        => "在此创建子组"u8;

    /// <inheritdoc/>
    public override void OnClick(in IFileSystemFolder folder)
        => Im.Popup.Open($"CSF{folder.Identifier.Value}");

    /// <inheritdoc/>
    protected override bool MenuItem(in IFileSystemFolder data)
        => Im.Selectable(Label(data), false, SelectableFlags.NoAutoClosePopups);

    /// <inheritdoc/>
    public override bool HasTooltip
        => true;

    /// <inheritdoc/>
    public override void DrawTooltip(in IFileSystemFolder _)
        => Im.Text("在此新建一个空白子组。可以包含 '/' 来创建多个嵌套子组。"u8);

    /// <inheritdoc/>
    protected override void PostDraw(in IFileSystemFolder parentFolder)
    {
        // Handle the actual popup.
        if (!InputPopup.Open($"CSF{parentFolder.Identifier.Value}", StringU8.Empty, out var newName, "输入子组名称..."u8))
            return;

        try
        {
            var fullPath = $"{parentFolder.FullPath}/{newName.AsSpan().Trim('/')}";
            var folder   = fileSystem.FindOrCreateAllFolders(fullPath);
            fileSystem.ExpandAllAncestors(folder);
        }
        catch
        {
            // Ignored
        }
    }
}
