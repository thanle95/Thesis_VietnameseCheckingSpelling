using System.Windows.Forms;

namespace Spell.Algorithm
{
    interface ISpannedCell
    {
        int ColumnSpan { get; }
        int RowSpan { get; }
        DataGridViewCell OwnerCell { get; }
    }
}