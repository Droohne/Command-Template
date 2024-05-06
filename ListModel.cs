using System.Collections.Generic;

namespace LimitedSizeStack;

public interface ICommand<TList>
{
	string Name { get; }
	List<TList> StartedValues { get; set; }

	void Execute();
}


public class AddItemCommand<T> : ICommand<T>
{
	public List<T> Items; // set is redundant i guess
	public LimitedCommandHistory<ICommand<T>> History;
	public T Item;
	public string Name { get => $"Add {Item}"; }
	public List<T> StartedValues { get; set; }

	public AddItemCommand(List<T> Items, T addedItem, LimitedCommandHistory<ICommand<T>> currentHistory)
	{
		StartedValues = new List<T>(Items);
		this.Items = Items;
		History = currentHistory;
		Item = addedItem;
	}

	public void Execute()
	{
		Items.Add(Item);
		History.Push(this); // ?
	}
}


public class RemoveItemCommand<T> : ICommand<T>
{
	public int index;
	public List<T> Items;
	public LimitedCommandHistory<ICommand<T>> History;
	public List<T> StartedValues { get; set; }
	public string Name { get => $"Remove {index}"; }

	public RemoveItemCommand(List<T> Items, int index, LimitedCommandHistory<ICommand<T>> currentHistory)
	{
		StartedValues = new List<T>(Items);
		this.Items = Items;
		History = currentHistory;
		this.index = index;
	}
	public void Execute()
	{
		Items.RemoveAt(index);
		History.Push(this);
	}
}


public class UndoCommand<T> : ICommand<T>
{
	public List<T> StartedValues { get; set; }
	public string Name { get => "Undo"; } // can undo the undo? xd (no)
	public List<T> Items;
	public LimitedCommandHistory<ICommand<T>> History;

	public UndoCommand(List<T> Items, LimitedCommandHistory<ICommand<T>> history)
	{
		StartedValues = new List<T>(Items);
		this.Items = Items;
		History = history;
	}

	public void Execute()
	{
		ICommand<T> command = History.Pop();
		if (command.Name != "Undo")
			Items = command.StartedValues;
	}
}


public class LimitedCommandHistory<T>
{
	public LimitedSizeStack<T> Stack;
	public LimitedCommandHistory(int size)
	{
		Stack = new LimitedSizeStack<T>(size);
	}
	public void Push(T item)
	{
		Stack.Push(item);
	}
	public T Pop()
	{
		var t = Stack.Pop();
		return t;
	}

}


public class ListModel<TItem>
{
	public List<TItem> Items { get; set; }
	private LimitedCommandHistory<ICommand<TItem>> History;
	public int UndoLimit;
	private int Count = 0;

	public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
	{
	}

	public ListModel(List<TItem> items, int undoLimit)
	{
		Items = items;
		UndoLimit = undoLimit;
		History = new LimitedCommandHistory<ICommand<TItem>>(undoLimit);
	}

	public void AddItem(TItem item)
	{
		if (Count < UndoLimit)
			Count++;
		var command = new AddItemCommand<TItem>(Items, item, History);
		command.Execute();
	}

	public void RemoveItem(int index)
	{
		if (Count < UndoLimit)
			Count++;
		var command = new RemoveItemCommand<TItem>(Items, index, History);
		command.Execute();
	}

	public bool CanUndo()
	{
		return Count > 0;
	}

	public void Undo()
	{
		if (CanUndo())
		{
			Count--;
			var command = new UndoCommand<TItem>(Items, History);
			command.Execute();
			Items = command.Items;
			//Restore previous condition
		}
	}
}