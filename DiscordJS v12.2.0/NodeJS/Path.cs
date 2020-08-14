namespace NodeJS.Modules
{
    /// <summary>
    /// The path module equivalent to NodeJS
    /// </summary>
    public static class Path
    {
        /// <summary>
        /// Represents a parsed path
        /// </summary>
        public sealed class ParsedPath : IPathObject
        {
            /// <inheritdoc/>
            public string Root { get; }

            /// <inheritdoc/>
            public string Dir { get; }

            /// <inheritdoc/>
            public string Base { get; }

            /// <inheritdoc/>
            public string Ext { get; }

            /// <inheritdoc/>
            public string Name { get; }

            internal ParsedPath(string root, string dir, string @base, string ext, string name)
            {
                Root = root;
                Dir = dir;
                Base = @base;
                Ext = ext;
                Name = name;
            }
        }

        /// <summary>
        /// Makes an object a path-like object
        /// </summary>
        public interface IPathObject
        {
            /// <summary>
            /// The root of the path
            /// </summary>
            string Root { get; }

            /// <summary>
            /// The directory of the path
            /// </summary>
            string Dir { get; }

            /// <summary>
            /// The full file name
            /// </summary>
            string Base { get; }

            /// <summary>
            /// The extension of the file
            /// </summary>
            string Ext { get; }

            /// <summary>
            /// The name of the file
            /// </summary>
            string Name { get; }
        }

        /// <summary>
        /// The basic path object
        /// </summary>
        public class PathObject : IPathObject
        {
            /// <inheritdoc/>
            public string Root { get; set; }

            /// <inheritdoc/>
            public string Dir { get; set; }

            /// <inheritdoc/>
            public string Base { get; set; }

            /// <inheritdoc/>
            public string Ext { get; set; }

            /// <inheritdoc/>
            public string Name { get; set; }

            /// <summary>
            /// Instantiates a new path object with all properties being null
            /// </summary>
            public PathObject()
            {
                Root = null;
                Dir = null;
                Base = null;
                Ext = null;
                Name = null;
            }

            /// <summary>
            /// Instantiates a new path object with the given properties
            /// </summary>
            /// <param name="root">The root of the path</param>
            /// <param name="dir">The director of the path</param>
            /// <param name="base">The full file name of the path</param>
            /// <param name="ext">The file extension (includes '.')</param>
            /// <param name="name">The file name without the extension</param>
            public PathObject(string root, string dir, string @base, string ext, string name)
            {
                Root = root;
                Dir = dir;
                Base = @base;
                Ext = ext;
                Name = name;
            }
        }

        /// <summary>
        /// Return the last portion of a path. Similar to the Unix basename command. Often used to extract the file name from a fully qualified path.
        /// </summary>
        /// <param name="path">the path to evaluate.</param>
        /// <param name="ext">optionally, an extension to remove from the result.</param>
        /// <returns></returns>
        public static string Basename(string path, string ext = null)
        {
            string resExt = System.IO.Path.GetExtension(path);
            string resName = System.IO.Path.GetFileNameWithoutExtension(path);
            string resBase = System.IO.Path.GetFileName(path);
            if (ext != null && resExt.StartsWith(ext)) return resName;
            else return resBase;
        }

        /// <summary>
        /// Return the directory name of a path. Similar to the Unix dirname command.
        /// </summary>
        /// <param name="path">the path to evaluate.</param>
        /// <returns></returns>
        public static string Dirname(string path)
        {
            return System.IO.Path.GetDirectoryName(path);
        }

        /// <summary>
        /// Return the extension of the path, from the last '.' to end of string in the last portion of the path. If there is no '.' in the last portion of the path or the first character of it is '.', then it returns an empty string
        /// </summary>
        /// <param name="path">the path to evaluate.</param>
        /// <returns></returns>
        public static string Extname(string path)
        {
            return System.IO.Path.GetExtension(path);
        }

        /// <summary>
        /// Returns a path string from an object - the opposite of parse().
        /// </summary>
        /// <param name="pathObject">The path object</param>
        /// <returns></returns>
        public static string Format(IPathObject pathObject)
        {
            string dir = pathObject.Dir;
            string root = pathObject.Root;
            string Base = pathObject.Base;
            string name = pathObject.Name;
            string ext = pathObject.Ext;

            return System.IO.Path.Combine(dir == null ? root : dir, Base == null ? name + ext : Base);
        }

        /// <summary>
        /// Determines whether <paramref name="path"/> is an absolute path. An absolute path will always resolve to the same location, regardless of the working directory.
        /// </summary>
        /// <param name="path">The path to test</param>
        /// <returns></returns>
        public static bool IsAbsolute(string path)
        {
            return System.IO.Path.IsPathRooted(path);
        }

        /// <summary>
        /// Join all arguments together and normalize the resulting path. Arguments must be strings.
        /// </summary>
        /// <param name="paths">Paths to join</param>
        /// <returns></returns>
        public static string Join(params string[] paths)
        {
            return System.IO.Path.Combine(paths);
        }

        /// <summary>
        /// Normalize a string path, reducing '..' and '.' parts. When multiple slashes are found, they're replaced by a single one; when the path contains a trailing slash, it is preserved. On Windows backslashes are used.
        /// </summary>
        /// <param name="path">Path to normalize</param>
        /// <returns></returns>
        public static string Normalize(string path)
        {
            return System.IO.Path.GetFullPath(path);
        }

        /// <summary>
        /// Returns an object from a path string - the opposite of Format().
        /// </summary>
        /// <param name="path">path to evaluate.</param>
        /// <returns></returns>
        public static ParsedPath Parse(string path)
        {
            return new ParsedPath(System.IO.Path.GetPathRoot(path), 
                System.IO.Path.GetDirectoryName(path), 
                System.IO.Path.GetFileName(path), 
                System.IO.Path.GetExtension(path), 
                System.IO.Path.GetFileNameWithoutExtension(path));
        }
    }
}