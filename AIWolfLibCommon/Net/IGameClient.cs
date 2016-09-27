//
// IGameClient.cs
//
// Copyright (c) 2016 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

﻿namespace AIWolf.Common.Net
{
    interface IGameClient
    {
        object Recieve(Packet packet);
    }
}
