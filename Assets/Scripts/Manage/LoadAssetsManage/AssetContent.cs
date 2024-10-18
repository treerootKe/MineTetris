using System;
using UnityEngine;

namespace Manage.LoadAssetsManage
{
    public readonly struct AssetContent : IEquatable<AssetContent>
    {
        public readonly Transform TraParent;
        public readonly string StrPath;
        public readonly string StrType;
        
        public AssetContent(string strType, string strPath,Transform parent = null)
        {
            StrType = strType;
            StrPath = strPath;
            TraParent = parent;
        }

        public bool Equals(AssetContent other)
        {
            return Equals(TraParent, other.TraParent) && StrPath == other.StrPath && StrType == other.StrType;
        }

        public override bool Equals(object obj)
        {
            return obj is AssetContent other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (TraParent != null ? TraParent.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (StrPath != null ? StrPath.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (StrType != null ? StrType.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
} 