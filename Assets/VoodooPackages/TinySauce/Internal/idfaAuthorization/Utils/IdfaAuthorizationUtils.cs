﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Voodoo.Sauce.Internal.IdfaAuthorization
{
    public static class IdfaAuthorizationUtils
    {
        private const string TAG = "IdfaAuthorizationUtils";
        public static bool IsAttEnabled()
        {
            return Directory.Exists(IdfaAuthorizationConstants.WithAttFolder);
        }
    }
}
