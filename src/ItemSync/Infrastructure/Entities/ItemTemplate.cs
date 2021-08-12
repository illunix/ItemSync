using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemSync.Infrastructure.Entities
{
    public record ItemTemplate(
        int Entry,
        int Class,
        int SubClass,
        int SoundOverrideSubClass,
        int Material,
        int DisplayId,
        int InventoryType,
        int Sheath
    );
}
