﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Util
{
    interface ISplittable
    {
        List<byte[]> GetData();
    }
}
