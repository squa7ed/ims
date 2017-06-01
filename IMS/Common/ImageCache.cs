using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace IMS.Common
{
    public class ImageCache : IDisposable
    {
        private static Dictionary<int, WeakReference> _cache;

        private readonly IList<byte[]> bufArray;

        private readonly int decodeSize;

        public ImageCache(IList<byte[]> bufArray, int decodeSize)
        {
            this.bufArray = bufArray;
            this.decodeSize = decodeSize;
            _cache = new Dictionary<int, WeakReference>();
            for (int i = 0; i < bufArray.Count; i++)
            {
                using (var ms = new MemoryStream(bufArray[i]))
                {
                    var b = new BitmapImage();
                    b.BeginInit();
                    b.CacheOption = BitmapCacheOption.OnLoad;
                    b.DecodePixelWidth = decodeSize;
                    b.StreamSource = ms;
                    b.EndInit();
                    _cache.Add(i, new WeakReference(b, false));
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ImageCache() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
