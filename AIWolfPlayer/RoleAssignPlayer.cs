//
// RoleAssignPlayer.cs
//
// Copyright (c) 2017 Takashi OTSUKI
//
// This software is released under the MIT License.
// http://opensource.org/licenses/mit-license.php
//

using System;
using AIWolf.Player.Lib;

namespace AIWolf.Player.Sample
{
#if JHELP
    /// <summary>
    /// 役職ごとに実際に使用するプレイヤーを切り替えるプレイヤー
    /// </summary>
#else
    /// <summary>
    /// Player class which switches player actually used according to its role.
    /// </summary>
#endif
    public class RoleAssignPlayer : AbstractRoleAssignPlayer
    {
#if JHELP
        /// <summary>
        /// サンプルエージェントを使うので何も設定しない
        /// </summary>
#else
        /// <summary>
        /// There is no setting to use sample agents.
        /// </summary>
#endif
        protected override void SetPlayers()
        {
        }
    }
}
