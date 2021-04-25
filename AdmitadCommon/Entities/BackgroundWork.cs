// a.snegovoy@gmail.com

using System;

namespace AdmitadCommon.Entities
{
    public sealed record BackgroundWork(
        Action Action,
        string Id );
}