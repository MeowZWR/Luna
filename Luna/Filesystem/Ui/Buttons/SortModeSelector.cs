namespace Luna;

/// <summary> A menu selector for folder-specific sort-modes. </summary>
/// <param name="drawer"> The parent file system drawer. </param>
public class SortModeSelector(FileSystemDrawer drawer) : BaseButton<IFileSystemFolder>
{
    /// <inheritdoc/>
    public override ReadOnlySpan<byte> Label(in IFileSystemFolder data)
        => "排序模式"u8;

    /// <inheritdoc/>
    public override bool DrawMenuItem(in IFileSystemFolder data)
    {
        if (!SortModeCombo.DrawCombo(drawer.ValidSortModes, "单个折叠组排序"u8, data.SortMode, out var newSortMode, true,
                180 * Im.Style.GlobalScale))
            return false;

        drawer.FileSystem.ChangeFolderSortMode(data, newSortMode);
        return true;
    }
}
