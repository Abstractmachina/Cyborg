using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.GH
{
    public struct LoftOptions
    {
        bool adjustSeams;
        bool closedLoft;
        int rebuildCount;
        double refitTolerance;
        int loftType;
        //int loftFit;

        public LoftOptions(bool adjustSeams_, bool closedLoft_, int rebuildCount_, double refitTolerance_, int loftType_)
        {
            adjustSeams = adjustSeams_;
            closedLoft = closedLoft_;
            rebuildCount = rebuildCount_;
            refitTolerance = refitTolerance_;
            loftType = loftType_;
        }
    }
}
