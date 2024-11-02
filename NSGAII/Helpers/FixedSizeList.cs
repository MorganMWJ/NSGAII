using NSGAII.Extensions;
using System.Drawing;

namespace NSGAII.Helpers;
public class FixedSizeList<T> : List<T>
{
    private readonly int _maxSize;    

    public FixedSizeList(int maxSize) : base(maxSize)
    {
        if (maxSize <= 0) throw new ArgumentOutOfRangeException(nameof(maxSize));

        _maxSize = maxSize;
    }

    public int mAXSize => _maxSize;

    public bool IsFull => Count >= _maxSize;

    public new void Add(T item)
    {
        if (Count >= _maxSize)
        {
            throw new InvalidOperationException("The list has reached its maximum capacity.");
        }
        base.Add(item);
    }

    public void AddRange(FixedSizeList<T> items)
    {
        if (Count + items.Count > _maxSize)
        {
            throw new InvalidOperationException("Range add will push list over its maximum capacity.");
        }

        base.AddRange(items);
    }

    public void AddRange(IList<T> items)
    {
        if ((Count + items.Count) > _maxSize)
        {
            throw new InvalidOperationException("Range add will push list over its maximum capacity.");
        }

        base.AddRange(items);
    }
}
