// a.snegovoy@gmail.com

namespace TheStore.Api.Front.Entity
{
    internal sealed class ImageInfo
    {
        public ImageInfo( byte[] image, bool isNotFound = false ) =>
            ( Image, IsNotFound ) = ( image, isNotFound );
        public byte[] Image { get; }
        public bool IsNotFound { get; }
    }
}