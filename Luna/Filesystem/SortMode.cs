namespace Luna;

/// <summary> An interface for different sort modes that can be used with a file system. </summary>
public interface ISortMode : IEquatable<ISortMode>
{
    /// <summary> The display name of the sort mode for combo selection. </summary>
    public ReadOnlySpan<byte> Name { get; }

    /// <summary> The description of the sort mode for combo tooltips. </summary>
    public ReadOnlySpan<byte> Description { get; }

    /// <summary> The method the sort mode uses to get the children of a folder in its specific order. </summary>
    /// <param name="folder"> The folder to fetch children from. </param>
    /// <returns> The children of <paramref name="folder"/> ordered according to this sort mode. </returns>
    public IEnumerable<IFileSystemNode> GetChildren(IFileSystemFolder folder);

    /// <summary> See <see cref="FoldersFirstT.Description"/>. </summary>
    public static readonly ISortMode FoldersFirst = new FoldersFirstT();

    /// <summary> See <see cref="LexicographicalT.Description"/>. </summary>
    public static readonly ISortMode Lexicographical = new LexicographicalT();

    /// <summary> See <see cref="InverseFoldersFirstT.Description"/>. </summary>
    public static readonly ISortMode InverseFoldersFirst = new InverseFoldersFirstT();

    /// <summary> See <see cref="InverseLexicographicalT.Description"/>. </summary>
    public static readonly ISortMode InverseLexicographical = new InverseLexicographicalT();

    /// <summary> See <see cref="FoldersLastT.Description"/>. </summary>
    public static readonly ISortMode FoldersLast = new FoldersLastT();

    /// <summary> See <see cref="InverseFoldersLastT.Description"/>. </summary>
    public static readonly ISortMode InverseFoldersLast = new InverseFoldersLastT();

    /// <summary> See <see cref="InternalOrderT.Description"/>. </summary>
    public static readonly ISortMode InternalOrder = new InternalOrderT();

    /// <summary> See <see cref="InverseInternalOrderT.Description"/>. </summary>
    public static readonly ISortMode InverseInternalOrder = new InverseInternalOrderT();

    /// <inheritdoc/>
    bool IEquatable<ISortMode>.Equals(ISortMode? other)
        => GetType() == other?.GetType();

    private struct FoldersFirstT : ISortMode
    {
        public ReadOnlySpan<byte> Name
            => "折叠组优先"u8;

        public ReadOnlySpan<byte> Description
            => "在每个折叠组中，按字典顺序排序所有子折叠组，然后按字典顺序排序所有数据节点。"u8;

        public IEnumerable<IFileSystemNode> GetChildren(IFileSystemFolder folder)
            => folder.GetSubFolders().Cast<IFileSystemNode>().Concat(folder.GetLeaves());
    }

    private struct LexicographicalT : ISortMode
    {
        public ReadOnlySpan<byte> Name
            => "字典顺序"u8;

        public ReadOnlySpan<byte> Description
            => "在每个折叠组中，按字典顺序排序所有子折叠组。"u8;

        public IEnumerable<IFileSystemNode> GetChildren(IFileSystemFolder folder)
            => folder.Children;
    }

    private struct InverseFoldersFirstT : ISortMode
    {
        public ReadOnlySpan<byte> Name
            => "折叠组优先 (反转)"u8;

        public ReadOnlySpan<byte> Description
            => "在每个折叠组中，按反字典顺序排序所有子折叠组，然后按反字典顺序排序所有数据节点。"u8;

        public IEnumerable<IFileSystemNode> GetChildren(IFileSystemFolder folder)
            => folder.GetSubFolders().Cast<IFileSystemNode>().Reverse().Concat(folder.GetLeaves().Reverse());
    }

    public struct InverseLexicographicalT : ISortMode
    {
        public ReadOnlySpan<byte> Name
            => "字典顺序 (反转)"u8;

        public ReadOnlySpan<byte> Description
            => "在每个折叠组中，按反字典顺序排序所有子折叠组。"u8;

        public IEnumerable<IFileSystemNode> GetChildren(IFileSystemFolder folder)
            => folder.Children.Reverse();
    }

    public struct FoldersLastT : ISortMode
    {
        public ReadOnlySpan<byte> Name
            => "折叠组最后"u8;

        public ReadOnlySpan<byte> Description
            => "在每个折叠组中，按字典顺序排序所有数据节点，然后按字典顺序排序所有子折叠组。"u8;

        public IEnumerable<IFileSystemNode> GetChildren(IFileSystemFolder folder)
            => folder.GetLeaves().Cast<IFileSystemNode>().Concat(folder.GetSubFolders());
    }

    public struct InverseFoldersLastT : ISortMode
    {
        public ReadOnlySpan<byte> Name
            => "折叠组最后 (反转)"u8;

        public ReadOnlySpan<byte> Description
            => "在每个折叠组中，按反字典顺序排序所有数据节点，然后按反字典顺序排序所有子折叠组。"u8;

        public IEnumerable<IFileSystemNode> GetChildren(IFileSystemFolder folder)
            => folder.GetLeaves().Cast<IFileSystemNode>().Reverse().Concat(folder.GetSubFolders().Reverse());
    }

    public struct InternalOrderT : ISortMode
    {
        public ReadOnlySpan<byte> Name
            => "内部顺序"u8;

        public ReadOnlySpan<byte> Description
            => "在每个折叠组中，按标识符顺序排序所有子折叠组（即按其在文件系统中的创建顺序排序）。"u8;

        public IEnumerable<IFileSystemNode> GetChildren(IFileSystemFolder folder)
            => folder.Children.OrderBy(c => c.Identifier);
    }

    public struct InverseInternalOrderT : ISortMode
    {
        public ReadOnlySpan<byte> Name
            => "内部顺序 (反转)"u8;

        public ReadOnlySpan<byte> Description
            => "在每个折叠组中，按反标识符顺序排序所有子折叠组（即按其在文件系统中的创建顺序反序排序）。"u8;

        public IEnumerable<IFileSystemNode> GetChildren(IFileSystemFolder folder)
            => folder.Children.OrderByDescending(c => c.Identifier);
    }
}
