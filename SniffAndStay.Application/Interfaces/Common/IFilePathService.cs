using System;
using System.Collections.Generic;
using System.Text;

namespace SniffAndStay.Application.Interfaces.Common
{
    public interface IFilePathService
    {
        string ResolveFilePath(string fileName, FileType fileType);
        string ResolveFileName(Guid userId, string fileName);
    }
}
