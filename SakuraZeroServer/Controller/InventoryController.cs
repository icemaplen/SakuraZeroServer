using SakuraZeroCommon.Core;
using SakuraZeroCommon.Protocol;
using SakuraZeroServer.Core;
using SakuraZeroServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SakuraZeroServer.Controller
{
    class InventoryController : BaseController
    {
        public InventoryController()
        {
            requestCode = ERequestCode.Inventory;
        }

        public void GetAllItems(Conn conn, ProtocolBase protocol)
        {
            ProtocolBytes result;
            List<Item> itemList = dataMgr.GetAllItems(conn.player.ID);
            if (itemList != null)
            {
                result = new ProtocolBytes(requestCode, EActionCode.GetAllItems, EReturnCode.Success);
                result.AddInt(itemList.Count);
                foreach (Item i in itemList)
                {
                    result.AddString(i.ToString());
                }
            }
            else
            {
                result = new ProtocolBytes(requestCode, EActionCode.GetAllItems, EReturnCode.Failed);
            }
            Send(conn, result);
        }

        public void UpdateItem(Conn conn, ProtocolBase protocol)
        {
            int start = sizeof(Int32) * 3;
            ProtocolBytes p = protocol as ProtocolBytes;
            int playerid = conn.player.ID;
            int itemid = p.GetInt(start, ref start);
            int count = p.GetInt(start, ref start);
            ProtocolBytes result;
            if (dataMgr.UpdateItem(playerid, itemid, count))
            {
                result = new ProtocolBytes(requestCode, EActionCode.UpdateItem, EReturnCode.Success);
            }
            else
            {
                result = new ProtocolBytes(requestCode, EActionCode.UpdateItem, EReturnCode.Failed);
            }
            Send(conn, result);
        }

        public void UpdateEquipmentStatus(Conn conn, ProtocolBase protocol)
        {
            int start = sizeof(Int32) * 3;
            ProtocolBytes p = protocol as ProtocolBytes;
            int playerid = conn.player.ID;
            int itemid = p.GetInt(start, ref start);
            bool isDressed = p.GetInt(start, ref start) == 1;
            ProtocolBytes result;
            if (dataMgr.UpdateEquipmentStatus(playerid, itemid, isDressed))
            {
                result = new ProtocolBytes(requestCode, EActionCode.UpdateEquipmentStatus, EReturnCode.Success);
            }
            else
            {
                result = new ProtocolBytes(requestCode, EActionCode.UpdateEquipmentStatus, EReturnCode.Failed);
            }
            Send(conn, result);
        }

        public void UpdateGold(Conn conn, ProtocolBase protocol)
        {
            int start = sizeof(Int32) * 3;
            ProtocolBytes p = protocol as ProtocolBytes;
            int playerid = conn.player.ID;
            int gold = p.GetInt(start, ref start);
            ProtocolBytes result;
            if (dataMgr.UpdateGold(playerid, gold))
            {
                result = new ProtocolBytes(requestCode, EActionCode.UpdateGold, EReturnCode.Success);
            }
            else
            {
                result = new ProtocolBytes(requestCode, EActionCode.UpdateGold, EReturnCode.Failed);
            }
            Send(conn, result);
        }
    }
}
