using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Clarity.App.Worlds.Assets
{
    public class AssetHashMd5 : IEquatable<AssetHashMd5>
    {
        public byte[] Bytes { get; }

        public AssetHashMd5(byte[] bytes)
        {
            if (bytes == null || bytes.Length != 16)
                throw new ArgumentException("MD5 hash must be 16 bytes long.");
            Bytes = bytes;
        }

        public bool Equals(AssetHashMd5 other)
        {
            if (ReferenceEquals(other, null))
                return false;
            for (int i = 0; i < Bytes.Length; i++)
                if (Bytes[i] != other.Bytes[i])
                    return false;
            return true;
        }

        public override bool Equals(object obj) => obj is AssetHashMd5 md5 && Equals(md5);
        public override int GetHashCode() => Bytes[0] | (Bytes[1] << 8) | (Bytes[2] << 16) | (Bytes[3] << 24);
        public static bool operator ==(AssetHashMd5 first, AssetHashMd5 second) => first?.Equals(second) ?? false;
        public static bool operator !=(AssetHashMd5 first, AssetHashMd5 second) => !(first == second);

        public static AssetHashMd5 FromSingleFile(byte[] fileData)
        {
            // MD5.Create() does not work in some cases with Unity
            // https://stackoverflow.com/questions/30055358/md5-gethash-work-only-in-unity-editor
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var hash = md5.ComputeHash(fileData);
                return new AssetHashMd5(hash);
            }
        }

        public static AssetHashMd5 FromSingleFile(Stream fileReadStream)
        {
            // MD5.Create() does not work in some cases with Unity
            // https://stackoverflow.com/questions/30055358/md5-gethash-work-only-in-unity-editor
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var hash = md5.ComputeHash(fileReadStream);
                return new AssetHashMd5(hash);
            }
        }

        public static AssetHashMd5 FromMultipleFiles(IEnumerable<byte[]> filesData)
        {
            return FromHashes(filesData.Select(FromSingleFile).ToArray());
        }

        public static AssetHashMd5 FromMultipleFiles(Func<string, Stream> openFile, IEnumerable<string> paths)
        {
            var hashes = new List<AssetHashMd5>();
            foreach (var path in paths)
                using (var stream = openFile(path))
                    hashes.Add(FromSingleFile(stream));
            return FromHashes(hashes);
        }

        public static AssetHashMd5 FromHashes(IReadOnlyList<AssetHashMd5> hashes)
        {
            var data = new byte[hashes.Sum(x => x.Bytes.Length)];
            var offset = 0;
            foreach (var hash in hashes)
            {
                Array.Copy(hash.Bytes, 0, data, offset, hash.Bytes.Length);
                offset += hash.Bytes.Length;
            }
            return FromSingleFile(data);
        }

        public static AssetHashMd5 Random(Random random)
        {
            using (var memStream = new MemoryStream())
            using (var writer = new BinaryWriter(memStream))
            {
                writer.Write(random.NextDouble());
                writer.Write(random.NextDouble());
                writer.Write(random.NextDouble());
                writer.Write(random.NextDouble());
                writer.Flush();
                var data = memStream.ToArray();
                return FromSingleFile(data);
            }
        }
    }
}