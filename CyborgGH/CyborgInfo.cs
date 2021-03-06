﻿using System;
using System.Drawing;
using Grasshopper.Kernel;
using System.IO;
using Cyborg.CyborgGH.Properties;

namespace Cyborg
{
    public class CyborgInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return Cyborg.GH.Strings.LIB_NAME;
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return Resources.cyborg_logo;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "Custom Functionality for Grasshopper";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("e235eca4-d625-4041-9314-27188a37d740");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Taole Chen";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
