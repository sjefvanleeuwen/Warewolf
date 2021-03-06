/*
*  Warewolf - Once bitten, there's no going back
*  Copyright 2019 by Warewolf Ltd <alpha@warewolf.io>
*  Licensed under GNU Affero General Public License 3.0 or later. 
*  Some rights reserved.
*  Visit our website for more information <http://warewolf.io/>
*  AUTHORS <http://warewolf.io/authors.php> , CONTRIBUTORS <http://warewolf.io/contributors.php>
*  @license GNU Affero General Public License <http://www.gnu.org/licenses/agpl-3.0.html>
*/

using System.Collections.Generic;
using Dev2.Common.Interfaces.Core.Graph;

namespace Dev2.Runtime.ServiceModel.Data
{
    public class RecordsetList : List<Recordset>
    {
        public IOutputDescription Description { get; set; }
    }

    public class RecordsetListWrapper
    {
       public  RecordsetList RecordsetList { get; set; }
       public  IOutputDescription Description { get; set; }

       public string SerializedResult { get; set; }
    }
}
