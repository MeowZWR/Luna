namespace Luna;

/// <summary> Provides color pickers to specify individual colors for this folder. </summary>
/// <param name="drawer"> The parent drawer. </param>
public sealed class FolderColorEdits(FileSystemDrawer drawer) : BaseButton<IFileSystemFolder>
{
    /// <inheritdoc/>
    public override ReadOnlySpan<byte> Label(in IFileSystemFolder data)
        => "颜色编辑"u8;

    /// <inheritdoc/>
    public override bool DrawMenuItem(in IFileSystemFolder data)
    {
        var expandedColor = drawer.ExpandedFolderColor;
        if (ImEx.IconCheckbox("##defExp"u8, LunaStyle.LockedIcon, data.ExpandedColor.IsDefault, out var isDefault))
            drawer.FileSystem.ChangeFolderExpandedColor(data, isDefault ? ColorParameter.Default : expandedColor);

        Im.Tooltip.OnHover(isDefault ? "使用此处配置的自定义颜色。"u8 : "使用全局设置的折叠组颜色。"u8);

        Im.Line.SameInner();
        using (Im.Disabled(isDefault))
        {
            var color = data.ExpandedColor.Color?.ToVector() ?? expandedColor;
            if (Im.Color.Editor("展开折叠组颜色"u8, ref color, ColorEditorFlags.AlphaPreviewHalf | ColorEditorFlags.NoInputs))
                drawer.FileSystem.ChangeFolderExpandedColor(data, color);
        }

        var collapsedColor = drawer.CollapsedFolderColor;
        if (ImEx.IconCheckbox("##defColl"u8, LunaStyle.LockedIcon, data.CollapsedColor.IsDefault, out isDefault))
            drawer.FileSystem.ChangeFolderCollapsedColor(data, isDefault ? ColorParameter.Default : collapsedColor);

        Im.Tooltip.OnHover(isDefault ? "使用此处配置的自定义颜色。"u8 : "使用全局设置的折叠组颜色。"u8);

        Im.Line.SameInner();
        using (Im.Disabled(isDefault))
        {
            var color = data.CollapsedColor.Color?.ToVector() ?? collapsedColor;
            if (Im.Color.Editor("收起折叠组颜色"u8, ref color, ColorEditorFlags.AlphaPreviewHalf | ColorEditorFlags.NoInputs))
                drawer.FileSystem.ChangeFolderCollapsedColor(data, color);
        }

        return false;
    }
}
