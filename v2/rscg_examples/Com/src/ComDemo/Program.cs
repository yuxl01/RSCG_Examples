﻿using test;

IShellExecute shellExecute = new ShellExecuteClass();

// Open a file using the default associated program
IntPtr result = shellExecute.ShellExecute(
    IntPtr.Zero, // HWND (handle to parent window)
    "open",      // Operation to perform
    "notepad.exe", // File to open (replace with your file or URL)
    "",           // Parameters
    "",           // Working directory
    1            // Show command (SW_SHOWNORMAL)
);

Console.WriteLine($"ShellExecute Result: {result}");
