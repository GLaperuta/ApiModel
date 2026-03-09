using System;
using System.Collections.Generic;
using System.Text;

namespace MyApi.Domain.Models;


public sealed class TodoItem
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; } = string.Empty;
    public bool Done { get; private set; }
    public Guid Version { get; private set; } = Guid.NewGuid();

    private TodoItem() { } // EF
    public TodoItem(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.");
        Title = title.Trim();
    }

    public void MarkDone()
    {
        if (!Done)
        {
            Done = true;
            Touch();
        }
    }

    public void Rename(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title is required.");
        Title = title.Trim();
        Touch();
    }

    private void Touch() => Version = Guid.NewGuid();
}