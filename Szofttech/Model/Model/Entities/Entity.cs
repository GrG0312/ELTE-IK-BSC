using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;

namespace Bomberman_Prototype1.Model.Entities;

public class ModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] String? property = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}
public abstract class Entity : ModelBase
{
    protected int row, col, y, x;
    
    public int X
    {
        get
        {
            return x;
        }
        protected set
        {
            if (x != value)
            {
                x = value;
                OnPropertyChanged(nameof(X));
            }
        }
    }
    public int Y
    {
        get
        {
            return y;
        }
        protected set
        {
            if (y != value)
            {
                y = value;
                OnPropertyChanged(nameof(Y));
            }
        }
    }
    public virtual int Row
    {
        get
        {
            return row;
        }
        protected set
        {
            if (row == 0) 
            { 
                row = value; 
            }
            
        }
    }
    public virtual int Col
    {
        get
        {
            return col;
        }
        protected set
        {
            if (col == 0) 
            { 
                col = value;
            }
        }
    }
    public string SpritePath { get; private set; }
    public Entity(int col, int row, string spritePath)
    {
        Row = row;
        Col = col;
        X = col * 70 - 15;
        Y = row * 70;
        
        SpritePath = spritePath;
    }
}