/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later.
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System;
using System.Reflection;
using Vestris.ResourceLib;



namespace Dev2.Studio.Utils
{
    public static class VersionInfo
    {    
        public static string FetchVersionInfo()
        {
            var versionResource = GetVersionResource();
            return versionResource.FileVersion;
        }

        public static string FetchInformationalVersion()
        {
            var versionResource = GetVersionResource();
            return versionResource.ProductVersion;
        }

        public static Version FetchVersionInfoAsVersion()
        {
            var versionResource = GetVersionResource();
            return new Version(versionResource.FileVersion);
        }

        static VersionResource GetVersionResource()
        {
            var asm = Assembly.GetExecutingAssembly();
            var versionResource = new VersionResource();
            var fileName = asm.Location;
            versionResource.LoadFrom(fileName);
            return versionResource;
        }
    }
}